using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacEntry : DacListOfModel<DacEntityContains>
    {


        public DacEntry()
            : base("Entry")
        {

        }


        public override XElement Serialize()
        {
            var xml = new XElement(XName.Get(Key));

            foreach (var item in this)
                xml.Add(item.Serialize());

            return xml;
        }

        public DacEntry SqlIndexedColumnSpecification(Action<SqlIndexedColumnSpecification> action)
        {
            var item = new SqlIndexedColumnSpecification();
            Add(item);
            action(item);
            return this;
        }

        public DacEntry Column(string columnName, Action<DacSqlSimpleColumn> action)
        {
            var item = new DacSqlSimpleColumn()
            {
                Name = columnName,
            };
            Add(item);
            action(item);
            return this;
        }

        public DacEntry References(string name, string ExternalSource = null)
        {

            var item = new DacReferences();

            if (ExternalSource != null)
                item.ExternalSource = ExternalSource;

            item.Name = name;

            Add(item);

            return this;

        }

        public DacEntry Element(ElementTypePropertyValue type, Action<DacElement> action)
        {
            var item = new DacElement(type);
            Add(item);
            action(item);
            return this;

        }

        protected override T1 Create<T1>()
        {
            T1 value = default;

            if (typeof(T1) == typeof(DacReferences))
                value = (T1)(object)new DacReferences();
            else
                value = (T1)(object)new DacElement(ElementTypePropertyValue.Empty);

            return value;

        }

    }


}
