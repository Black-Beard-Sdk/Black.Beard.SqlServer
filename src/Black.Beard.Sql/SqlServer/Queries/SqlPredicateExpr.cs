namespace Bb.SqlServer.Queries
{


    public abstract class SqlPredicateExpr : SqlExpr
    {

        public SqlPredicateExpr()
        {

        }

        //public override void Accept(QueryBaseVisitor visitor)
        //{
        //    visitor.VisitPredicate(this);
        //}

        public static UnarySqlExpression Not(SqlExpr expr) { return new UnarySqlExpression(expr, UnaryExprEnum.Not); }
        public static UnarySqlExpression IsNull(SqlExpr expr) { return new UnarySqlExpression(expr, UnaryExprEnum.IsNull); }
        public static UnarySqlExpression IsNotNull(SqlExpr expr) { return new UnarySqlExpression(expr, UnaryExprEnum.IsNotNull); }
        public static UnarySqlExpression IsDistinctFrom(SqlExpr expr) { return new UnarySqlExpression(expr, UnaryExprEnum.IsDistinctFrom); }
        public static UnarySqlExpression IsNotDistinctFrom(SqlExpr expr) { return new UnarySqlExpression(expr, UnaryExprEnum.IsNotDistinctFrom); }

        public static BinarySqlExpression Add(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Add, exprright); }
        public static BinarySqlExpression Substract(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Substract, exprright); }
        public static BinarySqlExpression Divid(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Divid, exprright); }
        public static BinarySqlExpression Modulo(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Modulo, exprright); }

        public static BinarySqlExpression Equal(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Equal, exprright); }
        public static BinarySqlExpression NotEqual(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.NotEqual, exprright); }
        public static BinarySqlExpression GreatThan(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.GreatThan, exprright); }
        public static BinarySqlExpression NotGreatThan(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.NotGreatThan, exprright); }
        public static BinarySqlExpression GreatOrEqualThan(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.GreatOrEqualThan, exprright); }
        public static BinarySqlExpression LessThan(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.LessThan, exprright); }
        public static BinarySqlExpression NotLessThan(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.NotLessThan, exprright); }
        public static BinarySqlExpression LessOrEqualThan(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.LessOrEqualThan, exprright); }
        public static BinarySqlExpression And(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.And, exprright); }
        public static BinarySqlExpression Or(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Or, exprright); }
        public static BinarySqlExpression Like(SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.Like, exprright); }

        public static BinarySqlExpression Between(SqlExpr expr, SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(expr, BinaryExprEnum.Between, And(exprLeft, exprright)); }
        public static BinarySqlExpression NotBetween(SqlExpr expr, SqlExpr exprLeft, SqlExpr exprright) { return new BinarySqlExpression(expr, BinaryExprEnum.And, Not(And(exprLeft, exprright))); }

        public static BinarySqlExpression In(SqlExpr exprLeft, SqlExpressionList exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.In, exprright); }
        public static BinarySqlExpression In(SqlExpr exprLeft, params SqlExpr[] exprright) { return new BinarySqlExpression(exprLeft, BinaryExprEnum.In, new SqlExpressionList(exprright)); }


        public static FunctionSqlExpression Match(SqlExpr argument) { return new FunctionSqlExpression("MATCH", argument); }
        public static FunctionSqlExpression Exists(SqlExpr argument) { return new FunctionSqlExpression("EXISTS", argument); }


    }

    public enum UnaryExprEnum
    {
        Not,
        IsNull,
        IsNotNull,
        IsDistinctFrom,
        IsNotDistinctFrom,
    }

    public enum BinaryExprEnum
    {
        Equal,
        NotEqual,
        GreatThan,
        NotGreatThan,
        GreatOrEqualThan,
        LessThan,
        NotLessThan,
        LessOrEqualThan,
        And,
        Or,
        Like,
        In,

        Add,
        Substract,
        Multiply,
        Divid,
        Modulo,

        Between,
    }

}
