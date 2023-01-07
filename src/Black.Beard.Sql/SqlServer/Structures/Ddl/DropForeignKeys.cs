using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Structures.Ddl
{
    public class DropForeignKeys : DdlBase
    {

        public DropForeignKeys(Writer writer, ScriptContext ctx) : base (writer, ctx) 
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

            foreach (ForeignKeyDescriptor foreign in table.ForeignKeys)
            {

                if (targetTable != null)
                {

                    var targetIndex = targetTable.GetForeignKey(foreign.Name);

                    if (targetIndex != null && targetIndex.IsDifferent(foreign))
                        Parse(table, foreign);

                }

            }

        }

        public void Parse(TableDescriptor table, ForeignKeyDescriptor index)
        {
            Append("ALTER TABLE ", AsLabel(table.Schema, table.Name));
            using (Indent())
            {
                Append("DROP CONSTRAINT ", AsLabel(index.Name));
                AppendEndLine(";");
            }
            Go();

        }

    }


}