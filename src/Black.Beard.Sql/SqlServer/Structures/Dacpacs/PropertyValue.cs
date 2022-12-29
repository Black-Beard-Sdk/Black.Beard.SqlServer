namespace Bb.SqlServer.Structures.Dacpacs
{

    public class PropertyValue
    {

        protected PropertyValue(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

    }

}
