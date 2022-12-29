using System.Text;
using System.Text.RegularExpressions;

namespace Bb.SqlServerStructures
{


    public class ScriptItem
    {

        public ScriptItem(int position, string value)
        {

            this.Position = position;
            _sql = new StringBuilder(value.Trim());

            IsCreateDatabase = _createDatabase.IsMatch(_sql.ToString());
            IsAlterDatabase = _alterDatabase.IsMatch(_sql.ToString());
            IsUseDatabase = _useDatabase.IsMatch(_sql.ToString());

        }

        public override string ToString()
        {
            return _sql.ToString();
        }

        public int Position { get; }

        private StringBuilder _sql;

        public bool IsCreateDatabase { get; }
        public bool IsAlterDatabase { get; }
        public bool IsUseDatabase { get; }


        private Regex _createDatabase = new Regex(@"CREATE\s+DATABASE\s+\[\w*.\]\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private Regex _alterDatabase = new Regex(@"ALTER\s+DATABASE\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private Regex _useDatabase = new Regex(@"USE\s+\[\w*.\]\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase);


    }

}