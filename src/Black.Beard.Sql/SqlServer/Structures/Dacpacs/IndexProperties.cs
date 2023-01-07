namespace Bb.SqlServer.Structures.Dacpacs
{
    public class IndexProperties
    {

        public bool PadIndex { get; set; } = false;

        public bool StatisticsNorecompute { get; set; } = false;

        public bool AllowRowLocks { get; set; } = true;

        public bool AllowPageLocks { get; set; } = true;

        public bool OptimizeForSequentialKey { get; set; } = false;


        public virtual bool IsDifferent(IndexProperties target)
        {

            if (this.PadIndex != target.PadIndex)
                return true;

            if (this.StatisticsNorecompute != target.StatisticsNorecompute)
                return true;

            if (this.AllowRowLocks != target.AllowRowLocks)
                return true;

            if (this.AllowPageLocks != target.AllowPageLocks)
                return true;

            if (this.OptimizeForSequentialKey != target.OptimizeForSequentialKey)
                return true;

            return false;

        }

        internal void CloneFrom(IndexProperties source)
        {
            this.PadIndex = source.PadIndex;
            this.StatisticsNorecompute = source.StatisticsNorecompute;
            this.AllowPageLocks= source.AllowPageLocks;
            this.AllowRowLocks= source.AllowRowLocks;
            this.OptimizeForSequentialKey= source.OptimizeForSequentialKey;
        }

        // public bool DropExisting { get; set; } = false;
        // public bool Online { get; set; } = false;
        // public bool SortInTempdb { get; set; } = false;

    }

}
