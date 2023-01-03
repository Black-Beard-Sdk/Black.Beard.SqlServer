namespace Bb.SqlServer.Queries
{
    public static class SqlPredicateExpression
    {

        public static UnarySqlExpression Not(this SqlExpr expr) { return SqlPredicateExpr.Not(expr); }
        public static UnarySqlExpression IsNull(this SqlExpr expr) { return SqlPredicateExpr.IsNull(expr); }
        public static UnarySqlExpression IsNotNull(this SqlExpr expr) { return SqlPredicateExpr.IsNotNull(expr); }
        public static UnarySqlExpression IsDistinctFrom(this SqlExpr expr) { return SqlPredicateExpr.IsDistinctFrom(expr); }
        public static UnarySqlExpression IsNotDistinctFrom(this SqlExpr expr) { return SqlPredicateExpr.IsNotDistinctFrom(expr); }

        public static BinarySqlExpression Add(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Add(exprLeft, exprright); }
        public static BinarySqlExpression Substract(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Substract(exprLeft, exprright); }
        public static BinarySqlExpression Divid(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Divid(exprLeft, exprright); }
        public static BinarySqlExpression Modulo(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Modulo(exprLeft, exprright); }

        public static BinarySqlExpression Equal(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Equal(exprLeft, exprright); }
        public static BinarySqlExpression NotEqual(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.NotEqual(exprLeft, exprright); }
        public static BinarySqlExpression GreatThan(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.GreatThan(exprLeft, exprright); }
        public static BinarySqlExpression NotGreatThan(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.NotGreatThan(exprLeft, exprright); }
        public static BinarySqlExpression GreatOrEqualThan(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.GreatOrEqualThan(exprLeft, exprright); }
        public static BinarySqlExpression LessThan(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.LessThan(exprLeft, exprright); }
        public static BinarySqlExpression NotLessThan(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.NotLessThan(exprLeft, exprright); }
        public static BinarySqlExpression LessOrEqualThan(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.LessOrEqualThan(exprLeft, exprright); }
        public static BinarySqlExpression And(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.And(exprLeft, exprright); }
        public static BinarySqlExpression Or(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Or(exprLeft, exprright); }
        public static BinarySqlExpression Like(this SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Like(exprLeft, exprright); }

        public static BinarySqlExpression Between(this SqlExpr expr, SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.Between(expr, exprLeft, exprright); }
        public static BinarySqlExpression NotBetween(this SqlExpr expr, SqlExpr exprLeft, SqlExpr exprright) { return SqlPredicateExpr.NotBetween(expr, exprLeft, exprright); }

        public static BinarySqlExpression In(this SqlExpr exprLeft, SqlExpressionList exprright) { return SqlPredicateExpr.In(exprLeft, exprright); }
        public static BinarySqlExpression In(this SqlExpr exprLeft, params SqlExpr[] exprright) { return SqlPredicateExpr.In(exprLeft, exprright); }

        public static Constant Constant(this int value) => SqlPredicateExpr.Constant(value);
        public static Constant Constant(this string value) => SqlPredicateExpr.Constant(value);

        public static FunctionSqlExpression Match(this SqlExpr argument) { return SqlPredicateExpr.Match(argument); }
        public static FunctionSqlExpression Exists(this SqlExpr argument) { return SqlPredicateExpr.Exists(argument); }


    }

}
