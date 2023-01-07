using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Structures.Ddl
{
    public class DropIndexes : DdlBase
    {

        public DropIndexes(Writer writer, ScriptContext ctx) : base(writer, ctx)
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

                if (targetTable != null)
                {

                    var targetIndex = targetTable.GetIndex(index.Name);

                    if (targetIndex != null && targetIndex.IsDifferent(index))
                        Parse(table, index);

                }

            }

        }

        private void Parse(TableDescriptor table, IndexDescriptor index)
        {
            AppendEndLine("DROP INDEX ", AsLabel(index.Name), " ON ", AsLabel(table.Schema, table.Name), ";");
            Go();
        }

    }


}