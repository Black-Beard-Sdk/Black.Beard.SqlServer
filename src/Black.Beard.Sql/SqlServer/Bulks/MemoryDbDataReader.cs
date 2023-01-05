using System.Collections;
using System.Data.Common;

namespace Bb.SqlServer.Bulks
{
    public class MemoryDbDataReader : DbDataReader
    {


        public MemoryDbDataReader()
        {

        }

        public override object this[int ordinal]
        {
            get
            {
                return null;
            }
        }

        public override object this[string name]
        {
            get
            {
                return null;
            }
        }

        public override int Depth
        {
            get
            {
                return 0;
            }
        }

        public override int FieldCount
        {
            get
            {
                return 0;
            }
        }

        public override bool HasRows
        {
            get
            {
                return false;
            }
        }

        public override bool IsClosed
        {
            get
            {
                return false;
            }
        }

        public override int RecordsAffected
        {
            get
            {
                return 0;
            }
        }

        public override bool GetBoolean(int ordinal)
        {
            return false;
        }

        public override byte GetByte(int ordinal)
        {
            return (byte)0;
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            return 0;
        }

        public override char GetChar(int ordinal)
        {
            return (char)0;
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            return 0;
        }

        public override string GetDataTypeName(int ordinal)
        {
            return string.Empty;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return DateTime.MinValue;
        }

        public override decimal GetDecimal(int ordinal)
        {
            return 0;
        }

        public override double GetDouble(int ordinal)
        {
            return 0;
        }

        public override IEnumerator GetEnumerator()
        {
            return null;
        }

        public override Type GetFieldType(int ordinal)
        {
            return typeof(string);
        }

        public override float GetFloat(int ordinal)
        {
            return 0;
        }

        public override Guid GetGuid(int ordinal)
        {
            return Guid.Empty;
        }

        public override short GetInt16(int ordinal)
        {
            return 0;
        }

        public override int GetInt32(int ordinal)
        {
            return 0;
        }

        public override long GetInt64(int ordinal)
        {
            return 0;
        }

        public override string GetName(int ordinal)
        {
            return string.Empty;
        }

        public override int GetOrdinal(string name)
        {
            return 0;
        }

        public override string GetString(int ordinal)
        {
            return string.Empty;
        }

        public override object GetValue(int ordinal)
        {
            return null;
        }

        public override int GetValues(object[] values)
        {
            return 0;
        }

        public override bool IsDBNull(int ordinal)
        {
            return false;
        }

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            return false;
        }
    }


}
