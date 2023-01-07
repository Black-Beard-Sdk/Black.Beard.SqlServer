using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures.Ddl;
using Bb.SqlServerStructures;
using System.Text;

namespace Bb.SqlServer.Structures
{

    public class TableDescriptor : SqlServerDescriptor
    {


        #region Ctors

        public TableDescriptor()
        {
            Columns = new ColumnListDescriptor();
            Keys = new PrimaryKeyListDescriptor();
            Indexes = new IndexListDescriptor();
            ForeignKeys = new ForeignKeyListDescriptor();
        }

        public TableDescriptor(string? schema, string name, params SqlServerDescriptor[] objects)
            : this()
        {
            Schema = schema ?? "dbo";
            Name = name;

            foreach (var item in objects)
            {
                if (item is ColumnDescriptor column)
                    Columns.Add(column);

                else if (item is PrimaryKeyDescriptor primary)
                    Keys.Add(primary);

                else if (item is ForeignKeyDescriptor foreignKey)
                    ForeignKeys.Add(foreignKey);

                else if (item is IndexDescriptor index)
                    Indexes.Add(index);

                else
                    throw new NotSupportedException(item.GetType().Name);

            }

        }
             
        public TableDescriptor(string name, params SqlServerDescriptor[] objects)
            : this(null, name, objects)
        {

        }

        #endregion Ctors



        public TableDescriptor AddColumns(params ColumnDescriptor[] columns)
        {
            Columns.AddRange(columns);
            return this;
        }

        public TableDescriptor AddColumn(string name, SqlTypeDescriptor type, bool allowNul = false)
        {
            Columns.Add(new ColumnDescriptor(name, type, allowNul));
            return this;
        }

        public TableDescriptor AddIndexes(params IndexDescriptor[] indexes)
        {
            Indexes.AddRange(indexes);
            return this;
        }

        public TableDescriptor AddForeignKey(string? constraintName, string remoteSchema, string remoteTable, Action<ForeignKeyDescriptor> action)
        {


            if (string.IsNullOrEmpty(constraintName) || string.IsNullOrWhiteSpace(remoteSchema))
            {
                constraintName = $"{remoteSchema}_{remoteTable}_has_{this.Schema}_{this.Name}";
                if (constraintName.Length > 128)
                    constraintName = $"{remoteTable}_has_{this.Name}";
            }

            var foreignKey = new ForeignKeyDescriptor()
            {
                Name = constraintName,
            };

            foreignKey.RemoteColumns.Schema = remoteSchema ?? Schema;
            foreignKey.RemoteColumns.TableName = remoteTable;

            AddForeignKey(foreignKey);

            action(foreignKey);

            return this;

        }

        public TableDescriptor AddForeignKey(ForeignKeyDescriptor foreignKey)
        {
            ForeignKeys.Add(foreignKey);
            return this;
        }

        public TableDescriptor SetFilegroupOnPrimaryKeys(string fileGroup)
        {

            PrimaryKeyDescriptor k;
            if (Keys.Count == 0)
                k = new PrimaryKeyDescriptor();
            else
                k = Keys[0];

            k.PartitionSchemeName = fileGroup;

            return this;
        }


        #region Add primary key
        
        public TableDescriptor AddPrimaryKeys(string key1, SortIndex sort1)
        {

            AddPrimaryKeys(
                new IndexedColumnReferenceDescriptor() { Name = key1, Sort = sort1 }
                );

            return this;

        }

        public TableDescriptor AddPrimaryKeys(string key1, SortIndex sort1, string key2, SortIndex sort2)
        {

            AddPrimaryKeys(
                new IndexedColumnReferenceDescriptor() { Name = key1, Sort = sort1 },
                new IndexedColumnReferenceDescriptor() { Name = key2, Sort = sort2 }
                );

            return this;

        }

        public TableDescriptor AddPrimaryKeys(string key1, SortIndex sort1, string key2, SortIndex sort2, string key3, SortIndex sort3)
        {

            AddPrimaryKeys(
                new IndexedColumnReferenceDescriptor() { Name = key1, Sort = sort1 },
                new IndexedColumnReferenceDescriptor() { Name = key2, Sort = sort2 },
                new IndexedColumnReferenceDescriptor() { Name = key3, Sort = sort3 }
                );

            return this;

        }

        public TableDescriptor AddPrimaryKeys(string key1, SortIndex sort1, string key2, SortIndex sort2, string key3, SortIndex sort3, string key4, SortIndex sort4)
        {

            AddPrimaryKeys(
                new IndexedColumnReferenceDescriptor() { Name = key1, Sort = sort1 },
                new IndexedColumnReferenceDescriptor() { Name = key2, Sort = sort2 },
                new IndexedColumnReferenceDescriptor() { Name = key3, Sort = sort3 },
                new IndexedColumnReferenceDescriptor() { Name = key4, Sort = sort4 }
                );

            return this;

        }

        public TableDescriptor AddPrimaryKeys(params string[] keys)
        {

            List<IndexedColumnReferenceDescriptor> u = new List<IndexedColumnReferenceDescriptor>(keys.Length);
            foreach (var key in keys)
                if (!string.IsNullOrEmpty(key))
                    u.Add(new IndexedColumnReferenceDescriptor() { Name = key });

            AddPrimaryKeys(u.ToArray());

            return this;

        }

        public TableDescriptor AddPrimaryKeys(params IndexedColumnReferenceDescriptor[] keys)
        {

            PrimaryKeyDescriptor k;
            if (Keys.Count == 0)
                k = new PrimaryKeyDescriptor() { Name = $"Pk_{this.Schema}_{this.Name}" };
            else
                k = Keys[0];

            k.AddRange(keys);
            Keys.Add(k);
            return this;
        }

        #endregion Add primary key


        #region Properties

        public ColumnListDescriptor Columns { get; set; }

        public PrimaryKeyDescriptor? Key { get => Keys.Count > 0 ? Keys[0] : null; }

        public PrimaryKeyListDescriptor Keys { get; set; }

        public ForeignKeyListDescriptor ForeignKeys { get; set; }

        public IndexListDescriptor Indexes { get; set; }

        public string Schema { get; set; }

        public string PartitionSchemeName { get; set; } = "PRIMARY";

        #endregion Properties


        public IndexDescriptor? GetIndex(string name)
        {
            return this.Indexes.FirstOrDefault(c => c.Name == name);
        }

        public ColumnDescriptor? GetColumn(string name)
        {
            return this.Columns.FirstOrDefault(c => c.Name == name);
        }

        public ForeignKeyDescriptor? GetForeignKey(string name)
        {
            return this.ForeignKeys.FirstOrDefault(c => c.Name == name);
        }




        public static TableDescriptor Load(Reader<ColumnStructures> reader)
        {

            var schemaName = reader.GetString(ColumnStructures.Schema);
            var tableName = reader.GetString(ColumnStructures.TableName);

            var table = new TableDescriptor() { Schema = schemaName, Name = tableName };

            return table;

        }

        public void LoadIndexes(Reader<IndexColumns> reader)
        {

            var is_primary = reader.GetBoolean(IndexColumns.is_primary);

            if (is_primary)
            {
                var primary = PrimaryKeyDescriptor.Create(reader);
                if (primary != null)
                    this.Keys.Add(primary);
            }

            else
            {
                var index = IndexDescriptor.Create(reader);
                if (index != null)
                    this.Indexes.Add(index);
            }
        }




        public TableDescriptor Clone(string? newName = null, bool copyKey = false, bool copyIndex = false)
        {

            var table = new TableDescriptor()
            {
                Name = newName ?? this.Name,
                Schema = this.Schema,
                PartitionSchemeName = this.PartitionSchemeName,
            };

            foreach (var column in this.Columns)
                table.Columns.Add(column.Clone());

            if (copyKey)
                foreach (PrimaryKeyDescriptor key in this.Keys)
                    table.Keys.Add(key.Clone() as PrimaryKeyDescriptor);

            if (copyIndex)
                foreach (IndexDescriptor index in this.Indexes)
                    table.Indexes.Add(index.Clone());

            return table;

        }

        public static string GetRandomTableName(bool isTemp = false)
        {
            
            if(isTemp)
                return "#tmp_" + Guid.NewGuid().ToString("N");

            return "table_" + Guid.NewGuid().ToString("N");

        }


        #region scripts

        public string GetScriptToCreate()
        {
            var sb = new System.Text.StringBuilder();
            GetScriptToCreate(sb);
            return sb.ToString();
        }

        public string GetScriptToDrop()
        {
            var sb = new System.Text.StringBuilder();
            GetScriptToDrop(sb);
            return sb.ToString();
        }

        public void GetScriptToCreate(StringBuilder sb)
        {

            var c = new CreateTables
                (
                    new Writer(sb),
                    new Structures.Dacpacs.ScriptContext(new Structures.Dacpacs.Variables())
                );

            c.Parse(this);

        }

        public void GetScriptToDrop(StringBuilder sb)
        {

            var writer = new Writer(sb);

            writer.AppendEndLine(TextQueries.TestTableExists(Schema, Name));
            using (writer.Indent())
                writer.AppendEndLine($"DROP TABLE {Writer.ToLabel(this.Schema, this.Name)}");

        }

        #endregion scripts


    }


}