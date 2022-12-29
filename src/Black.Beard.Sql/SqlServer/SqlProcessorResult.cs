namespace Bb.SqlServerStructures
{
    public class SqlProcessorResult
    {

        public int CountInpactedObjects { get; internal set; }

        public Exception Exception { get; internal set; }

        public bool Success { get; internal set; }

        public virtual object Item { get; internal set; }


        public static implicit operator bool(SqlProcessorResult item)
        {
            return item.Success;
        }

        public static implicit operator int(SqlProcessorResult item)
        {
            return item.CountInpactedObjects;
        }


    }


}


