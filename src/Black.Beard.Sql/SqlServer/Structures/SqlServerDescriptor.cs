using System.Diagnostics;

namespace Bb.SqlServer.Structures
{

    [DebuggerDisplay("{Name}")]
    public class SqlServerDescriptor
    {

        public string Name { get; set; }

    }

}