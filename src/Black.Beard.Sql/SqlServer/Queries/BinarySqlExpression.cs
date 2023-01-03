namespace Bb.SqlServer.Queries
{
    public class BinarySqlExpression : SqlPredicateExpr
    {

        public BinarySqlExpression(SqlExpr left, BinaryExprEnum kind, SqlExpr right)
        {

            this.Left = left;
            this.Right = right;
            this.Kind = kind;
        }

        public SqlExpr Left { get; }
        
        public BinaryExprEnum Kind { get; }
        
        public SqlExpr Right { get; }

        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitBinary(this);
        }

    }



}
