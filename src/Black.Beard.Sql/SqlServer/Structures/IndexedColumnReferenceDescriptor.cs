namespace Bb.SqlServer.Structures
{




    public class IndexedColumnReferenceDescriptor : ColumnReferenceDescriptor
    {

        public IndexedColumnReferenceDescriptor()
        {

        }

        public SortIndex Sort { get; set; }

        public override ColumnReferenceDescriptor Clone()
        {
            return new IndexedColumnReferenceDescriptor() { Name = this.Name, Sort = this.Sort };
        }

    }


}