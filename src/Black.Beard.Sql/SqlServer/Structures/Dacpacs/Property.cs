using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class Property
    {

        public Property(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public string Value { get => PropertyValue?.Value; }

        public void Set(PropertyValue value)
        {
            PropertyValue = value;
        }

        public XElement SerializeToAttribute(XElement parent)
        {
            parent.Add(new XAttribute("Name", Key));
            parent.Add(new XAttribute("Value", Value));
            return parent;
        }

        public XAttribute SerializeToAttribute()
        {
            return new XAttribute(Key, Value);
        }

        public XElement SerializeToElement()
        {
            return new XElement(Key, Value);
        }

        public PropertyValue PropertyValue { get; private set; }

    }

}
