namespace Bb.SqlServer.Structures.Dacpacs
{
    public class CollationPropertyValue : PropertyValue
    {

        private CollationPropertyValue(string key)
            : base(key)
        {

        }

        public static CollationPropertyValue SQL_Latin1_General_CP1_CI_AS = new CollationPropertyValue("SQL_Latin1_General_CP1_CI_AS");

    }




}
