using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System;

namespace Bb.SqlServer.Structures.Ddl
{

    public class CreateTables : DdlBase
    {

        public CreateTables(Writer writer, ScriptContext? ctx = null) : base(writer, ctx)
        {

        }


        public void Parse(DatabaseStructure structure)
        {

            CommentLine("Create tables");

            Parse(structure.Tables);

        }


        public void Parse(TableListDescriptor tables)
        {

            AppendEndLine("SET ANSI_NULLS ON");
            Go();

            AppendEndLine("SET QUOTED_IDENTIFIER ON");
            Go();

            foreach (var table in tables)
                if (_ctx.CreateAllTables || this._ctx.CurrentState?.GetTable(table.Schema, table.Name) == null)
                    Parse(table);

        }

        public void Parse(TableDescriptor table)
        {

            AppendEndLine(TextQueries.TestTableNotExists(table.Schema, table.Name));
            using (Indent())
            {

                AppendEndLine("CREATE TABLE ", AsLabel(table.Schema, table.Name));
                
                using (IndentWithParentheses())
                {
                    AppendEndLine();
                    Parse(table.Columns);
                    if (table.Keys.Count > 0)
                        Parse(table.Keys[0]);
                    AppendEndLine();
                }

                AppendEndLine(" ON ", AsLabel(table.PartitionSchemeName));

            }

            Go();

        }

        public void Parse(PrimaryKeyDescriptor key)
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

        public void Parse(ColumnListDescriptor columns)
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

        public void Parse(ColumnDescriptor column)
        {

            //    <column_definition> ::=
            //       column_name <data_type>
            Append(AsLabel(column.Name), " ");
            Parse(column.SqlType);


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

        public void Parse(SqlTypeDescriptor type)
        {
            //   <data_type> ::=
            //       [ type_schema_name. ] type_name
            if (!string.IsNullOrEmpty(type.TypeSchemaName))
                Append(AsLabel(type.TypeSchemaName, type.SqlDataType.SqlLabel));
            else
                Append(AsLabel(type.SqlDataType.SqlLabel));

            //           [ ( precision [ , scale ] | max |
            if (type .Argument1.HasValue)
            {

                //           [ IDENTITY [ ( seed , increment ) ]


                if (type.Argument2.HasValue)
                {
                    if (type.IsIdentity)
                        Append(" IDENTITY");

                    Append("(", type.Argument1, ", ", type.Argument2, ")");

                }
                else if (string.IsNullOrEmpty(type.XmlSchemaCollection))
                {
                    Append("(");
                    if (type.Argument1 == -1)
                        Append("MAX");
                    else
                        Append(type.Argument1);
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