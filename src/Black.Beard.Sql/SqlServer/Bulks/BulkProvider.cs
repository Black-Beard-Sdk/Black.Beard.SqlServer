using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServerStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bb.SqlServer.Bulks
{

    public static class BulkProvider
    {

        public static BulkWriter GetBulkLoader(this TableDescriptor table, ConnectionStringSetting setting)
        {
            BulkWriter loader = new BulkWriter(table, setting);
            return loader;
        }


    }


    public class BulkWriter
    {

        public BulkWriter(TableDescriptor table, ConnectionStringSetting setting)
        {
            this._table = table;
            this._setting = setting;
        }


        public void LoadDatas(DbDataReader reader)
        {

            string tempName = "#_temp_" + Guid.NewGuid().ToString("N");
            string tableSource = Writer.ToLabel(tempName);

            var sql = this._setting.CreateProcessor();

            using (var cnx = sql.GetConnexion())
            using (var transaction = cnx.BeginTransaction())
            {

                sql.ExecuteNonQuery($"INSERT TOP(0) INTO {tableSource} FROM {Writer.ToLabel(_table.Schema, _table.Name)}");

                var mappings = new List<(string, string)>();
                List<SetColumnClause> columns = new List<SetColumnClause>(_table.Columns.Count);

                foreach (var item in _table.Columns)
                {
                    mappings.Add((item.Name, item.Name));
                    columns.Add(new SetColumnClause("col1", SqlExpr.Constant("ValueCol1")));
                }

                sql.WriteBulk(reader, tableSource, mappings.ToArray());


                SqlPredicateExpr _on = null;
                var key = _table.Keys[0];
                foreach (var item in key)
                {
                    var newItem = SqlPredicateExpr.Reference("s", item.Name).Equal(SqlExpr.Reference("t", item.Name));
                    if (_on == null)
                        _on = newItem;
                    else
                        _on = SqlPredicateExpr.And(_on, newItem);
                }



                if (_on != null)
                {
                    var m = new Merge()
                    .Into(_table.Schema, _table.Name, c => { c.As("t"); })
                    .Using(tableSource, c => { c.As("s"); })
                    .On(_on)
                    .WhenMatched(c => { c.Update(columns.ToArray()); })
                    .WhenNotMatchedByTarget(c => { c.Insert(columns.ToArray()); })
                    .WhenNotMatchedBySource(c => { c.Delete(); })
                    ;

                    var o = m.ToString();

                    sql.ExecuteNonQuery(o);

                }


            }

        }



        private TableDescriptor _table;
        private readonly ConnectionStringSetting _setting;
    }



    public class MemoryDbDataReader : DbDataReader
    {


        public MemoryDbDataReader()
        {

        }

        public override object this[int ordinal] => throw new NotImplementedException();

        public override object this[string name] => throw new NotImplementedException();

        public override int Depth => throw new NotImplementedException();

        public override int FieldCount => throw new NotImplementedException();

        public override bool HasRows => throw new NotImplementedException();

        public override bool IsClosed => throw new NotImplementedException();

        public override int RecordsAffected => throw new NotImplementedException();

        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            throw new NotImplementedException();
        }
    }


}
