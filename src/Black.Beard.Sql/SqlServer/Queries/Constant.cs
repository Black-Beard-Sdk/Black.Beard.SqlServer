namespace Bb.SqlServer.Queries
{
    public class Constant : SqlExpr
    {

        public Constant(string key)
        {
            this.Key = key;
        }

        public string Key { get; }

        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitConstant(this);
        }


    }



}
