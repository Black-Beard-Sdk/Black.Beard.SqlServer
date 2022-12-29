using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacProperty : ModelBase
    {

        public DacProperty(string key = null)
            : base(key ?? "Property")
        {

        }

        public bool Inline { get; set; } = true;

        public string Name { get; set; }

        public T GetValue<T>()
            where T : PropertyValue
        {
            return (T)_value;
        }

        public DacProperty SetValue<T>(T value)
            where T : PropertyValue
        {
            _value = value;
            return this;
        }

        public override XElement Serialize()
        {
            var xml = new XElement(XName.Get(Key));

            xml.Add(new XAttribute(XName.Get("Name"), Name));

            if (Inline)
            {
                xml.Add(new XAttribute(XName.Get("Value"), _value.Value));
            }
            else
            {
                var value = new XElement(XName.Get("Value"));
                value.Add(new XCData(_value.Value));
                xml.Add(value);
            }

            return xml;
        }

        private PropertyValue _value;

    }



}
