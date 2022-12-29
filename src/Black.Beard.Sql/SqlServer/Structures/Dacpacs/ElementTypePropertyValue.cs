namespace Bb.SqlServer.Structures.Dacpacs
{
    public class ElementTypePropertyValue : PropertyValue
    {

        private ElementTypePropertyValue(string key)
            : base(key)
        {

        }

        public static ElementTypePropertyValue SqlDatabaseOptions = new ElementTypePropertyValue("SqlDatabaseOptions");
        public static ElementTypePropertyValue SqlIndex = new ElementTypePropertyValue("SqlIndex");
        public static ElementTypePropertyValue SqlPrimaryKeyConstraint = new ElementTypePropertyValue("SqlPrimaryKeyConstraint");
        public static ElementTypePropertyValue SqlTable = new ElementTypePropertyValue("SqlTable");
        public static ElementTypePropertyValue SqlForeignKeyConstraint = new ElementTypePropertyValue("SqlForeignKeyConstraint");
        public static ElementTypePropertyValue SqlIndexedColumnSpecification = new ElementTypePropertyValue("SqlIndexedColumnSpecification");
        public static ElementTypePropertyValue SqlSimpleColumn = new ElementTypePropertyValue("SqlSimpleColumn");
        public static ElementTypePropertyValue SqlTypeSpecifier = new ElementTypePropertyValue("SqlTypeSpecifier");
        public static ElementTypePropertyValue SqlExtendedProperty = new ElementTypePropertyValue("SqlExtendedProperty");
        public static ElementTypePropertyValue SqlFilegroup = new ElementTypePropertyValue("SqlFilegroup");
        public static ElementTypePropertyValue SqlSchema = new ElementTypePropertyValue("SqlSchema");



        public static ElementTypePropertyValue Empty = new ElementTypePropertyValue(string.Empty);

    }




}
