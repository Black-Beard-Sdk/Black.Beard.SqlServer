namespace Bb.SqlServer.Structures.Dacpacs
{
    public class BooleanPropertyValue : PropertyValue
    {

        private BooleanPropertyValue(string key)
            : base(key)
        {

        }

        public static BooleanPropertyValue False = new BooleanPropertyValue("False");
        public static BooleanPropertyValue True = new BooleanPropertyValue("True");

    }




}
