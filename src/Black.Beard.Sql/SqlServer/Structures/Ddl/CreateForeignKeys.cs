using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Structures.Ddl
{
    public class CreateForeignKeys : DdlBase
    {

        public CreateForeignKeys(Writer writer, ScriptContext ctx) : base (writer, ctx)
        {

        }


        internal void Parse(DatabaseStructure structure)
        {

            Parse(structure.Tables);

        }


        private void Parse(TableListDescriptor tables)
        {

            AppendEndLine("SET ANSI_NULLS ON");
            Go();

            AppendEndLine("SET QUOTED_IDENTIFIER ON");
            Go();

            foreach (var table in tables)
                Parse(table);

        }

        private void Parse(TableDescriptor table)
        {

            var targetTable = this._ctx.TargetState?.GetTable(table.Schema, table.Name);

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

        private void Parse(TableDescriptor table, ForeignKeyDescriptor foreign)
        {

            /*
             * 
    ALTER TABLE Sales.TempSalesReason
        ADD CONSTRAINT FK_TempSales_SalesReason 
            FOREIGN KEY 
            (
                TempID
            )
            REFERENCES Sales.SalesReason 
            (
                SalesReasonID
            )
            ON DELETE CASCADE
            ON UPDATE CASCADE
;
             
             */

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
                }
                AppendEndLine();

            }

            Go();

        }


    }


}