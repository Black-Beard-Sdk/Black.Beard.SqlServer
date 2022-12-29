using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Diagnostics;

namespace Bb.SqlServer.Structures
{


    [DebuggerDisplay("{Name} {Type}")]
    public class ColumnDescriptor : SqlServerDescriptor
    {

        static ColumnDescriptor()
        {

        }
        public ColumnDescriptor()
        {

        }


        public SqlTypeDescriptor Type { get; set; }

        public bool AllowNull { get; set; }

        public string Caption { get; set; } = string.Empty;

        public object DefaultValue { get; set; }

    }

}