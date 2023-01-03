using Bb.SqlServer.Structures.Dacpacs;
using System.Text;

namespace Bb.SqlServer.Queries
{
    public abstract class QueryBase
    {

        public abstract void Accept(QueryBaseVisitor visitor);


        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            Write(sb);
            return sb.ToString();

        }

        public void Write(StringBuilder sb)
        {
            var wrt = new Writer(sb);
            Write(wrt);
        }

        public void Write(Writer wrt)
        {
            var variables = new Structures.Dacpacs.Variables();
            var ctx = new Structures.Dacpacs.ScriptContext(variables);
            Write(wrt, ctx);
        }

        public void Write(Writer wrt, ScriptContext ctx)
        {
            var visitor = new QueryToString(wrt, ctx);
            visitor.Visit(this);
        }

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
