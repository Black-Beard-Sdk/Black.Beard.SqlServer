using System.Text;
using System;

namespace Bb.SqlServer.Queries
{
    public class ForseekTableHint : TableHint
    {

        internal ForseekTableHint() 
            : base ("FORCESEEK")
        {

        }

        public ForseekTableHint(string? indexName = null, params string[] columnNames) 
            : this()
        {
            this.IndexValue = indexName;
            this.ColumnNames = columnNames;
        }

        public string? IndexValue { get; }

        public string[] ColumnNames { get; }


        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(this.Hint);

            var b = string.IsNullOrEmpty(this.IndexValue);

            if (b)
            {

                sb.Append("(");
                sb.Append(IndexValue);
            }

            if (this.ColumnNames.Length > 0)
            {
                sb.Append(" (");
                string comma = string.Empty;
                foreach (var item in ColumnNames)
                {
                    sb.Append(comma);
                    sb.Append(item);
                    comma = ", ";
                }
                sb.Append(")");
            }

            if (b)
            {
                sb.Append(")");
            }

            return sb.ToString();

        }



    }

}
