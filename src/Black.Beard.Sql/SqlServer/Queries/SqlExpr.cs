namespace Bb.SqlServer.Queries
{

    // https://learn.microsoft.com/en-us/sql/t-sql/language-elements/expressions-transact-sql?source=recommendations&view=sql-server-ver16

    public abstract class SqlExpr
    {

        public bool WithParenteses { get; set; } = false;

        public static Keyword DefaultValues => new Keyword("DEFAULT VALUES");

        public abstract void Accept(QueryBaseVisitor visitor);

        public static Constant Constant(int value) => new Constant(value.ToString());
        public static Constant Constant(string value) => new Constant(value);

        public static Keyword ALL() => new Keyword("ALL");
        public static Keyword SOME() => new Keyword("SOME");
        public static Keyword ANY() => new Keyword("ANY");
        public static Keyword Wildcare() => new Keyword("*");

        public static SqlLabelReference Reference(params string[] items) => SqlLabelReference.Create(items);


    }

    /*
     
     <expression> ::=   
    {  
        constant   
        | scalar_function   
        | column  
        | variable  
        | ( expression  )  
        | { unary_operator } expression   
        | expression { binary_operator } expression   
    }  
    [ COLLATE Windows_collation_name ]  
  
    -- Scalar Expression in a DECLARE, SET, IF...ELSE, or WHILE statement  
    <scalar_expression> ::=  
    {  
        constant   
        | scalar_function   
        | variable  
        | ( expression  )  
        | (scalar_subquery )  
        | { unary_operator } expression   
        | expression { binary_operator } expression   
    }  
    [ COLLATE { Windows_collation_name ]  

     */


}
