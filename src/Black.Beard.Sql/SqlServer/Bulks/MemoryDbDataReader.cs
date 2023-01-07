using Bb.SqlServer.Queries;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Bb.SqlServer.Bulks
{



    public class MemoryDbDataReader<T> : DbDataReader
    {


        public MemoryDbDataReader(IEnumerable<T> items, params (string, Func<T, object>)[] columns)
        {

            this._items = items;
            this._enumerator = this._items.GetEnumerator();
            this._columnsByName = new Dictionary<string, Func<T, object>>();
            this._columnsByOrdinal = new Dictionary<int, Func<T, object>>();
            this._ordinals = new Dictionary<string, int>();
            this._names = new Dictionary<int, string>();


            foreach (var item in columns)
            {
                this._columnsByName.Add(item.Item1, item.Item2);
                this._columnsByOrdinal.Add(this._ordinals.Count, item.Item2);
                this._ordinals.Add(item.Item1, this._ordinals.Count);
                this._names.Add(this._names.Count, item.Item1);
            }

        }

        public override bool Read()
        {
            return this._enumerator.MoveNext();
        }

        public override int GetOrdinal(string name)
        {
            return this._ordinals[name];
        }

        public override int FieldCount
        {
            get
            {
                return this._columnsByName.Count;
            }
        }


        public override object this[int ordinal] { get => _columnsByOrdinal[ordinal]; }

        public override object this[string name] { get => _columnsByName[name]; }

        public override int Depth
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
                return _enumerator.Current != null;
            }
        }

        public override bool IsClosed
        {
            get
            {
                return false;
            }
        }

        public override int RecordsAffected { get => 0; }

        public override IEnumerator GetEnumerator() => this._enumerator;

        public override bool GetBoolean(int ordinal)
        {
            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return false;

            if (value is bool b)
                return b;

            if (value is int c)
                return c != 0;

            if (value is string d)
            {
                d = d.ToLower().Trim();
                switch (d)
                {
                    case "true":
                    case "vrai":
                    case "-1":
                    case "1":
                        return true;
                    default:
                        return false;
                }
            }

            return (bool)Convert.ChangeType(value, typeof(bool));

            return false;
        }

        public override byte GetByte(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (byte)0;

            if (value is byte b)
                return b;

            if (value is char c)
                return (byte)c;

            return (byte)(object)value;

        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            return 0;
        }

        public override char GetChar(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (char)0;

            if (value is char b)
                return b;

            if (value is byte c)
                return (char)c;

            return (char)(object)value;

        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            return 0;
        }

        public override string GetDataTypeName(int ordinal)
        {
            return string.Empty;
        }

        public override Type GetFieldType(int ordinal)
        {
            return typeof(string);
        }

        public override DateTime GetDateTime(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return DateTime.MinValue;

            if (value is DateTime b)
                return b;

            return (DateTime)(object)value;

        }

        public override decimal GetDecimal(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (decimal)0;

            if (value is decimal b)
                return b;

            return (decimal)(object)value;

        }

        public override double GetDouble(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (double)0;

            if (value is double b)
                return b;

            return (double)(object)value;

        }

        public override float GetFloat(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (float)0;

            if (value is float b)
                return b;

            return (float)(object)value;
        }

        public override Guid GetGuid(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return Guid.Empty;

            if (value is Guid b)
                return b;

            if (value is string c)
                return new Guid(c);

            return (Guid)(object)value;

        }

        public override short GetInt16(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (short)0;

            if (value is short v0)
                return v0;

            if (value is long v1)
                return Convert.ToInt16(v1);

            if (value is int v2)
                return Convert.ToInt16(v2);

            if (value is byte v3)
                return v3;

            return (short)(object)value;

        }

        public override int GetInt32(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (int)0;

            if (value is int v0)
                return v0;

            if (value is long v1)
                return Convert.ToInt32(v1);

            if (value is short v2)
                return (int)v2;

            if (value is byte v3)
                return v3;

            return (int)(object)value;


        }

        public override long GetInt64(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return (long)0;

            if (value is long v0)
                return v0;

            if (value is int v1)
                return Convert.ToInt64(v1);

            if (value is short v2)
                return Convert.ToInt64(v2);

            if (value is byte v3)
                return v3;

            return (Int64)(object)value;
        }

        public override string GetName(int ordinal) => _names[ordinal];

        public override string GetString(int ordinal)
        {

            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            if (value == null || (object)value == DBNull.Value)
                return null;

            if (value is string b)
                return b;

            return value.ToString();

        }

        public override object GetValue(int ordinal)
        {
            var value = _columnsByOrdinal[ordinal](_enumerator.Current);

            return value;
        }

        public override int GetValues(object[] values)
        {
            return 0;
        }

        public override bool IsDBNull(int ordinal)
        {
            var value = _columnsByOrdinal[ordinal](_enumerator.Current);
            return value == null || ((object)value) == DBNull.Value;
        }

        public override bool NextResult()
        {
            var result = _enumerator.MoveNext();
            return result;
        }


        private readonly IEnumerable<T> _items;
        private readonly IEnumerator<T> _enumerator;
        private readonly Dictionary<string, Func<T, object>> _columnsByName;
        private readonly Dictionary<int, Func<T, object>> _columnsByOrdinal;
        private readonly Dictionary<string, int> _ordinals;
        private readonly Dictionary<int, string> _names;
    }


}
