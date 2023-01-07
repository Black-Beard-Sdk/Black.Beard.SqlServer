namespace Bb.SqlServer.Structures
{

    public class ColumnListDescriptor : ListModelDescriptor<ColumnDescriptor>
    {

        public ColumnListDescriptor()
        {

        }

        public ColumnListDescriptor(int capacity) : base(capacity)
        {

        }

        public void For(Action<ColumnDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }

}