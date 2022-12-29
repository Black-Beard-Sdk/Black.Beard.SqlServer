namespace Bb.SqlServer.Structures.Dacpacs
{

    public class RelationshipNamePropertyValue : PropertyValue
    {

        private RelationshipNamePropertyValue(string key)
            : base(key)
        {

        }

        public static RelationshipNamePropertyValue Column = new RelationshipNamePropertyValue("Column");
        public static RelationshipNamePropertyValue Columns = new RelationshipNamePropertyValue("Columns");
        public static RelationshipNamePropertyValue ForeignColumns = new RelationshipNamePropertyValue("ForeignColumns");
        public static RelationshipNamePropertyValue ColumnSpecifications = new RelationshipNamePropertyValue("ColumnSpecifications");
        public static RelationshipNamePropertyValue DefaultFilegroup = new RelationshipNamePropertyValue("DefaultFilegroup");
        public static RelationshipNamePropertyValue DefiningTable = new RelationshipNamePropertyValue("DefiningTable");
        public static RelationshipNamePropertyValue Filegroup = new RelationshipNamePropertyValue("Filegroup");
        public static RelationshipNamePropertyValue IndexedObject = new RelationshipNamePropertyValue("IndexedObject");
        public static RelationshipNamePropertyValue ForeignTable = new RelationshipNamePropertyValue("ForeignTable");
        public static RelationshipNamePropertyValue TypeSpecifier = new RelationshipNamePropertyValue("TypeSpecifier");
        public static RelationshipNamePropertyValue Type = new RelationshipNamePropertyValue("Type");
        public static RelationshipNamePropertyValue Schema = new RelationshipNamePropertyValue("Schema");
        public static RelationshipNamePropertyValue Host = new RelationshipNamePropertyValue("Host");
        public static RelationshipNamePropertyValue Authorizer = new RelationshipNamePropertyValue("Authorizer");



        public static RelationshipNamePropertyValue Empty = new RelationshipNamePropertyValue(string.Empty);

    }




}
