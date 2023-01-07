using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using Bb.SqlServer.Queries;
using Bb.SqlServerStructures;

namespace Bb.SqlServer.Structures
{


    [DebuggerDisplay("{Name} {SqlType}")]
    public class ColumnDescriptor : SqlServerDescriptor
    {

        public ColumnDescriptor()
        {

        }

        public ColumnDescriptor(string name) : base(name)
        {

        }

        public ColumnDescriptor(string name, SqlTypeDescriptor type, bool allowNull = false) : base(name)
        {
            this.SqlType = type;
            this.AllowNull = allowNull;
        }


        public SqlTypeDescriptor SqlType { get; set; }

        public bool AllowNull { get; set; }

        public string Caption { get; set; } = string.Empty;

        public object DefaultValue { get; set; }

        public static ColumnDescriptor Create(Reader<ColumnStructures> reader)
        {

            var column = new ColumnDescriptor()
            {
                Name = reader.GetString(ColumnStructures.ColumnName),
                Caption = reader.GetString(ColumnStructures.Description),
                AllowNull = reader.GetBoolean(ColumnStructures.is_nullable),
                SqlType = SqlTypeDescriptor.Create
                    (
                        reader.GetString(ColumnStructures.system_data_type),
                        reader.GetBoolean(ColumnStructures.is_identity),
                        reader.GetInt32(ColumnStructures.max_length),
                        reader.GetByte(ColumnStructures.precision),
                        reader.GetByte(ColumnStructures.scale),
                        reader.GetInt32(ColumnStructures.Seed),
                        reader.GetInt32(ColumnStructures.Increment)
                    )
            };

            return column;

        }

        public ColumnDescriptor Clone()
        {

            var column = new ColumnDescriptor()
            {
                Name = Name,
                AllowNull = AllowNull,
                Caption = Caption,
                DefaultValue = DefaultValue,
                SqlType = SqlType.Clone(),
            };

            return column;

        }

    }

}