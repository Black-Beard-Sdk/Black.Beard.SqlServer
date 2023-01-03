namespace Bb.SqlServer.Queries
{
    public class UnarySqlExpression : SqlExpr
    {

        public UnarySqlExpression(SqlExpr child, UnaryExprEnum kind)
        {

            this.Child = child;
            this.Kind = kind;
        }

        public SqlExpr Child { get; }

        public UnaryExprEnum Kind { get; }

        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitUnary(this);
        }


    }



}
