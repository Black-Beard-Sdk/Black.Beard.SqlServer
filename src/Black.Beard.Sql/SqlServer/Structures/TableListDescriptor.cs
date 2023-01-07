using Bb.Dacpacs;
using Bb.SqlServer.Queries;
using Bb.SqlServerStructures;

namespace Bb.SqlServer.Structures
{

    public class TableListDescriptor : ListModelDescriptor<TableDescriptor>
    {

        public TableListDescriptor()
        {

        }

        public TableListDescriptor(int capacity) : base(capacity)
        {

        }

        public void For(Action<TableDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }



}