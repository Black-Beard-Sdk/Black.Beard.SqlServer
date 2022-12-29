namespace Bb.SqlServer.Structures
{
    public class IndexedColumnReferenceDescriptor : ColumnReferenceDescriptor
    {

        public IndexedColumnReferenceDescriptor()
        {

        }

        public SortIndex Sort { get; set; }

    }


}