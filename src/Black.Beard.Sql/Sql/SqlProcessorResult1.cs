namespace Bb.Sql
{
    public class SqlProcessorResult1<T> : SqlProcessorResult
    {

        public new T Item { get => (T)base.Item; internal set => base.Item = (T)value; }


    }


}


