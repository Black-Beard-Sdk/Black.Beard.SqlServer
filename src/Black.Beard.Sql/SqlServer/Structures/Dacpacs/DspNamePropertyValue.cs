namespace Bb.SqlServer.Structures.Dacpacs
{
    public class DspNamePropertyValue : PropertyValue
    {

        private DspNamePropertyValue(string key)
            : base(key)
        {

        }

        public static DspNamePropertyValue Value = new DspNamePropertyValue("Microsoft.Data.Tools.Schema.Sql.Sql130DatabaseSchemaProvider");

    }




}
