using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System.Xml.Linq;

namespace Bb.Dacpacs
{

    public partial class DacDataSchemaModel : ModelBase
    {


        public string GetToString()
        {

            var txtDac = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                + Environment.NewLine
                + this.Serialize()
                .ToString(SaveOptions.OmitDuplicateNamespaces)
                .Replace(" xmlns=\"\"", "");

            return txtDac;

        }

        public DacDataSchemaModel ForeignKey(string @namespace, string constraintName, string tableName, string[] columns, string remoteTableName, string[] remoteColumns)
        {

            if (string.IsNullOrEmpty(@namespace))
                @namespace = "dbo";

            if (string.IsNullOrEmpty(constraintName))
                throw new ArgumentNullException(nameof(constraintName));

            if (string.IsNullOrEmpty(remoteTableName))
                throw new ArgumentNullException(nameof(remoteTableName));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (columns == null || columns.Length == 0)
                throw new ArgumentNullException(nameof(columns));

            if (remoteColumns == null || remoteColumns.Length == 0)
                throw new ArgumentNullException(nameof(remoteColumns));

            if (remoteColumns.Length != remoteColumns.Length)
                throw new ArgumentException(nameof(remoteColumns), $"{nameof(remoteColumns)} and {nameof(columns)} haven't the same length. ");

            @namespace = @namespace.Trim();
            constraintName = constraintName.Trim();
            tableName = tableName.Trim();
            remoteTableName = remoteTableName.Trim();

            this.Model.SqlForeignKeyConstraint($"[{@namespace}].[{constraintName}]", p =>
            {

                p.Relationship(RelationshipNamePropertyValue.Columns, r1 =>
                {
                    foreach (var item in columns)
                        r1.Entry(e1 =>
                        {
                            e1.References($"[{@namespace}].[{tableName}].[{item.Trim()}]");
                        });
                });

                p.Relationship(RelationshipNamePropertyValue.DefiningTable, r2 =>
                {
                    r2.Entry(e1 =>
                    {
                        e1.References($"[{@namespace}].[{tableName}]");
                    });
                });

                p.Relationship(RelationshipNamePropertyValue.ForeignColumns, r3 =>
                {
                    foreach (var item in remoteColumns)
                    {
                        r3.Entry(e1 =>
                        {
                            e1.References($"[{@namespace}].[{remoteTableName}].[{item.Trim()}]");
                        });
                    }
                });

                p.Relationship(RelationshipNamePropertyValue.ForeignTable, r4 =>
                {
                    r4.Entry(e1 =>
                    {
                        e1.References($"[{@namespace}].[{remoteTableName}]");
                    });
                });

            });

            return this;

        }

        public DacDataSchemaModel PrimaryKey(string @namespace, string table, string onpartitionSchemeName, bool isClustered, (string, SortIndex)[] fields, IndexProperties indexProperties)
        {

            if (string.IsNullOrEmpty(@namespace))
                @namespace = "dbo";

            if (string.IsNullOrEmpty(@table))
                throw new ArgumentNullException(nameof(table));

            if (fields == null || fields.Length == 0)
                throw new ArgumentNullException(nameof(fields));

            if (string.IsNullOrEmpty(onpartitionSchemeName))
                onpartitionSchemeName = "PRIMARY";

            @namespace = @namespace.Trim();
            table = table.Trim();

            this.Model.SqlPrimaryKeyConstraint(p =>
            {

                if (indexProperties != null)
                {

                    if (!isClustered)
                        p.Property("IsClustered", "False");

                    if (indexProperties.PadIndex)
                        p.Property("IsPadded", "True");                  // PAD_INDEX

                    if (indexProperties.StatisticsNorecompute)
                        p.Property("DoRecomputeStatistics", "True");    // STATISTICS_NORECOMPUTE

                    if (!indexProperties.AllowRowLocks)
                        p.Property("DoAllowRowLocks", "False");          // ALLOW_ROW_LOCKS

                    if (!indexProperties.AllowPageLocks)
                        p.Property("DoAllowPageLocks", "False");         // ALLOW_PAGE_LOCKS

                    if (indexProperties.OptimizeForSequentialKey)
                        p.Property("DoOptimizeForSequentialKey", "True");         // ALLOW_PAGE_LOCKS
                }

                //indexProperties.SortInTempdb = false;
                //indexProperties.DropExisting = false;
                //indexProperties.Online = false;

                p.Relationship(RelationshipNamePropertyValue.ColumnSpecifications,
                    r =>
                    {
                        foreach (var field in fields)
                            r.Entry(e => e.SqlIndexedColumnSpecification(e2 =>
                            {

                                if (field.Item2 == SortIndex.Descending)
                                    e2.Property("IsAscending", "False");

                                e2.Relationship(RelationshipNamePropertyValue.Column,
                                    r2 =>
                                    {
                                        r2.Entry(e3 =>
                                        {
                                            e3.References($"[{@namespace}].[{table}].[{field.Item1.Trim()}]");
                                        });
                                    });

                            }
                        ));
                    }
                )
                .Relationship(RelationshipNamePropertyValue.DefiningTable,
                    r => r.Entry(e => e.References($"[{@namespace}].[{table}]"))
                )

                .Relationship(RelationshipNamePropertyValue.Filegroup,
                    r => r.Entry(e => e.References($"[{onpartitionSchemeName}]", "BuiltIns"))
                )

                //.Annotation(AnnotationTypePropertyValue.SqlInlineConstraintAnnotation, a =>
                //{
                //    a.Disambiguator = 3;
                //})
                ;
            });

            return this;

        }

        public DacDataSchemaModel FileGroup(string fileGroup)
        {

            this.Model.Filegroup(p =>
            {

                p.Name = $"[{fileGroup}]";

            });

            return this;

        }

        public DacDataSchemaModel Schema(string SchemaName, string? authorisationName)
        {


            if (string.IsNullOrEmpty(authorisationName) || string.IsNullOrWhiteSpace(authorisationName))
                authorisationName = "dbo";

            this.Model.Schema(p =>
            {

                p.Name = $"[{SchemaName}]";

                p.Relationship(RelationshipNamePropertyValue.Authorizer, r =>
                {
                    r.Entry(e =>
                    {
                        e.References($"[{authorisationName}]", "BuiltIns");
                    });
                });

            });

            return this;

        }

        public DacDataSchemaModel Index(string name, string @namespace, string table, string onpartitionSchemeName, bool isClustered, bool isUnique, (string, SortIndex)[] fields, IndexProperties indexProperties)
        {

            if (string.IsNullOrEmpty(@namespace))
                @namespace = "dbo";

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), $"Index {nameof(name)} in table '{table}' can't be null or empty.");

            if (string.IsNullOrEmpty(@table))
                throw new ArgumentNullException(nameof(table));

            if (fields == null || fields.Length == 0)
                throw new ArgumentNullException(nameof(fields));

            if (string.IsNullOrEmpty(onpartitionSchemeName))
                onpartitionSchemeName = "PRIMARY";
            else 
                onpartitionSchemeName = onpartitionSchemeName.Trim('[', ']');

            @namespace = @namespace.Trim();
            table = table.Trim();

            this.Model.SqlIndex(p =>
            {

                p.Name = $"[{@namespace}].[{table}].[{name}]";

                if (indexProperties != null)
                {

                    if (!isClustered)
                        p.Property("IsClustered", "False");

                    if (isUnique)
                        p.Property("IsUnique", "True");

                    if (indexProperties.PadIndex)
                        p.Property("IsPadded", "True");                     // PAD_INDEX
                    
                    if (indexProperties.StatisticsNorecompute)
                        p.Property("DoRecomputeStatistics", "True");        // STATISTICS_NORECOMPUTE
                    
                    if (!indexProperties.AllowRowLocks)
                        p.Property("DoAllowRowLocks", "False");             // ALLOW_ROW_LOCKS
                    
                    if (!indexProperties.AllowPageLocks)
                        p.Property("DoAllowPageLocks", "False");            // ALLOW_PAGE_LOCKS
                    
                    if (indexProperties.OptimizeForSequentialKey)
                        p.Property("DoOptimizeForSequentialKey", "True");   // ALLOW_PAGE_LOCKS

                    //indexProperties.SortInTempdb = false;
                    //indexProperties.DropExisting = false;
                    //indexProperties.Online = false;
                }

                p.Relationship(RelationshipNamePropertyValue.ColumnSpecifications,
                        r =>
                        {
                            foreach (var field in fields)
                                r.Entry(e => e.SqlIndexedColumnSpecification(e2 =>
                                {

                                    if (field.Item2 == SortIndex.Descending)
                                        e2.Property("IsAscending", "False");

                                    e2.Relationship(RelationshipNamePropertyValue.Column,
                                        r2 =>
                                        {
                                            r2.Entry(e3 =>
                                            {
                                                e3.References($"[{@namespace}].[{table}].[{field.Item1.Trim()}]");
                                            });
                                        });

                                }
                            ));

                        }
                    )
                    .Relationship(RelationshipNamePropertyValue.Filegroup,
                        r => r.Entry(e => e.References($"[{onpartitionSchemeName}]" /*, "BuiltIns"*/))
                    )

                    .Relationship(RelationshipNamePropertyValue.IndexedObject,
                        r => r.Entry(e => e.References($"[{@namespace}].[{table}]"))
                    )

                    //.Annotation(AnnotationTypePropertyValue.SqlInlineConstraintAnnotation, a =>
                    //{
                    //    a.Disambiguator = 3;
                    //})
                    ;
            });

            return this;

        }

        public DacDataSchemaModel Table(string @namespace, string table, string onpartitionSchemeName, Action<DacRelationship> action)
        {

            if (string.IsNullOrEmpty(@namespace))
                @namespace = "dbo";

            if (string.IsNullOrEmpty(@table))
                throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(onpartitionSchemeName))
                onpartitionSchemeName = "PRIMARY";
            else
                onpartitionSchemeName = onpartitionSchemeName.Trim('[', ']');

            this.Model.SqlTable($"[{@namespace}].[{table}]", p =>
            {
                p.Property("IsAnsiNullsOn", "True")
                .Relationship(RelationshipNamePropertyValue.Columns, action)
                .Relationship(RelationshipNamePropertyValue.Schema, r =>
                {
                    r.Entry(e =>
                    {
                        e.References($"[{@namespace}]", "BuiltIns");
                    });
                })
                .Relationship(RelationshipNamePropertyValue.Filegroup,
                    r => r.Entry(e => e.References($"[{onpartitionSchemeName}]", "BuiltIns"))
                )
                //.AttachedAnnotation(null, a =>
                //{
                //    a.Disambiguator = 5;
                //});
                ;
            });

            return this;

        }

    }

}
