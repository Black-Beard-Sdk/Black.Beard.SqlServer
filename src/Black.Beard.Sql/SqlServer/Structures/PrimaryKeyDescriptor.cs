using Bb.SqlServer.Queries;
using Bb.SqlServerStructures;

namespace Bb.SqlServer.Structures
{
    public class PrimaryKeyDescriptor : IndexDescriptor
    {

        public PrimaryKeyDescriptor()
        {

        }

        public PrimaryKeyDescriptor(int capacity) : base(capacity)
        {

        }


        public override bool IsPrimaryKey => true;

        public bool IsDifferent(PrimaryKeyDescriptor target)
        {

            if (base.IsDifferent(target))
                return true;

            return false;

        }

        public override IndexDescriptor Clone()
        {

            var result = new PrimaryKeyDescriptor()
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

        public static PrimaryKeyDescriptor Create(Reader<IndexColumns> reader)
        {

            var type = reader.GetString(IndexColumns.index_type);

            var key = new PrimaryKeyDescriptor()
            {
                Name = reader.GetString((int)IndexColumns.name),
                Clustered = type == "Clustered index",
                Unique = reader.GetBoolean(IndexColumns.is_unique),
                PartitionSchemeName = reader.GetString(IndexColumns.Filegroup),
            };

            Map(reader, key);

            return key;

        }

    }



}