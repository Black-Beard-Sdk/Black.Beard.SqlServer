using System.Text;

namespace Bb.SqlServer.Queries
{
    public class NoExpandTableHint : TableHint
    {

        internal NoExpandTableHint(params TableHintIndex[] indexes)
            : base("NOEXPAND")
        {
            this.Indexes = indexes;
        }

        public TableHintIndex[] Indexes { get; }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(this.Hint);
            if (this.Indexes.Length > 0)
            {
                
                string comma = string.Empty;
                foreach (var item in Indexes)
                {
                    sb.Append(item.ToString());
                    sb.Append(item);
                    comma = ", ";
                }
                sb.Append(" ");
            }

            return sb.ToString();

        }


    }

}
