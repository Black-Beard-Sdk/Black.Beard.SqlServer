using System.Text;

namespace Bb.SqlServer.Queries
{
    public class SpatialWindowMaxCellsTableHint : TableHint
    {

        internal SpatialWindowMaxCellsTableHint(int value) 
            : base("SPATIAL_WINDOW_MAX_CELLS")
        {
            this.Value = value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Hint);
            sb.Append(" = ");
            sb.Append(this.Value);
            return sb.ToString();
        }

        public int Value { get; }

    }

}
