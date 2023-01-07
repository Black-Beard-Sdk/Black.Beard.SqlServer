using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Structures.Ddl
{
    public class CreateIndexes : DdlBase
    {

        public CreateIndexes(Writer writer, ScriptContext ctx) 
            : base (writer, ctx)
        {

        }


        public void Parse(DatabaseStructure structure)
        {

            Parse(structure.Tables);

        }


        public void Parse(TableListDescriptor tables)
        {

            AppendEndLine("SET ANSI_NULLS ON");
            Go();

            AppendEndLine("SET QUOTED_IDENTIFIER ON");
            Go();

            foreach (var table in tables)
                Parse(table);

        }

        public void Parse(TableDescriptor table)
        {

            var targetTable = this._ctx.CurrentState?.GetTable(table.Schema, table.Name);

            foreach (IndexDescriptor index in table.Indexes)
            {

                if (targetTable == null)
                    Parse(table, index);

                else
                {
                    var targetIndex = targetTable.GetIndex(index.Name);
                    if (targetIndex == null)
                        Parse(table, index);

                    else if (targetIndex.IsDifferent(index))
                        Parse(table, index);

                }

            }

        }

        public void Parse(TableDescriptor table, IndexDescriptor index)
        {

            Append("CREATE", Evaluate(index.Unique, UNIQUE, ""));
                      
            AppendEndLine(" ", Evaluate(index.Clustered, CLUSTERED, NONCLUSTERED), " ");

            AppendEndLine("INDEX ", AsLabel(index.Name));

            using (Indent())
            {
                AppendEndLine("ON ", AsLabel(table.Schema, table.Name));
                Parse(index);
            }
        
        }

        public void Parse(IndexDescriptor key)
        {

            using (IndentWithParentheses(true))
            {


                bool f = false;

                foreach (IndexedColumnReferenceDescriptor item in key)
                {

                    if (f)
                        AppendEndLine(", ");

                    Append(AsLabel(item.Name), " ", Evaluate(item.Sort == SortIndex.Ascending, ASC, DESC));                   
                    f = true;
                }

                AppendEndLine();

            }

            AppendEndLine("WITH");
            using (IndentWithParentheses())
            {

                AppendEndLine($"PAD_INDEX = {Evaluate(key.Properties.PadIndex)},");
                AppendEndLine($"STATISTICS_NORECOMPUTE = {Evaluate(key.Properties.StatisticsNorecompute)},");
                AppendEndLine($"IGNORE_DUP_KEY = OFF,");
                AppendEndLine($"ALLOW_ROW_LOCKS = {Evaluate(key.Properties.AllowRowLocks)},");
                AppendEndLine($"ALLOW_PAGE_LOCKS = {Evaluate(key.Properties.AllowPageLocks)}, ");
                AppendEndLine($"OPTIMIZE_FOR_SEQUENTIAL_KEY = {Evaluate(key.Properties.OptimizeForSequentialKey)}");

            }
            AppendEndLine(" ON ", AsLabel(key.PartitionSchemeName));
        }


    }


}