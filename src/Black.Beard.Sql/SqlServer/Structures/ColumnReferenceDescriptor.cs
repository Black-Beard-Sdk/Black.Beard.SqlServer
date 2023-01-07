namespace Bb.SqlServer.Structures
{


    public class ColumnReferenceDescriptor : SqlServerDescriptor
    {

        public ColumnReferenceDescriptor()
        {

        }

        public bool IsDifferent(ColumnReferenceDescriptor target)
        {
            return this.Name != target.Name;
        }

        public virtual ColumnReferenceDescriptor Clone()
        {

            return new ColumnReferenceDescriptor() { Name = this.Name };

        }

    }


}