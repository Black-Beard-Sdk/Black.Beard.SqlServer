using Bb.SqlServer.Structures.Dacpacs;
using System.Diagnostics;

namespace Bb.SqlServer.Structures
{

    [DebuggerDisplay("{Name}")]
    public class IndexDescriptor : List<IndexedColumnReferenceDescriptor>
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

        public string Name { get; set; }

        public IndexProperties Properties { get; set; }

        public bool Clustered { get; set; } = true;

        public bool Unique { get; set; } = true;

        public string PartitionSchemeName { get; set; } = "PRIMARY";

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