namespace Bb.SqlServer.Queries
{
    public class Top
    {

        public int Limit { get; set; }

        public TopModeEnum Mode { get; set; }

    }

    public enum TopModeEnum
    {
        Count,
        Percent
    }

}
