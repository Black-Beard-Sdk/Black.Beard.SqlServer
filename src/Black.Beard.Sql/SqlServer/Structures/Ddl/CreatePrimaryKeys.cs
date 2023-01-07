using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System;

namespace Bb.SqlServer.Structures.Ddl
{

    public class CreatePrimaryKeys : DdlBase
    {

        public CreatePrimaryKeys(Writer writer, ScriptContext ctx) : base (writer, ctx)
        {

        }


        public void Parse(DatabaseStructure structure)
        {

            CommentLine("Create primary key");

            Parse(structure.Tables);

        }


        public void Parse(TableListDescriptor tables)
        {

            AppendEndLine("SET ANSI_NULLS ON");
            Go();

            AppendEndLine("SET QUOTED_IDENTIFIER ON");
            Go();

            foreach (var table in tables)
                if (this._ctx.CurrentState != null)
                {
                    var targetTable = this._ctx.CurrentState.GetTable(table.Schema, table.Name);
                    if (targetTable != null)
                    {

                        if (table.Keys[0].IsDifferent(targetTable.Keys[0]))
                        {

                            AppendEndLine("ALTER TABLE ", AsLabel(table.Schema, table.Name));
                            using (Indent())
                                AppendEndLine("DROP ", AsLabel(table.Keys[0].Name));

                            Go();

                            Parse(table);

                        }

                    }
                }

        }

        public void Parse(TableDescriptor table)
        {

            Append("ALTER TABLE ");
            AppendEndLine(AsLabel(table.Schema, table.Name));
            using (Indent())
            {
                Parse(table.Keys[0]);
            }
            Go();



        }

        public void Parse(PrimaryKeyDescriptor key)
        {

            Append("ADD CONSTRAINT ", AsLabel(key.Name), " PRIMARY KEY ");

            AppendEndLine(Evaluate(key.Clustered, CLUSTERED, NONCLUSTERED));

            using (IndentWithParentheses())
            {
                AppendEndLine();

                bool f = false;

                foreach (var item in key)
                {

                    if (f)
                    {
                        Append(", ");
                        AppendEndLine();
                    }

                    Append(AsLabel(item.Name), " ", Evaluate(item.Sort == SortIndex.Ascending, ASC, DESC));
                    f = true;
                }

                AppendEndLine();
            }
            AppendEndLine(" WITH");
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
            ;
        }

       


    }


}