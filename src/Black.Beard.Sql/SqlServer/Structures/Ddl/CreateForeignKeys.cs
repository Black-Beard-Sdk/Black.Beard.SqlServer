using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Structures.Ddl
{
    public class CreateForeignKeys : DdlBase
    {

        public CreateForeignKeys(Writer writer, ScriptContext ctx) : base(writer, ctx)
        {

        }


        public void Parse(DatabaseStructure structure)
        {

            Parse(structure.Tables);

        }


        public void Parse(TableListDescriptor tables)
        {

            foreach (var table in tables)
                Parse(table);

        }

        public void Parse(TableDescriptor table)
        {

            var targetTable = this._ctx.CurrentState?.GetTable(table.Schema, table.Name);

            foreach (ForeignKeyDescriptor foreign in table.ForeignKeys)
            {

                if (targetTable == null)
                    Parse(table, foreign);

                else
                {
                    var targetForeign = targetTable.GetForeignKey(foreign.Name);
                    if (targetForeign == null)
                        Parse(table, foreign);

                    else if (targetForeign.IsDifferent(foreign))
                        Parse(table, foreign);

                }

            }

        }

        public void Parse(TableDescriptor table, ForeignKeyDescriptor foreign)
        {

            AppendEndLine(TextQueries.TestConstraintExists(foreign.Name));
            using (Indent())
            {
                AppendEndLine("ALTER TABLE ", AsLabel(table.Schema, table.Name));
                using (Indent())
                {
                    AppendEndLine("DROP CONSTRAINT ", AsLabel(foreign.Name));
                }
            }
            Go();

            AppendEndLine("ALTER TABLE ", AsLabel(table.Schema, table.Name));
            using (Indent())
            {

                AppendEndLine("ADD CONSTRAINT ", AsLabel(foreign.Name));
                using (Indent())
                {

                    AppendEndLine("FOREIGN KEY ");
                    using (IndentWithParentheses())
                    {
                        AppendEndLine();
                        bool f = false;
                        foreach (ColumnReferenceDescriptor column in foreign.LocalColumns)
                        {

                            if (f)
                            {
                                Append(", ");
                                AppendEndLine();
                            }
                            Append(AsLabel(column.Name));
                            f = true;
                        }
                        AppendEndLine();
                    }

                    AppendEndLine();

                    AppendEndLine("REFERENCES ", AsLabel(foreign.RemoteColumns.Schema, foreign.RemoteColumns.TableName));

                    using (IndentWithParentheses())
                    {
                        AppendEndLine();

                        bool f = false;
                        foreach (ColumnReferenceDescriptor column in foreign.RemoteColumns)
                        {
                            if (f)
                            {
                                Append(", ");
                                AppendEndLine();
                            }
                            Append(AsLabel(column.Name));
                            f = true;
                        }
                        AppendEndLine();

                    }
                    AppendEndLine();

                    if (foreign.OnDeleteCascade)
                        AppendEndLine("ON DELETE CASCADE");

                    if (foreign.OnUpdateCascade)
                        AppendEndLine("ON UPDATE CASCADE");

                }
                AppendEndLine();

            }

            Go();

        }


    }


}