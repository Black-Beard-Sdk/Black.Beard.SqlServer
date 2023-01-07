namespace Bb.SqlServer.Structures
{



    public class IndexListDescriptor : ListModelDescriptor<IndexDescriptor>
    {

        public IndexListDescriptor() : base()
        {

        }


        public IndexListDescriptor(int capacity) : base(capacity)
        {

        }

        public void For(Action<IndexDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }



}