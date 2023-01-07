namespace Bb.SqlServer.Structures
{
    public class RemoteColumnReferenceListDescriptor : ColumnReferenceListDescriptor
    {


        public RemoteColumnReferenceListDescriptor()
        {

        }

        public RemoteColumnReferenceListDescriptor(int capacity) : base(capacity)
        {

        }

        public string Schema { get; set; }

        public string TableName { get; set; }

        public void For(Action<ColumnReferenceDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }

}