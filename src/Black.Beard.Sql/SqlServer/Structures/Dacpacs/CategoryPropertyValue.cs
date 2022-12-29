namespace Bb.SqlServer.Structures.Dacpacs
{
    public class CategoryPropertyValue : PropertyValue
    {

        private CategoryPropertyValue(string key)
            : base(key)
        {

        }

        public static CategoryPropertyValue AnsiNulls = new CategoryPropertyValue("AnsiNulls");
        public static CategoryPropertyValue QuotedIdentifier = new CategoryPropertyValue("QuotedIdentifier");
        public static CategoryPropertyValue CompatibilityMode = new CategoryPropertyValue("CompatibilityMode");
        public static CategoryPropertyValue Reference = new CategoryPropertyValue("Reference");
        public static CategoryPropertyValue SqlCmdVariables = new CategoryPropertyValue("SqlCmdVariables");

        // 
    }




}
