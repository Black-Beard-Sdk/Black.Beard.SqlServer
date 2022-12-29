using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Bb.SqlServerStructures
{
    public class Reader<T>
        where T : Enum
    {

        public Reader(SqlDataReader source)
        {
            this._source = source;
        }


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public string? GetString(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetString(i);
        }


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool GetBoolean(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return false;

            var value = _source.GetValue(i);
            if (value is bool v0)
                return v0;

            if (value is int v1)
                return v1 != 0;

            if (value is short v2)
                return v2 != 0;

            if (value is long v3)
                return v3 != 0;

            if (value is byte v4)
                return v4 != 0;

            return _source.GetBoolean(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public byte GetByte(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;
            return _source.GetByte(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public long GetBytes(T columnIndex, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;
            return _source.GetBytes(i, dataIndex, buffer, bufferIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public char GetChar(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return '\0';
            return _source.GetChar(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public long GetChars(T columnIndex, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;
            return _source.GetChars(i, dataIndex, buffer, bufferIndex, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public string GetDataTypeName(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return string.Empty;
            return _source.GetDataTypeName(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public DateTime? GetDateTime(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetDateTime(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public DateTimeOffset? GetDateTimeOffset(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetDateTimeOffset(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public decimal GetDecimal(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;
            return _source.GetDecimal(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public double GetDouble(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;
            return _source.GetDouble(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Type GetFieldType(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            //if (_source.IsDBNull(i))
            //    return null;
            return _source.GetFieldType(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public U? GetFieldValue<U>(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return default(U);
            try
            {
                return _source.GetFieldValue<U>(i);
            }
            catch (Exception)
            {

            }

            return default(U);

        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public float GetFloat(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;
            return _source.GetFloat(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Guid GetGuid(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return Guid.Empty;
            return _source.GetGuid(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public short GetInt16(T columnIndex)
        {

            var i = (int)(object)columnIndex;

            if (_source.IsDBNull(i))
                return 0;

            var value = _source.GetValue(i);

            if (value is short v0)
                return v0;

            if (value is long v1)
                return Convert.ToInt16(v1);

            if (value is int v2)
                return Convert.ToInt16(v2);

            if (value is byte v3)
                return v3;

            return (short)value;

        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetInt32(T columnIndex)
        {

            var i = (int)(object)columnIndex;

            if (_source.IsDBNull(i))
                return 0;

            var value = _source.GetValue(i);

            if (value is int v0)
                return v0;

            if (value is long v1)
                return Convert.ToInt32(v1);

            if (value is short v2)
                return (int)v2;

            if (value is byte v3)
                return v3;

            return (Int32)value;

        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public long GetInt64(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return 0;

            var value = _source.GetValue(i);

            if (value is long v0)
                return v0;

            if (value is int v1)
                return Convert.ToInt64(v1);

            if (value is short v2)
                return Convert.ToInt64(v2);

            if (value is byte v3)
                return v3;

            return (Int64)value;

        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public string GetName(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return string.Empty;
            return _source.GetName(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public T GetOrdinal(string name)
        {
            return (T)(object)_source.GetOrdinal(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlBinary GetSqlBinary(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlBinary();
            return _source.GetSqlBinary(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlBoolean GetSqlBoolean(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlBoolean();
            return _source.GetSqlBoolean(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlByte GetSqlByte(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlByte();
            return _source.GetSqlByte(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlBytes GetSqlBytes(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlBytes();
            return _source.GetSqlBytes(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlChars GetSqlChars(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlChars();
            return _source.GetSqlChars(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlDateTime GetSqlDateTime(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlDateTime();
            return _source.GetSqlDateTime(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlDecimal GetSqlDecimal(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlDecimal();
            return _source.GetSqlDecimal(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlDouble GetSqlDouble(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlDouble();
            return _source.GetSqlDouble(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlGuid GetSqlGuid(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlGuid();
            return _source.GetSqlGuid(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlInt16 GetSqlInt16(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlInt16();
            return _source.GetSqlInt16(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlInt32 GetSqlInt32(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlInt32();
            return _source.GetSqlInt32(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlInt64 GetSqlInt64(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlInt64();
            return _source.GetSqlInt64(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlMoney GetSqlMoney(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlMoney();
            return _source.GetSqlMoney(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlSingle GetSqlSingle(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlSingle();
            return _source.GetSqlSingle(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlString GetSqlString(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlString();
            return _source.GetSqlString(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public object? GetSqlValue(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetSqlValue(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public SqlXml GetSqlXml(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new SqlXml();
            return _source.GetSqlXml(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Stream? GetStream(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetStream(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public TextReader? GetTextReader(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetTextReader(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public TimeSpan GetTimeSpan(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return new TimeSpan();
            return _source.GetTimeSpan(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public object? GetValue(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetValue(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public XmlReader? GetXmlReader(T columnIndex)
        {
            var i = (int)(object)columnIndex;
            if (_source.IsDBNull(i))
                return null;
            return _source.GetXmlReader(i);
        }


        private SqlDataReader _source;

    }

}


