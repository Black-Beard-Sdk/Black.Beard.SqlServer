using System.Text;

namespace Bb.SqlServer.Queries
{


    public class TableHintIndex : TableHint
    {

        internal TableHintIndex(params string[] indexValues) : base("INDEX")
        {

            this.IndexValues = indexValues;

        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(this.Hint);
            if (this.IndexValues.Length > 0)
            {
                sb.Append(" (");
                string comma = string.Empty;
                foreach (var item in IndexValues)
                {
                    sb.Append(comma);
                    sb.Append(item);
                    comma = ", ";
                }
                sb.Append(")");
            }

            return sb.ToString();

        }

        public string[] IndexValues { get; }


    }

}
