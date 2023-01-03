namespace Bb.SqlServer.Queries
{
    public class MergeClause
    {

        public MergeClause(string clause, SqlPredicateExpr? clauseSearchCondition = null)
        {
            this.Clause = clause;
            ClauseSearchCondition = clauseSearchCondition;
        }

        public string Clause { get; }

        public SqlPredicateExpr? ClauseSearchCondition { get; }

        public SetColumnClause[]? Sets { get; private set; }

        public MergeClauseActionEnumEnum KindAction { get; private set; }

        public void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitMergeClause(this);
        }

        public MergeClause Insert(params SetColumnClause[] sets)
        {
            KindAction = MergeClauseActionEnumEnum.Insert;
            this.Sets = sets;
            return this;
        }

        public MergeClause Update(params SetColumnClause[] sets)
        {
            KindAction = MergeClauseActionEnumEnum.Update;
            this.Sets = sets;
            return this;
        }

        public MergeClause Delete()
        {
            KindAction = MergeClauseActionEnumEnum.Delete;
            return this;
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

    public enum MergeClauseActionEnumEnum
    {

        Insert,
        Update,
        Delete

    }


}
