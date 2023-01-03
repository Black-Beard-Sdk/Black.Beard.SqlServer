namespace Bb.SqlServer.Queries
{
    public class FunctionSqlExpression : SqlExpr
    {

        public FunctionSqlExpression(string functionName, params SqlExpr[] arguments)
        {
            this.Name = functionName;
            this.Arguments = arguments;
        }

        public string Name { get; }

        public SqlExpr[] Arguments { get; }

        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitFunction(this);
        }

    }



}
