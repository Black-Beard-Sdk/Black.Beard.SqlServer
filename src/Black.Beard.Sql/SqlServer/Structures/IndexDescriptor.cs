using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures.Ddl;
using Bb.SqlServerStructures;
using System.Diagnostics;
using System.Text;

namespace Bb.SqlServer.Structures
{

    [DebuggerDisplay("{Name}")]
    public class IndexDescriptor : ListModelDescriptor<IndexedColumnReferenceDescriptor>
    {

        public IndexDescriptor()
        {
            Properties = new IndexProperties();
        }

        public IndexDescriptor(int capacity) : base(capacity)
        {
            Properties = new IndexProperties();
        }

        public IndexDescriptor AddColumns(params IndexedColumnReferenceDescriptor[] columns)
        {
            AddRange(columns);
            return this;
        }

        public IndexDescriptor SetProperties(Action<IndexProperties> action)
        {
            action(Properties);
            return this;
        }

        public virtual bool IsPrimaryKey  => false;

        public bool IsDifferent(IndexDescriptor target)
        {

            if (this.Name != target.Name)
                return true;

            if (this.Clustered != target.Clustered)
                return true;

            if (this.Unique != target.Unique)
                return true;

            if (this.PartitionSchemeName != target.PartitionSchemeName)
                return true;

            if (this.Properties.IsDifferent(target.Properties)) 
                return true;

            return false;

        }

        public virtual IndexDescriptor Clone()
        {

            var result = new IndexDescriptor()
            {
                Name = this.Name,
                Clustered = this.Clustered,
                Unique = this.Unique,
                PartitionSchemeName = this.PartitionSchemeName,
            };

            result.Properties.CloneFrom(this.Properties);

            foreach (var item in this)
                this.Add(item.Clone() as IndexedColumnReferenceDescriptor);

            return result;

        }

        public string Name { get; set; }

        public IndexProperties Properties { get; set; }

        public bool Clustered { get; set; } = true;

        public bool Unique { get; set; } = true;

        public string PartitionSchemeName { get; set; } = "PRIMARY";

        public IndexDescriptor CopyProperty(IndexProperties properties)
        {
            this.Properties.CloneFrom(properties); 
            return this;
        }

        public IndexDescriptor CopyColumns(ListModelDescriptor<IndexedColumnReferenceDescriptor> source)
        {
            foreach (var item in source)
                this.Add(item.Clone());
            return this;
        }

        public static IndexDescriptor? Create(Reader<IndexColumns> reader)
        {

            var type = reader.GetString(IndexColumns.index_type);
            switch (type)
            {

                case "HEAP":
                    break;

                case "Clustered index":
                case "Nonclustered index":
                    var key = new IndexDescriptor()
                    {
                        Name = reader.GetString((int)IndexColumns.name),
                        Clustered = type == "Clustered index",
                        Unique = reader.GetBoolean(IndexColumns.is_unique),
                    };
                    Map(reader, key);
                    return key;

                default:
                    break;
            }

            return null;

        }

        protected static void Map(Reader<IndexColumns> reader, IndexDescriptor key)
        {
            key.Properties.PadIndex = reader.GetBoolean(IndexColumns.is_padded);
            key.Properties.StatisticsNorecompute = reader.GetBoolean(IndexColumns.no_recompute);
            key.Properties.AllowRowLocks = reader.GetBoolean(IndexColumns.allow_row_locks);
            key.Properties.AllowPageLocks = reader.GetBoolean(IndexColumns.allow_page_locks);
            key.Properties.OptimizeForSequentialKey = reader.GetBoolean(IndexColumns.optimize_sequential_key);

            var c1 = reader.GetString(IndexColumns.columns);
            var c2 = reader.GetString(IndexColumns.column_descendings);
            var columns = c1.Split(',');
            var descendings = c2.Split(',');

            for (int i = 0; i < columns.Length; i++)
            {

                key.Add(new IndexedColumnReferenceDescriptor()
                {
                    Name = columns[i].Trim(),
                    Sort = descendings[i].Trim() == "1" ? SortIndex.Descending : SortIndex.Ascending,
                });
            }
        }

        #region scripts

        public string GetScriptToCreate(TableDescriptor table)
        {
            var sb = new System.Text.StringBuilder();
            GetScriptToCreate(table, sb);
            return sb.ToString();
        }

        public string GetScriptToDrop()
        {
            var sb = new System.Text.StringBuilder();
            GetScriptToDrop(sb);
            return sb.ToString();
        }

        public void GetScriptToCreate(TableDescriptor table, StringBuilder sb)
        {

            var c = new CreateIndexes
                (
                    new Writer(sb),
                    new Structures.Dacpacs.ScriptContext(new Structures.Dacpacs.Variables())
                );

            c.Parse(table, this);

        }

        public void GetScriptToDrop(StringBuilder sb)
        {

            var writer = new Writer(sb);

            writer.AppendEndLine(TextQueries.TestIndexExists(Name));
            using (writer.Indent())
                writer.AppendEndLine($"DROP INDEX {Writer.ToLabel(this.Name)}");

        }

        #endregion scripts
    }


    public enum IndexType
    {
        heap = 0,
        Clustered_index = 1,
        Nonclustered_index = 2,

        XML_index = 3,
        Spatial_index = 4,
        
        Clustered_columnstore_index = 5,
        Nonclustered_columnstore_index = 6,
        Nonclustered_hash_index = 7,
    }


}