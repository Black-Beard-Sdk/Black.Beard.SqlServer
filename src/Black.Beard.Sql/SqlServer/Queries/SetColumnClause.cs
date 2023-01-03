namespace Bb.SqlServer.Queries
{
    public class SetColumnClause
    {


        public SetColumnClause(SqlLabelReference columnName, SqlExpr value)
        {
            this.Column = columnName;
            this.Value = value;
        }


        public SqlLabelReference Column { get; }

        public SqlExpr Value { get; }


    }



    // <merge_matched>::= { UPDATE SET <set_clause> | DELETE }  
    /*
     {        column_name = { expression | DEFAULT | NULL }  
          | { udt_column_name.{ {   property_name = expression  
                                  | field_name = expression 
                                }  
                                | method_name ( argument [ ,...n ] )  
                              }  
            }  
          |   column_name { .WRITE ( expression , @Offset , @Length ) }  
          |   @variable = expression  
          |   @variable = column = expression  
          |   column_name { += | -= | *= | /= | %= | &= | ^= | |= } expression  
          |   @variable { += | -= | *= | /= | %= | &= | ^= | |= } expression  
          |   @variable = column { += | -= | *= | /= | %= | &= | ^= | |= } expression  
     } [ ,...n ]   
    */



    /*
       <merge_not_matched>::=  
          {  
              INSERT [ ( column_list ) ]
                  { VALUES ( values_list )  
                  | DEFAULT VALUES }  
          }  
     */


}
