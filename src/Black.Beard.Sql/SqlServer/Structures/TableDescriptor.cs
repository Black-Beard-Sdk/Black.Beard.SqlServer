namespace Bb.SqlServer.Structures
{

    public class TableDescriptor : SqlServerDescriptor
    {

        public TableDescriptor()
        {
            Columns = new ColumnListDescriptor();
            Keys = new PrimaryKeyListDescriptor();
            Indexes = new IndexListDescriptor();
            ForeignKeys = new ForeignKeyListDescriptor();
        }

        public TableDescriptor(string schema, string name, params SqlServerDescriptor[] objects)
            : this()
        {

            Schema = schema;
            Name = name;

        }

        public TableDescriptor(string name, params SqlServerDescriptor[] objects)
            : this("dbo", name, objects)
        {

        }


        public TableDescriptor AddColumns(params ColumnDescriptor[] columns)
        {
            Columns.AddRange(columns);
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
                constraintName = $"{remoteSchema}.{remoteTable}_has_{this.Schema}.{this.Name}";
                if (constraintName.Length > 128)
                    constraintName = $"{remoteTable}_has_{this.Name}";
            }

            var f = new ForeignKeyDescriptor()
            {
                Name = constraintName,
            };

            f.RemoteColumns.Schema = remoteSchema ?? Schema;
            f.RemoteColumns.TableName = remoteTable;
            ForeignKeys.Add(f);

            action(f);

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

        public ColumnListDescriptor Columns { get; set; }


        public PrimaryKeyListDescriptor Keys { get; set; }


        public ForeignKeyListDescriptor ForeignKeys { get; set; }


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

        public IndexListDescriptor Indexes { get; set; }

        public string Schema { get; set; }

        public string PartitionSchemeName { get; set; } = "PRIMARY";


    }


}