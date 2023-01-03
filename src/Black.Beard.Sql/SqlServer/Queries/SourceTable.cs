namespace Bb.SqlServer.Queries
{
    public class SourceTable
    {

        public SourceTable(params string[] targetTableReferences)
        {
            SourceName = SqlLabelReference.Create(targetTableReferences);
        }

        public SqlLabelReference SourceName { get; }

        public string Alias { get; set; }

        public SourceTable As(string alias)
        {
            this.Alias = Alias;
            return this;
        }

    }

}
