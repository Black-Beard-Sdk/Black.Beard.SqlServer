using System;
using System.Diagnostics;

namespace Bb.SqlServer.Structures
{

    [DebuggerDisplay("{ColumnType}")]
    public partial class SqlTypeDescriptor
    {

        static SqlTypeDescriptor()
        {

            Types = new Dictionary<string, SqlDataTypeDescriptor>();

            AddType(_BIGINT, typeof(long));
            AddType(_NUMERIC, typeof(decimal));
            AddType(_BIT, typeof(bool));
            AddType(_SMALLINT, typeof(short));
            AddType(_DECIMAL, typeof(decimal));
            AddType(_SMALLMONEY, typeof(decimal));
            AddType(_INT, typeof(int));
            AddType(_TINYINT, typeof(byte));
            AddType(_MONEY, typeof(decimal));

            AddType(_FLOAT, typeof(double));
            AddType(_REAL, typeof(float));

            AddType(_DATE, typeof(DateTime));
            AddType(_DATETIMEOFFSET, typeof(DateTimeOffset));
            AddType(_DATETIME2, typeof(DateTime));
            AddType(_SMALLDATETIME, typeof(DateTime));
            AddType(_DATETIME, typeof(DateTime));
            AddType(_TIME, typeof(TimeSpan));
            AddType(_TIMESTAMP, typeof(byte[]));


            AddType(_CHAR, typeof(string));
            AddType(_FILESTREAM, typeof(byte[]));
            AddType(_VARCHAR, typeof(string));
            AddType(_TEXT, typeof(string));
            AddType(_NCHAR, typeof(string));
            AddType(_NVARCHAR, typeof(string));
            AddType(_NTEXT, typeof(string));
            AddType(_BINARY, typeof(byte[]));
            AddType(_VARBINARY, typeof(byte[]));
            AddType(_IMAGE, typeof(byte[]));
            AddType(_ROWVERSION, typeof(byte[]));
            AddType(_UNIQUEIDENTIFIER, typeof(Guid));
            AddType(_SQL_VARIANT, typeof(object));
            AddType(_XML, typeof(System.Xml.XmlElement));
            AddType(_GEOMETRY, typeof(object));
            AddType(_GEOGRAPHY, typeof(object));


            //AddType(CURSOR, typeof());
            //AddType(HIERARCHID, typeof());
        }


        public SqlTypeDescriptor()
        {


        }

        public SqlTypeDescriptor(string sqlLabel)
        {
            ColumnType = sqlLabel;

        }

        public SqlTypeDescriptor(int arg1, string sqlLabel)
        {
            ColumnType = sqlLabel;
            Argument1 = arg1;
        }

        public SqlTypeDescriptor(int arg1, int arg2, string sqlLabel)
        {
            ColumnType = sqlLabel;
            Argument1 = arg1;
            Argument2= arg2;
        }


        public SqlTypeDescriptor(SqlDataTypeDescriptor type)
        {
            ColumnType = type.SqlLabel;

        }

        public string TypeSchemaName { get; set; }

        public string XmlSchemaCollection { get; set; }

        public static int Max { get => -1; }

        public bool IsIdentity { get; set; }

        public int? Argument1 { get; set; }

        public int? Argument2 { get; set; }

        public SqlDataTypeDescriptor SqlDataType
        {
            get
            {
                return Types[ColumnType];
            }
            set
            {
                ColumnType = value.SqlLabel;
            }
        }

        public string ColumnType { get; set; }


        private static void AddType(string sqlType, Type type)
        {
            Types.Add(sqlType, new SqlDataTypeDescriptor(sqlType, type));
        }


        private static Dictionary<string, SqlDataTypeDescriptor> Types { get; }


        //AddType("CURSOR", typeof());
        //AddType("HIERARCHID", typeof());

    }

}