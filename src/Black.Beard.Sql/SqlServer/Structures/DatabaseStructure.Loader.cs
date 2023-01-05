using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml.Linq;
using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServerStructures;
using static System.Formats.Asn1.AsnWriter;

namespace Bb.SqlServer.Structures
{

    public partial class DatabaseStructure
    {


        public static DatabaseStructure Load(ConnectionStringSetting setting)
        {

            DatabaseStructure str = new DatabaseStructure();

            try
            {
                LoadTables(setting, str);
                LoadIndexes(setting, str);
                LoadFileGroups(setting, str);
                LoadSchemas(setting, str);
                LoadForeign(setting, str);
            }
            catch (SqlException e)
            {
        
            }

            return str;

        }

        private static void LoadTables(ConnectionStringSetting setting, DatabaseStructure str)
        {

            var processor = setting.CreateProcessor();

            foreach (var reader in processor.Read<ColumnStructures>(TextQueries.Structures.Replace("$dbId", processor.ConnectionBuilder.InitialCatalog)))
            {

                var schemaName = reader.GetString(ColumnStructures.Schema);
                var tableName = reader.GetString(ColumnStructures.table_name);

                var table = str.GetTable(schemaName, tableName);
                if (table == null)
                {
                    table = new TableDescriptor() { Schema = schemaName, Name = tableName };
                    str.Tables.Add(table);
                }

                var column = new ColumnDescriptor()
                {
                    Name = reader.GetString(ColumnStructures.column_name),
                    Caption = reader.GetString(ColumnStructures.Description),
                    AllowNull = reader.GetBoolean(ColumnStructures.is_nullable),
                    Type = SqlTypeDescriptor.Create
                    (
                        reader.GetString(ColumnStructures.system_data_type),
                        reader.GetBoolean(ColumnStructures.is_identity),
                        reader.GetInt32(ColumnStructures.max_length),
                        reader.GetByte(ColumnStructures.precision),
                        reader.GetByte(ColumnStructures.scale),
                        reader.GetInt32(ColumnStructures.Seed),
                        reader.GetInt32(ColumnStructures.Increment)
                    )
                };

                table.AddColumns(column);

            }
        }

        private static void LoadForeign(ConnectionStringSetting setting, DatabaseStructure str)
        {

            var processor = setting.CreateProcessor();

            foreach (var reader in processor.Read<ForeignColumns>(TextQueries.ForeignKeys.Replace("$dbId", processor.ConnectionBuilder.InitialCatalog)))
            {

                var schemaNameFk = reader.GetString(ForeignColumns.foreign_schema);
                var tableNameFk = reader.GetString(ForeignColumns.foreign_table);
                var table = str.GetTable(schemaNameFk, tableNameFk);

                if (table == null)
                {
                    table = new TableDescriptor() { Schema = schemaNameFk, Name = tableNameFk };
                    str.Tables.Add(table);
                }

                var fk_constraint_name = reader.GetString(ForeignColumns.fk_constraint_name);
                var constraint = table.GetForeignKey(fk_constraint_name);

                if (constraint == null)
                {

                    constraint = new ForeignKeyDescriptor() { Name = fk_constraint_name };
                    constraint.RemoteColumns.Schema = reader.GetString(ForeignColumns.primary_schema);
                    constraint.RemoteColumns.TableName = reader.GetString(ForeignColumns.primary_table);
                    constraint.OnDeleteCascade = reader.GetBoolean(ForeignColumns.on_delete);
                    constraint.OnUpdateCascade = reader.GetBoolean(ForeignColumns.on_update);
                    table.AddForeignKey(constraint);

                }

                constraint.AddLocalColumns(reader.GetString(ForeignColumns.fk_column_name));
                constraint.AddRemoteColumns(reader.GetString(ForeignColumns.pk_column_name));

            }
        }

        private static void LoadIndexes(ConnectionStringSetting setting, DatabaseStructure str)
        {

            var processor = setting.CreateProcessor();

            foreach (var reader in processor.Read<IndexColumns>(TextQueries.Indexes))
            {

                var schemaName = reader.GetString(IndexColumns.schema_name);
                var tableNameViewName = reader.GetString(IndexColumns.tableView);
                var object_type = reader.GetString(IndexColumns.object_type);


                var type = reader.GetString(IndexColumns.index_type);

                if (object_type == "Table")
                {
                    var table = str.GetTable(schemaName, tableNameViewName);
                    if (table != null)
                    {

                        var is_primary = reader.GetBoolean(IndexColumns.is_primary);
                        if (is_primary)
                        {
                            var key = new PrimaryKeyDescriptor()
                            {
                                Name = reader.GetString((int)IndexColumns.name),
                                Clustered = type == "Clustered index",
                                Unique = reader.GetBoolean(IndexColumns.is_unique),
                                PartitionSchemeName = reader.GetString(IndexColumns.Filegroup),
                            };
                            Map(reader, key);
                            table.Keys.Add(key);
                        }
                        else
                        {
                            var key = new IndexDescriptor()
                            {
                                Name = reader.GetString((int)IndexColumns.name),
                                Clustered = type == "Clustered index",
                                Unique = reader.GetBoolean(IndexColumns.is_unique),
                            };
                            Map(reader, key);
                            table.Indexes.Add(key);
                        }

                    }
                }
                else if (type == "View")
                {



                }
                else
                {

                }

            }
        }

        private static void LoadFileGroups(ConnectionStringSetting setting, DatabaseStructure str)
        {

            var processor = setting.CreateProcessor();

            foreach (var reader in processor.Read<FileGroupColumns>(TextQueries.Filegroups))
            {

                var name = reader.GetString(FileGroupColumns.Name);

                var filegroup = str.GetFileGroup(name);
                if (filegroup == null)
                {
                    filegroup = new FileGroupDescriptor()
                    {
                        Name = name,
                    };
                    str.FileGroups.Add(filegroup);
                }


            }
        }

        private static void LoadSchemas(ConnectionStringSetting setting, DatabaseStructure str)
        {

            var processor = setting.CreateProcessor();

            foreach (var reader in processor.Read<SchemaColumns>(TextQueries.Schemas))
            {

                var name = reader.GetString(SchemaColumns.SCHEMA_NAME);

                var schema = str.GetSchema(name);
                if (schema == null)
                {
                    schema = new SchemaDescriptor()
                    {
                        Name = name,
                    };
                    str.Schemas.Add(schema);
                }

                schema.Parent = reader.GetString(SchemaColumns.SCHEMA_OWNER);

            }
        }

        private FileGroupDescriptor? GetFileGroup(string? name)
        {
            return this.FileGroups.FirstOrDefault(c => c.Name == name);
        }

        private SchemaDescriptor? GetSchema(string? name)
        {
            return this.Schemas.FirstOrDefault(c => c.Name == name);
        }

        private static void Map(Reader<IndexColumns> reader, IndexDescriptor key)
        {
            key.Properties.PadIndex = reader.GetBoolean(IndexColumns.is_padded);
            key.Properties.StatisticsNorecompute = reader.GetBoolean(IndexColumns.no_recompute);
            key.Properties.AllowRowLocks = reader.GetBoolean(IndexColumns.allow_row_locks);
            key.Properties.AllowPageLocks = reader.GetBoolean(IndexColumns.allow_page_locks);
            key.Properties.OptimizeForSequentialKey = reader.GetBoolean(IndexColumns.optimize_sequential_key);

            var columns = reader.GetString((int)IndexColumns.name).Split(',');
            var descendings = reader.GetString(IndexColumns.column_descendings).Split(',');

            for (int i = 0; i < columns.Length; i++)
            {

                key.Add(new IndexedColumnReferenceDescriptor()
                {
                    Name = columns[i].Trim(),
                    Sort = descendings[i].Trim() == "1" ? SortIndex.Descending : SortIndex.Ascending,
                });
            }
        }
    }

}