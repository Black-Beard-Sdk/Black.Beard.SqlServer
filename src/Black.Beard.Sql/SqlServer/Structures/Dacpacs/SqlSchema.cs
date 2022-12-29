namespace Bb.SqlServer.Structures.Dacpacs
{
    public class SqlSchema : DacElement
    {

        public SqlSchema()
            : base(ElementTypePropertyValue.SqlSchema)
        {

        }

        //public SqlFilegroup SetDescription(string description, string @namespace, string table)
        //{

        //    var property = new DacProperty("Value")
        //    {
        //        Inline = false,
        //    }.SetValue(new StringPropertyValue(description))
        //    ;

        //    this.Properties.Add(property);

        //    this.Relationship(RelationshipNamePropertyValue.Host, c =>
        //    {
        //        c.Entry
        //        (
        //          e =>
        //          {
        //              e.References($"[{Dequote(@namespace)}].[{Dequote(table)}].[{Dequote(field)}]");
        //          }
        //        );
        //    });

        //    return this;

        //}

        //public SqlFilegroup SetDescription(string description, string @namespace, string table)
        //{

        //    var property = new DacProperty("Value")
        //    {
        //        Inline = false,
        //    }.SetValue(new StringPropertyValue(description))
        //    ;

        //    this.Properties.Add(property);

        //    this.Relationship(RelationshipNamePropertyValue.Host, c =>
        //    {
        //        c.Entry
        //        (
        //          e =>
        //          {
        //              e.References($"[{Dequote(@namespace)}].[{Dequote(table)}]");
        //          }
        //        );
        //    });

        //    return this;

        //}

    }



}
