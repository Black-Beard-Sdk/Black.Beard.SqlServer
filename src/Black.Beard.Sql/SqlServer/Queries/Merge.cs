using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bb.SqlServer.Queries
{

    public class Merge : QueryBase
    {


        /// <summary>
        /// Clean top limit value
        /// </summary>
        /// <returns></returns>
        public Merge Top()
        {
            this.TopClause = null;
            return this;
        }

        /// <summary>
        /// Set top limit value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Merge Top(int value, TopModeEnum mode = TopModeEnum.Count)
        {
            this.TopClause = new Top() { Limit = value, Mode = mode };
            return this;
        }



        public Merge Into(string targetTableReference, Action<TargetTable>? actionOnTargetTable = null)
        {

            this.IntoClause = new TargetTable(targetTableReference);
            if (actionOnTargetTable != null)
                actionOnTargetTable(this.IntoClause);
            return this;

        }

        public Merge Into(string targetSchemaReference, string targetTableReference, Action<TargetTable>? actionOnTargetTable = null)
        {
            this.IntoClause = new TargetTable(targetSchemaReference, targetTableReference);
            if (actionOnTargetTable != null)
                actionOnTargetTable(this.IntoClause);
            return this;
        }

        public Merge Into(string targetDatabaseReference, string targetSchemaReference, string targetTableReference, Action<TargetTable> actionOnTargetTable = null)
        {
            this.IntoClause = new TargetTable(targetDatabaseReference, targetSchemaReference, targetTableReference);
            if (actionOnTargetTable != null)
                actionOnTargetTable(this.IntoClause);
            return this;
        }



        public Merge Using(string sourceTableReference, Action<SourceTable>? actionOnSourceTable = null)
        {
            this.UsingClause = new SourceTable(sourceTableReference);
            if (actionOnSourceTable != null)
                actionOnSourceTable(this.UsingClause);
            return this;
        }

        public Merge Using(string sourceSchemaReference, string sourceTableReference, Action<SourceTable> actionOnSourceTable = null)
        {
            this.UsingClause = new SourceTable(sourceSchemaReference, sourceTableReference);
            if (actionOnSourceTable != null)
                actionOnSourceTable(this.UsingClause);
            return this;
        }

        public Merge Using(string sourceDatabaseReference, string sourceSchemaReference, string sourceTableReference, Action<SourceTable> actionOnSourceTable = null)
        {
            this.UsingClause = new SourceTable(sourceDatabaseReference, sourceSchemaReference, sourceTableReference);
            if (actionOnSourceTable != null)
                actionOnSourceTable(this.UsingClause);
            return this;
        }



        public Merge On(SqlPredicateExpr expression)
        {
            this.OnClause = expression;
            return this;
        }



        public Merge WhenMatched(SqlPredicateExpr? clause_search_condition, Action<MergeClause> action)
        {

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            this.ClauseMatched = new MergeClause("WHEN MATCHED", clause_search_condition);

            action(this.ClauseMatched);

            // <merge_matched>::= { UPDATE SET <set_clause> | DELETE }  

            return this;

        }

        public Merge WhenMatched(Action<MergeClause> action)
        {
            WhenMatched(null, action);
            return this;
        }

        public Merge WhenNotMatchedByTarget(SqlPredicateExpr? clause_search_condition, Action<MergeClause> action)
        {

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            ClauseNotMatchedByTarget = new MergeClause("WHEN NOT MATCHED BY TARGET", clause_search_condition);
            action(this.ClauseNotMatchedByTarget);

            /*
             <merge_not_matched>::=  
                {  
                    INSERT [ ( column_list ) ]
                        { VALUES ( values_list )  
                        | DEFAULT VALUES }  
                }  
             */

            return this;

        }

        public Merge WhenNotMatchedByTarget(Action<MergeClause> action)
        {
            WhenNotMatchedByTarget(null, action);
            return this;
        }


        public Merge WhenNotMatchedBySource(SqlPredicateExpr? clause_search_condition, Action<MergeClause> action)
        {

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            ClauseNotMatchedBySource = new MergeClause("WHEN NOT MATCHED BY SOURCE", clause_search_condition);
            action(this.ClauseNotMatchedBySource);

            // <merge_matched>::= { UPDATE SET <set_clause> | DELETE }  


            return this;

        }

        public Merge WhenNotMatchedBySource(Action<MergeClause> action)
        {
            WhenNotMatchedBySource(null, action);
            return this;
        }


        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitMerge(this);
        }



        // [ <output_clause> ]  
        // [ OPTION ( <query_hint> [ ,...n ] ) ]

        public Top? TopClause { get; private set; }
        public TargetTable? IntoClause { get; private set; }
        public SourceTable? UsingClause { get; private set; }
        public SqlPredicateExpr? OnClause { get; private set; }

        public MergeClause? ClauseMatched { get; private set; }
        public MergeClause? ClauseNotMatchedBySource { get; private set; }
        public MergeClause? ClauseNotMatchedByTarget { get; private set; }

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
