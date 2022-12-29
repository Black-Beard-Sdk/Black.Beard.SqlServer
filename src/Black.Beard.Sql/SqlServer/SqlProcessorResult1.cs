namespace Bb.SqlServerStructures
{
    public class SqlProcessorResult1<T> : SqlProcessorResult
    {

        public new T Item { get => (T)base.Item; internal set => base.Item = (T)value; }


    }


}


