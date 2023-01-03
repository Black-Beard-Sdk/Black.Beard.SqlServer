using Bb.SqlServer.Structures.Dacpacs;
using System.Text;

namespace Bb.SqlServer.Queries
{


    public class SqlLabelReference : SqlExpr
    {

        public SqlLabelReference(string label)
        {
            this.Label = label;
        }

        public SqlLabelReference? Child { get; private set; }

        public static SqlLabelReference Create(params string[] targetReferences)
        {

            if (targetReferences.Length == 0)
                throw new InvalidOperationException($"{nameof(targetReferences)} can't be empty list");

            SqlLabelReference first = null;

            for (int i = 0; i < targetReferences.Length; i++)
            {
                if (first == null)
                    first = targetReferences[i];
                else
                    first.Map(targetReferences[i]);
            }

            if (first == null)
                throw new InvalidOperationException($"{nameof(targetReferences)} can't be empty list");

            return first;

        }


        public string Write(ScriptContext? ctx = null)
        {
            StringBuilder sb = new StringBuilder();
            Write(sb, ctx);
            return sb.ToString();
        }

        public override void Accept(QueryBaseVisitor visitor)
        {
            visitor.VisitLabelReference(this);
        }

        private void Write(StringBuilder sb, ScriptContext? ctx)
        {

            if (!string.IsNullOrEmpty(this.Label))
            {

                if (ctx != null)
                {
                    var datas = ctx.ReplaceVariables(this.Label);
                    if (!string.IsNullOrEmpty(datas))
                    {
                        if (sb.Length > 0)
                            sb.Append(".");
                        sb.Append($"[{datas}]");
                    }
                }

            }

            if (Child != null)
                Child.Write(sb, ctx);

        }

        public void Map(string targetReferences)
        {
            if (Child == null)
                Child = targetReferences;
            else
                Child.Map(targetReferences);
        }

        public void Map(SqlLabelReference newtChild)
        {
            if (Child == null)
                Child = newtChild;
            else
                Child.Map(newtChild);
        }

        public string Label { get; }


        public static implicit operator SqlLabelReference(string label)
        {
            return new SqlLabelReference(label);
        }

    }


}
