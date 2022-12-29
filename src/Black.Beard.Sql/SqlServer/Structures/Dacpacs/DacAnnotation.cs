using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{


    public class DacAnnotation : DacEntityContains
    {

        public DacAnnotation(string key = null)
            : base(key ?? "Annotation")
        {

            //Type = "SqlInlineConstraintAnnotation";
            //Disambiguator = 3;
        }

        public AnnotationTypePropertyValue Type
        {
            get => GetValue<AnnotationTypePropertyValue>("Type");
            set => Set("Type", value);
        }

        public IntPropertyValue Disambiguator
        {
            get => GetValue<IntPropertyValue>("Disambiguator");
            set => Set("Disambiguator", value);
        }

        public override XElement Serialize()
        {

            var xml = new XElement(XName.Get(Key));

            if (Exists("Type"))
                xml.Add(Get("Type").SerializeToAttribute());

            if (Exists("Disambiguator"))
                xml.Add(Get("Disambiguator").SerializeToAttribute());

            return xml;

        }

        private PropertyValue _value;

    }


}
