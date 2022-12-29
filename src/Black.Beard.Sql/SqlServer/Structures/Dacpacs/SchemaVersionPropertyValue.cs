namespace Bb.SqlServer.Structures.Dacpacs
{
    public class SchemaVersionPropertyValue : PropertyValue
    {

        private SchemaVersionPropertyValue(string key)
            : base(key)
        {

        }

        public static SchemaVersionPropertyValue Version29 = new SchemaVersionPropertyValue("2.9");

    }




}
