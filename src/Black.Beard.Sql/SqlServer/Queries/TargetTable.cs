namespace Bb.SqlServer.Queries
{
    public class TargetTable
    {

        public TargetTable(params string[] targetTableReferences)
            : base()
        {
            TargetName = SqlLabelReference.Create(targetTableReferences);
            this.Hints = new WithTableMergeHints();
        }

        public SqlLabelReference TargetName { get; }

        public WithTableMergeHints Hints { get; }

        public string Alias { get; set; }

        public TargetTable As(string alias)
        {
            this.Alias = alias;
            return this;
        }

    }

}
