using Bb.Extended;
using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Ddl;
using Bb.SqlServerStructures;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Bb.SqlServer.Bulks
{
    public class BulkWriter
    {

        public BulkWriter(ConnectionStringSetting setting)
        {
            this._setting = setting;
        }


        public void Write(DbDataReader reader, string schema, string table)
        {

            SqlProcessor sql = this._setting.CreateProcessor();
            using (var cnx = sql.GetConnexion())
            using (var transaction = cnx.BeginTransaction())
            {

                Write(reader, schema, table, sql, transaction);

                transaction.Commit();

            }

        }

        public void Write(DbDataReader reader, string schema, string table, SqlProcessor sql, SqlTransaction transaction)
        {

            string tableSource = TableDescriptor.GetRandomTableName();
            var structure = new DatabaseStructure();

            #region CreateStagingTable

            var _tableTarget = structure.ResolveTableFromDatabase(_setting, schema, table);
            structure.ResolveIndexesFromDatabase(_setting, _tableTarget);

            var _tableSource = _tableTarget.Clone(tableSource);
            _tableSource.Columns.For(c =>
            {
                if (c.SqlType.IsIdentity)
                {
                    c.SqlType.IsIdentity = false;
                    c.SqlType.Argument1 = null;
                    c.SqlType.Argument2 = null;
                }
                c.AllowNull = true;
            });

            var sb = new System.Text.StringBuilder();
            _tableSource.GetScriptToCreate(sb);
            sql.ExecuteNonQuery(sb.ToString());

            #endregion CreateStagingTable


            #region Load Data

            var mappings = new List<(string, string)>();
            List<SetColumnClause> columns = new List<SetColumnClause>(_tableTarget.Columns.Count);
            HashSet<string> _key = new HashSet<string>();

            foreach (var item in _tableTarget.Keys[0])
                _key.Add(item.Name);

            for (int i = 0; i < _tableTarget.Columns.Count; i++)
            {
                mappings.Add((_tableTarget.Columns[i].Name, _tableSource.Columns[i].Name));
                if (!_key.Contains(_tableTarget.Columns[i].Name))
                    columns.Add(new SetColumnClause(SqlExpr.Reference(_tableTarget.Columns[i].Name), SqlExpr.Reference("s", _tableSource.Columns[i].Name)));
            }


            var _sb = new StringBuilder();
            var wrt = new Writer(_sb);
            int count = 0;
            string comma3 = string.Empty;

            while (reader.Read())
            {

                count++;

                if (_sb.Length == 0)
                {
                    wrt.AppendEndLine($"INSERT INTO {Writer.ToLabel(_tableSource.Schema, _tableSource.Name)}");
                    using (wrt.IndentWithParentheses())
                    {
                        string comma = string.Empty;
                        foreach (var item in _tableSource.Columns)
                        {
                            wrt.AppendEndLine(comma);
                            wrt.Append(Writer.ToLabel(item.Name));
                            comma = ", ";
                        }
                    }

                    wrt.AppendEndLine($"VALUES");

                    wrt.AddIndent();

                }

                string comma2 = string.Empty;
                wrt.AppendEndLine(comma3);
                wrt.Append("(");

                for (int i = 0; i < reader.FieldCount; i++)
                {

                    wrt.Append(comma2);
                    var j = reader.GetValue(i);
                    if (j == null)
                        wrt.Append("NULL");

                    else
                    {
                        if (j.GetType().IsEnum)
                        {
                            wrt.Append($"{(int)j}");

                        }

                        else if (j is int int1)
                            wrt.Append($"{int1}");

                        else if (j is long int2)
                            wrt.Append($"{int2}");

                        else if (j is short int3)
                            wrt.Append($"{int3}");

                        else if (j is string s)
                            wrt.Append($"'{j}'");

                        else
                        {
                            wrt.Append(j.ToString());
                        }
                    }

                    comma2 = ", ";

                }
                wrt.Append(")");
                comma3 = ", ";

                /*
                VALUES ( dbo.CreateNewPoint(x, y) );  

                 */
                if (count > 500)
                {
                    wrt.DelIndent();
                    wrt.AppendEndLine();
                    wrt.AppendEndLine(";");
                    sql.ExecuteNonQuery(_sb.ToString());
                    _sb.Clear();
                    count = 0;
                }

            }
            if (count > 0)
            {
                wrt.DelIndent();
                wrt.AppendEndLine(); 
                wrt.AppendEndLine(";");
                sql.ExecuteNonQuery(_sb.ToString());
            }


            //sql.WriteBulk(transaction, reader, tableSource, mappings.ToArray());

            #endregion Load Data


            SqlPredicateExpr _on = null;
            PrimaryKeyDescriptor key = _tableTarget.Keys[0];
            foreach (IndexedColumnReferenceDescriptor item in key)
            {
                var newItem = SqlPredicateExpr.Reference("s", item.Name).Equal(SqlExpr.Reference("t", item.Name));
                if (_on == null)
                    _on = newItem;
                else
                    _on = SqlPredicateExpr.And(_on, newItem);
            }


            var index = new IndexDescriptor()
            {
                Name = "idx_" + _tableSource.Name,
                Unique = false,
                Clustered = false,
            }
            .CopyProperty(key.Properties)
            .CopyColumns(key);
            sql.ExecuteNonQuery(index.GetScriptToCreate(_tableSource));



            var m = new Merge()
            .Into(_tableTarget.Schema, _tableTarget.Name, c => { c.As("t"); })
            .Using(tableSource, c => { c.As("s"); })
            .On(_on)
            .WhenMatched(c => { c.Update(columns.ToArray()); })
            .WhenNotMatchedByTarget(c => { c.Insert(columns.ToArray()); })
            .WhenNotMatchedBySource(c => { c.Delete(); })
            ;

            var merge = m.ToString();

            ContainerTree c = new ContainerTree();
            foreach (var item in sql.Read(merge))
            {

                ContainerTree current = c;

                for (int i = 0; i < item.FieldCount; i++)
                {
                    var value = item.GetValue(i);
                    current = c.Map(value);
                }

                current.Id = item.GetValue(0);

            }

            //sql.ExecuteNonQuery(_tableSource.GetScriptToDrop());
        }

        public void ReloadIds<T>(DbDataReaderIdResolver<T> mappings, SqlProcessor sql, string schema, string table)
        {



            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            string comma = string.Empty;
            foreach (var item in mappings.Columns)
            {
                sb2.Append(comma);
                sb2.Append($"[{item}]");
                sb.Append($", [{item}]");
                comma = ", ";
            }
            string query = $"SELECT [{mappings.IdColumn}]{sb} FROM [{schema}].[{table}] ORDER BY {sb2}";


            ContainerTree c = new ContainerTree();
            foreach (var item in sql.Read(query))
            {

                ContainerTree current = c;

                for (int i = 1; i < item.FieldCount; i++)
                {

                    var value = item.GetValue(i);
                    current = c.Map(value);
                }

                current.Id = item.GetValue(0);

            }

            mappings.Map(c);

        }

        private readonly ConnectionStringSetting _setting;

    }

    internal class ContainerTree
    {

        public ContainerTree()
        {
            _map = new Dictionary<object, ContainerTree>();

        }

        public ContainerTree(object value)
        {
            _map = new Dictionary<object, ContainerTree>();
            this.Value = value;
        }

        internal ContainerTree Map(object value)
        {

            if (_map.TryGetValue(value, out var c))
                return c;

            _map.Add(value, c);

            return c;

        }

        internal ContainerTree? Get(object value)
        {

            if (this._map.TryGetValue(value, out var c))
            {

                return c;

            }

            return null;

        }

        private readonly Dictionary<object, ContainerTree> _map;

        public object Value { get; }
        public object Id { get; internal set; }
    }


}
