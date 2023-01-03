namespace Bb.SqlServer.Queries
{
    public class Keyword : SqlPredicateExpr
    {

        public Keyword(string keyword)
        {
            this.Key = keyword;
        }

        public string Key { get; }

        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitKeyword(this);
        }

    }



}
