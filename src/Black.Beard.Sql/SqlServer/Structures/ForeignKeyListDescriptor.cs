namespace Bb.SqlServer.Structures
{

    public class ForeignKeyListDescriptor : ListModelDescriptor<ForeignKeyDescriptor>
    {

        public ForeignKeyListDescriptor()
        {

        }

        public ForeignKeyListDescriptor(int capacity) : base(capacity)
        {

        }

        public void For(Action<ForeignKeyDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }

}