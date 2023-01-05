using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServerStructures;
using System.Data.Common;

namespace Bb.SqlServer.Bulks
{
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
            string merge = string.Empty;
            SqlPredicateExpr _on = null;
            var key = _table.Keys[0];

            var sql = this._setting.CreateProcessor();

            var mappings = new List<(string, string)>();
            List<SetColumnClause> columns = new List<SetColumnClause>(_table.Columns.Count);
            foreach (var item in _table.Columns)
            {
                mappings.Add((item.Name, item.Name));
                columns.Add(new SetColumnClause("col1", SqlExpr.Constant("ValueCol1")));
            }

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

                merge = m.ToString();

            }

            using (var cnx = sql.GetConnexion())
            using (var transaction = cnx.BeginTransaction())
            {
                sql.ExecuteNonQuery($"INSERT TOP(0) INTO {tableSource} FROM {Writer.ToLabel(_table.Schema, _table.Name)}");
                sql.WriteBulk(transaction, reader, tableSource, mappings.ToArray());
                sql.ExecuteNonQuery(merge);
            }

        }


        private TableDescriptor _table;
        private readonly ConnectionStringSetting _setting;
    }


}
