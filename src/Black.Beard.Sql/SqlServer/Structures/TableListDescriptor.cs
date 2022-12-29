using Bb.Dacpacs;

namespace Bb.SqlServer.Structures
{

    public class TableListDescriptor : List<TableDescriptor>
    {

        public TableListDescriptor()
        {

        }

        public TableListDescriptor(int capacity) : base(capacity)
        {

        }

    }



}