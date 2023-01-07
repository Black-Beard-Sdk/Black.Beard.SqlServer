namespace Bb.SqlServer.Structures
{


    public class ColumnReferenceListDescriptor : ListModelDescriptor<ColumnReferenceDescriptor>
    {

        public ColumnReferenceListDescriptor()
        {

        }

        public ColumnReferenceListDescriptor(int capacity) : base(capacity)
        {

        }


        public void For(Action<ColumnReferenceDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }

}