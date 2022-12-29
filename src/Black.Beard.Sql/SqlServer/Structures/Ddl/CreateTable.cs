using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System;

namespace Bb.SqlServer.Structures.Ddl
{
    public class CreateTable : DdlBase
    {

        public CreateTable(Writer writer, ScriptContext ctx) : base(writer, ctx)
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
                if (this._ctx.TargetState?.GetTable(table.Schema, table.Name) == null)
                    Parse(table);

        }

        private void Parse(TableDescriptor table)
        {

            AppendEndLine("CREATE TABLE ", AsLabel(table.Schema, table.Name));
            using (IndentWithParentheses())
            {
                AppendEndLine();
                Parse(table.Columns);
                Parse(table.Keys[0]);
            }
            AppendEndLine(" ON ", AsLabel(table.PartitionSchemeName));

            Go();

        }

        private void Parse(PrimaryKeyDescriptor key)
        {

            AppendEndLine(", ");

            AppendEndLine("CONSTRAINT ", AsLabel(key.Name), " PRIMARY KEY ", Evaluate(key.Clustered, CLUSTERED, NONCLUSTERED), " ");
                      
            using (IndentWithParentheses(true))
            {
                AppendEndLine();
                bool f = false;

                foreach (var item in key)
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

        private void Parse(ColumnListDescriptor columns)
        {

            bool f = false;

            foreach (var column in columns)
            {

                if (f)
                    AppendEndLine(", ");

                Parse(column);

                f = true;

            }

        }

        private void Parse(ColumnDescriptor column)
        {

            //    <column_definition> ::=
            //       column_name <data_type>
            Append(AsLabel(column.Name), " ");
            Parse(column.Type);


            //           [ FILESTREAM ]
            //           [ COLLATE collation_name ]
            //           [ SPARSE ]
            //           [ MASKED WITH ( FUNCTION = 'mask_function' ) ]
            //           [ [ CONSTRAINT constraint_name ] DEFAULT constant_expression ]
            //           [ NOT FOR REPLICATION ]
            //           [ GENERATED ALWAYS AS { ROW | TRANSACTION_ID | SEQUENCE_NUMBER } { START | END } [ HIDDEN ] ]

            //           [ [ CONSTRAINT constraint_name ] {NULL | NOT NULL} ]
            if (!column.AllowNull)
                Append(" NOT NULL");

            //           [ ROWGUIDCOL ]
            //           [ ENCRYPTED WITH
            //               ( COLUMN_ENCRYPTION_KEY = key_name ,
            //                 ENCRYPTION_TYPE = { DETERMINISTIC | RANDOMIZED } ,
            //                 ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
            //               ) ]
            //           [ <column_constraint> [ ,... n ] ]
            //           [ <column_index> ]
            //

        }

        private void Parse(SqlTypeDescriptor type)
        {
            //   <data_type> ::=
            //       [ type_schema_name. ] type_name
            if (!string.IsNullOrEmpty(type.TypeSchemaName))
                Append(AsLabel(type.TypeSchemaName, type.Type.SqlLabel));
            else
                Append(AsLabel(type.Type.SqlLabel));

            //           [ ( precision [ , scale ] | max |
            if (type is SqlTypeWithPrecisionDescriptor size)
            {

                //           [ IDENTITY [ ( seed , increment ) ]


                if (type is SqlTypeWithPrecisionAndScaleDescriptor scale)
                {
                    if (scale.IsIdentity)
                        Append(" IDENTITY");
                    Append("(", size.Argument1, ", ", scale.Argument2, ")");
                }
                else if (string.IsNullOrEmpty(type.XmlSchemaCollection))
                {
                    Append("(");
                    if (size.Argument1 == -1)
                        Append("MAX");
                    else
                        Append(size.Argument1);
                    Append(")");
                }
                else
                {
                    //               [ { CONTENT | DOCUMENT } ] xml_schema_collection ) ]
                    Append(type.XmlSchemaCollection);
                }


            }

        }


    }


}