namespace Bb.SqlServer.Queries
{

    public abstract class QueryBaseVisitor
    {

        public virtual void Visit(QueryBase q)
        {
            q.Accept(this);
        }


        public abstract void VisitMerge(Merge q);

        public abstract void VisitBinary(BinarySqlExpression q);

        public abstract void VisitConstant(Constant q);

        public abstract void VisitFunction(FunctionSqlExpression q);

        public abstract void VisitLabelReference(SqlLabelReference q);

        public abstract void VisitList(SqlExpressionList q);

        public abstract void VisitUnary(UnarySqlExpression q);

        public abstract void VisitKeyword(Keyword q);

        public abstract void VisitMergeClause(MergeClause q);
    
    }

}