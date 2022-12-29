using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{
    public class DacCustomData : DacListOfModel<DacMetadata>
    {

        public DacCustomData(CategoryPropertyValue category = null)
            : base("CustomData")
        {
            if (category != null)
                Category = category;
        }

        public CategoryPropertyValue Category
        {
            get => GetValue<CategoryPropertyValue>("Category");
            set => Set("Category", value);
        }

        public StringPropertyValue Type
        {
            get => GetValue<StringPropertyValue>("Type");
            set => Set("Type", value);

        }
        public DacCustomData Metadata(string key, string value)
        {

            var metadata = new DacMetadata()
            {
                Name = new StringPropertyValue(key),
                Value = new StringPropertyValue(value),
            };

            Add(metadata);

            return this;

        }


        public override XElement Serialize()
        {

            var xml = new XElement(XName.Get(Key));

            xml.Add(Get("Category").SerializeToAttribute());

            if (Exists("Type"))
                xml.Add(Get("Type").SerializeToAttribute());

            foreach (var item in this)
                xml.Add(item.Serialize());

            return xml;

        }

    }



}
