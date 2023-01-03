using SharpCompress.Writers;
using System.Text;

namespace Bb.SqlServer.Queries
{
    public class WithTableMergeHints
    {

        public WithTableMergeHints()
        {
            this._hints = new List<TableHint>();
        }

        public WithTableMergeHints Add(params TableHintIndex[] hints)
        {
            this._hints.AddRange(hints);
            return this;
        }

        public WithTableMergeHints Add(params TableHintLimited[] hints)
        {
            this._hints.AddRange(hints);
            return this;
        }

        public bool Any { get => _hints.Any(); }

        public IEnumerable<TableHint> Items { get => _hints; }


        public override string ToString()
        {

            string comma = string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var item in Items)
            {
                sb.Append(comma);
                sb.Append(item.ToString());
                comma = ", ";
            }

            return sb.ToString();

        }

        private readonly List<TableHint> _hints;


    }

}
