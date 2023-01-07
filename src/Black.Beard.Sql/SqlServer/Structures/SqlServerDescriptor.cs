using System.Diagnostics;

namespace Bb.SqlServer.Structures
{


    [DebuggerDisplay("{Name}")]
    public class SqlServerDescriptor
    {

        public SqlServerDescriptor()
        {

        }

        public SqlServerDescriptor(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public long Id { get; set; }

    }

}