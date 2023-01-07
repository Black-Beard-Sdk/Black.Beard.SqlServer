using System.Drawing;
using System.Text;
using Bb.SqlServerStructures;

namespace Bb.SqlServer.Structures
{

    public partial class SqlTypeDescriptor
    {


        public static SqlTypeDescriptor Create(string sqlDatatype, bool is_identity, int max_length, int precision, int scale, int seed, int increment)
        {

            if (Types.TryGetValue(sqlDatatype, out var type))
            {

                switch (sqlDatatype)
                {

                    case _INT:
                        if (is_identity)
                            return IdentityBigInt(seed, increment);
                        return Int();

                    case _BIGINT:
                        if (is_identity)
                            return IdentityInt(seed, increment); 
                        return BigInt();

                    case _NUMERIC:
                        return Numeric();

                    case _BIT:
                        return Bit();

                    case _SMALLINT:
                        return SmallInt();

                    case _DECIMAL:
                        return Decimal(scale, precision);

                    case _SMALLMONEY:
                        return SmallMoney();


                    case _TINYINT:
                        return TinyInt();

                    case _MONEY:
                        return Money();

                    case _FLOAT:
                        return Float();

                    case _REAL:
                        return Real();  

                    case _DATE:
                        return Date();

                    case _DATETIMEOFFSET:
                        return DateTimeOffset();

                    case _DATETIME2:
                        return DateTime2();

                    case _SMALLDATETIME:
                        return SmallDateTime();

                    case _DATETIME:
                        return DateTime();

                    case _TIME:
                        return Time();

                    case _TIMESTAMP:
                        return Timestamp();

                    case _CHAR:
                        return Char(max_length);

                    case _VARCHAR:
                        return Varchar(max_length);

                    case _TEXT:
                        return Text(max_length);

                    case _NCHAR:
                        return Nchar(max_length);

                    case _NVARCHAR:
                        return NVarchar(max_length);

                    case _NTEXT:
                        return NText(max_length);

                    case _BINARY:
                        return Binary(max_length);

                    case _VARBINARY:
                        return VarBinary(max_length);

                    case _IMAGE:
                        return Image(max_length);

                    case _ROWVERSION:
                        return RowVersion();

                    case _UNIQUEIDENTIFIER:
                        return UniqueIDentifier();

                    case _SQL_VARIANT:
                        return SqlVariant();

                    case _XML:
                        return Xml();

                    case _GEOMETRY:
                        return Geometry();

                    case _GEOGRAPHY:
                        return Geography();

                    default:
                        break;

                }

            }

            throw new NotImplementedException(sqlDatatype);

        }


        public static SqlTypeDescriptor Create(string sqlDatatype, bool is_identity, int? arg1, int? arg2)
        {

            if (Types.TryGetValue(sqlDatatype, out var type))
            {

                switch (sqlDatatype)
                {

                    case _INT:
                        if (is_identity)
                            return IdentityBigInt(arg1.Value, arg2.Value);
                        return Int();

                    case _BIGINT:
                        if (is_identity)
                            return IdentityInt(arg1.Value, arg2.Value);
                        return BigInt();

                    case _NUMERIC:
                        return Numeric();

                    case _BIT:
                        return Bit();

                    case _SMALLINT:
                        return SmallInt();

                    case _DECIMAL:
                        return Decimal(arg1.Value, arg2.Value);

                    case _SMALLMONEY:
                        return SmallMoney();


                    case _TINYINT:
                        return TinyInt();

                    case _MONEY:
                        return Money();

                    case _FLOAT:
                        return Float();

                    case _REAL:
                        return Real();

                    case _DATE:
                        return Date();

                    case _DATETIMEOFFSET:
                        return DateTimeOffset();

                    case _DATETIME2:
                        return DateTime2();

                    case _SMALLDATETIME:
                        return SmallDateTime();

                    case _DATETIME:
                        return DateTime();

                    case _TIME:
                        return Time();

                    case _TIMESTAMP:
                        return Timestamp();

                    case _CHAR:
                        return Char(arg1.Value);

                    case _VARCHAR:
                        return Varchar(arg1.Value);

                    case _TEXT:
                        return Text(arg1.Value);

                    case _NCHAR:
                        return Nchar(arg1.Value);

                    case _NVARCHAR:
                        return NVarchar(arg1.Value);

                    case _NTEXT:
                        return NText(arg1.Value);

                    case _BINARY:
                        return Binary(arg1.Value);

                    case _VARBINARY:
                        return VarBinary(arg1.Value);

                    case _IMAGE:
                        return Image(arg1.Value);

                    case _ROWVERSION:
                        return RowVersion();

                    case _UNIQUEIDENTIFIER:
                        return UniqueIDentifier();

                    case _SQL_VARIANT:
                        return SqlVariant();

                    case _XML:
                        return Xml();

                    case _GEOMETRY:
                        return Geometry();

                    case _GEOGRAPHY:
                        return Geography();

                    default:
                        break;

                }

            }

            throw new NotImplementedException(sqlDatatype);

        }


        public SqlTypeDescriptor Clone()
        {

            return Create(this.SqlDataType.SqlLabel, this.IsIdentity, this.Argument1, this.Argument2);

        }


        public static SqlTypeDescriptor BigInt()
        {
            return new SqlTypeDescriptor(_BIGINT);
        }
        
        public static SqlTypeDescriptor Numeric()
        {
            return new SqlTypeDescriptor(_BIT);
        }

        public static SqlTypeDescriptor SmallInt()
        {
            return new SqlTypeDescriptor(_SMALLINT);
        }

        public static SqlTypeDescriptor Bit()
        {
            return new SqlTypeDescriptor(_BIT);
        }

        public static SqlTypeDescriptor Decimal(int scale, int precision)
        {
            return new SqlTypeDescriptor(scale, precision, _DECIMAL);
        }

        public static SqlTypeDescriptor SmallMoney()
        {
            return new SqlTypeDescriptor(_SMALLMONEY);
        }

        public static SqlTypeDescriptor Int()
        {
            return new SqlTypeDescriptor(_INT);
        }

        public static SqlTypeDescriptor TinyInt()
        {
            return new SqlTypeDescriptor(_TINYINT);
        }

        public static SqlTypeDescriptor Money()
        {
            return new SqlTypeDescriptor(_MONEY);
        }

        public static SqlTypeDescriptor Float()
        {
            return new SqlTypeDescriptor(_FLOAT);
        }

        public static SqlTypeDescriptor Real()
        {
            return new SqlTypeDescriptor(_REAL);
        }

        public static SqlTypeDescriptor Date()
        {
            return new SqlTypeDescriptor(_DATE);
        }

        public static SqlTypeDescriptor DateTimeOffset()
        {
            return new SqlTypeDescriptor(_DATETIMEOFFSET);
        }

        public static SqlTypeDescriptor DateTime2()
        {
            return new SqlTypeDescriptor(_DATETIME2);
        }

        public static SqlTypeDescriptor SmallDateTime()
        {
            return new SqlTypeDescriptor(_SMALLDATETIME);
        }

        public static SqlTypeDescriptor DateTime()
        {
            return new SqlTypeDescriptor(_DATETIME);
        }

        public static SqlTypeDescriptor Time()
        {
            return new SqlTypeDescriptor(_TIME);
        }

        public static SqlTypeDescriptor Timestamp()
        {
            return new SqlTypeDescriptor(_TIMESTAMP);
        }

        public static SqlTypeDescriptor Char(int size)
        {
            return new SqlTypeDescriptor(size, _CHAR);
        }

        public static SqlTypeDescriptor Filestream()
        {
            return new SqlTypeDescriptor(_FILESTREAM);
        }

        public static SqlTypeDescriptor Varchar(int size)
        {
            return new SqlTypeDescriptor(size, _VARCHAR);
        }

        public static SqlTypeDescriptor Text(int size)
        {
            return new SqlTypeDescriptor(size, _TEXT);
        }

        public static SqlTypeDescriptor Nchar(int size)
        {
            return new SqlTypeDescriptor(size, _NCHAR);
        }

        public static SqlTypeDescriptor NVarchar(int size)
        {
            return new SqlTypeDescriptor(size, _NVARCHAR);
        }

        public static SqlTypeDescriptor NText(int size)
        {
            return new SqlTypeDescriptor(size, _NTEXT);
        }

        public static SqlTypeDescriptor Binary(int size)
        {
            return new SqlTypeDescriptor(size, _BINARY);
        }

        public static SqlTypeDescriptor VarBinary(int size)
        {
            return new SqlTypeDescriptor(size, _VARBINARY);
        }

        public static SqlTypeDescriptor Image(int size)
        {
            return new SqlTypeDescriptor(size, _IMAGE);
        }

        public static SqlTypeDescriptor RowVersion()
        {
            return new SqlTypeDescriptor(_ROWVERSION);
        }

        public static SqlTypeDescriptor UniqueIDentifier()
        {
            return new SqlTypeDescriptor(_UNIQUEIDENTIFIER);
        }

        public static SqlTypeDescriptor SqlVariant()
        {
            return new SqlTypeDescriptor(_SQL_VARIANT);
        }

        public static SqlTypeDescriptor Xml()
        {
            return new SqlTypeDescriptor(_XML);
        }

        public static SqlTypeDescriptor Geometry()
        {
            return new SqlTypeDescriptor(_GEOMETRY);
        }

        public static SqlTypeDescriptor Geography()
        {
            return new SqlTypeDescriptor(_GEOGRAPHY);
        }


        public static SqlTypeDescriptor IdentityInt(int start = 1, int step = 1)
        {
            return new SqlTypeDescriptor(start, step, _INT) { IsIdentity = true };
        }

        public static SqlTypeDescriptor IdentityBigInt(int start = 1, int step = 1)
        {
            return new SqlTypeDescriptor(start, step, _BIGINT) { IsIdentity = true };
        }



        public const string _BIGINT = "BIGINT";
        public const string _NUMERIC = "NUMERIC";
        public const string _BIT = "BIT";
        public const string _SMALLINT = "SMALLINT";
        public const string _DECIMAL = "DECIMAL";
        public const string _SMALLMONEY = "SMALLMONEY";
        public const string _INT = "INT";
        public const string _TINYINT = "TINYINT";
        public const string _MONEY = "MONEY";
        public const string _FLOAT = "FLOAT";
        public const string _REAL = "REAL";
        public const string _DATE = "DATE";
        public const string _DATETIMEOFFSET = "DATETIMEOFFSET";
        public const string _DATETIME2 = "DATETIME2";
        public const string _SMALLDATETIME = "SMALLDATETIME";
        public const string _DATETIME = "DATETIME";
        public const string _TIME = "TIME";
        public const string _TIMESTAMP = "TIMESTAMP";
        public const string _CHAR = "CHAR";
        public const string _FILESTREAM = "FILESTREAM";
        public const string _VARCHAR = "VARCHAR";
        public const string _TEXT = "TEXT";
        public const string _NCHAR = "NCHAR";
        public const string _NVARCHAR = "NVARCHAR";
        public const string _NTEXT = "NTEXT";
        public const string _BINARY = "BINARY";
        public const string _VARBINARY = "VARBINARY";
        public const string _IMAGE = "IMAGE";
        public const string _ROWVERSION = "ROWVERSION";
        public const string _UNIQUEIDENTIFIER = "UNIQUEIDENTIFIER";
        public const string _SQL_VARIANT = "SQL_VARIANT";
        public const string _XML = "XML";
        public const string _GEOMETRY = "GEOMETRY";
        public const string _GEOGRAPHY = "GEOGRAPHY";

    }


}