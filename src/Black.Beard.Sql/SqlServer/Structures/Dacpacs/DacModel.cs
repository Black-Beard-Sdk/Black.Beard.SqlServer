using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacModel : DacListOfModel<DacElement>
    {

        public DacModel() : base("Model")
        {
            AddSqlDatabaseOptions();
        }

        public DacModel AddSqlDatabaseOptions()
        {
            var item = new DacSqlDatabaseOptions();
            Add(item);
            return this;

        }

        public DacModel SqlForeignKeyConstraint(string name, Action<SqlForeignKeyConstraint> action)
        {
            var item = new SqlForeignKeyConstraint()
            {
                Name = name
            };
            Add(item);
            action(item);
            return this;
        }

        public DacModel SqlTable(string name, Action<SqlTable> action)
        {
            var item = new SqlTable()
            {
                Name = name,
            };
            Add(item);
            action(item);
            return this;
        }

        public DacModel SqlDescription(string @namespace, string table, string field, string description)
        {

            var name = $"[SqlColumn].[{Dequote(@namespace)}].[{Dequote(table)}].[{Dequote(field)}].[MS_Description]";

            var item = new SqlExtendedProperty()
            {
                Name = name,
            };
            Add(item);

            Action<SqlExtendedProperty> action = a =>
            {
                a.SetDescription(description, @namespace, table, field);
            };

            action(item);

            return this;

        }

        public DacModel SqlDescription(string @namespace, string table, string description)
        {

            var name = $"[SqlColumn].[{Dequote(@namespace)}].[{Dequote(table)}].[MS_Description]";

            var item = new SqlExtendedProperty()
            {
                Name = name,
            };
            Add(item);

            Action<SqlExtendedProperty> action = a =>
            {
                a.SetDescription(description, @namespace, table);
            };

            action(item);

            return this;

        }

        public DacModel SqlPrimaryKeyConstraint(Action<SqlPrimaryKeyConstraint> action)
        {
            var item = new SqlPrimaryKeyConstraint();
            Add(item);
            action(item);
            return this;
        }

        public DacModel SqlIndex(Action<SqlIndex> action)
        {
            var item = new SqlIndex();
            Add(item);
            action(item);
            return this;
        }

        public DacModel Filegroup(Action<SqlFilegroup> action)
        {
            var item = new SqlFilegroup();
            Add(item);
            action(item);
            return this;
        }

        public DacModel Schema(Action<SqlSchema> action)
        {
            var item = new SqlSchema();
            Add(item);
            action(item);
            return this;
        }

        public override XElement Serialize()
        {

            var xml = new XElement(XName.Get(Key));

            foreach (var item in this)
                xml.Add(item.Serialize());

            return xml;

        }

    }



}
