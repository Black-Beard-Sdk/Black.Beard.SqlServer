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


        public static DatabaseStructure ResolveFromDatabase(ConnectionStringSetting setting)
        {

            DatabaseStructure str = new DatabaseStructure();

            try
            {
                str.ResolveTablesFromDatabase(setting);
                str.ResolveIndexesFromDatabase(setting);
                ResolveFileGroupsFromDatabase(setting, str);
                ResolveSchemasFromDatabase(setting, str);
                ResolveForeignkeyFromDatabase(setting, str);
            }
            catch (SqlException e)
            {

            }

            return str;

        }

        public TableDescriptor? ResolveTableFromDatabase(ConnectionStringSetting setting, string schema, string table)
        {

            var sql = setting.CreateProcessor();

            var catalog = sql.ConnectionBuilder.InitialCatalog;
            var structure = TextQueries.Structures(catalog, $"tbs.[name] = '{table}' AND OBJECT_SCHEMA_NAME(tbs.[object_id], DB_ID('$dbId')) = '{schema}'");

            foreach (var reader in sql.Read<ColumnStructures>(structure))
                ResolveTablesFromDatabase(reader);

            return GetTable(schema, table);

        }

        public void ResolveTablesFromDatabase(ConnectionStringSetting setting)
        {

            var processor = setting.CreateProcessor();

            foreach (var reader in processor.Read<ColumnStructures>(TextQueries.Structures(processor.ConnectionBuilder.InitialCatalog)))
                ResolveTablesFromDatabase(reader);

        }

        public void ResolveTablesFromDatabase(Reader<ColumnStructures> reader)
        {

            var schemaName = reader.GetString(ColumnStructures.Schema);
            var tableName = reader.GetString(ColumnStructures.TableName);

            var table = GetTable(schemaName, tableName);
            if (table == null)
                Tables.Add(table = TableDescriptor.Load(reader));

            table.AddColumns(ColumnDescriptor.Create(reader));

        }

        public void ResolveIndexesFromDatabase(ConnectionStringSetting setting, TableDescriptor table)
        {

            var processor = setting.CreateProcessor();

            var filter = $" AND index_id > 0 AND t.[type] = 'U' AND schema_name(t.schema_id) = '{table.Schema}' AND t.[name] = '{table.Name}'";
            var query = TextQueries.Indexes(filter);

            foreach (var reader in processor.Read<IndexColumns>(query))
            {

                var object_type = reader.GetString(IndexColumns.object_type);

                if (object_type == "Table")
                    table.LoadIndexes(reader);

            }
        }

        private void ResolveIndexesFromDatabase(ConnectionStringSetting setting)
        {

            var processor = setting.CreateProcessor();

            var query = TextQueries.Indexes();

            foreach (var reader in processor.Read<IndexColumns>(query))
            {

                var schemaName = reader.GetString(IndexColumns.schema_name);
                var tableNameViewName = reader.GetString(IndexColumns.tableView);
                var object_type = reader.GetString(IndexColumns.object_type);

                if (object_type == "Table")
                {

                    var table = this.GetTable(schemaName, tableNameViewName);
                    if (table != null)
                        table.LoadIndexes(reader);

                }
                else if (object_type == "View")
                {



                }
                else
                {

                }

            }
        }

        private static void ResolveForeignkeyFromDatabase(ConnectionStringSetting setting, DatabaseStructure str)
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

        private static void ResolveFileGroupsFromDatabase(ConnectionStringSetting setting, DatabaseStructure str)
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

        private static void ResolveSchemasFromDatabase(ConnectionStringSetting setting, DatabaseStructure str)
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