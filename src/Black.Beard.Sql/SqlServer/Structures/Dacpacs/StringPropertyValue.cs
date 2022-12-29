namespace Bb.SqlServer.Structures.Dacpacs
{
    public class StringPropertyValue : PropertyValue
    {

        public StringPropertyValue(string key)
            : base(key)
        {

        }

        public static implicit operator StringPropertyValue(string value)
        {
            return new StringPropertyValue(value.ToString());
        }


    }




}
