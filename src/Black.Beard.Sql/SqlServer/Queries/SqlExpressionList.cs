using System.Collections;

namespace Bb.SqlServer.Queries
{
    public class SqlExpressionList : SqlExpr, IEnumerable<SqlExpr>
    {

        public SqlExpressionList(params SqlExpr[] items)
        {
            this._list = new List<SqlExpr>(items);
        }

        private readonly List<SqlExpr> _list;

        public IEnumerator<SqlExpr> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }


        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitList(this);
        }

    }



}
