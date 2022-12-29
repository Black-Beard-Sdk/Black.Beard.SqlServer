using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacElement : DacEntityContains
    {


        public DacElement(ElementTypePropertyValue type, string key = null)
            : base(key ?? "Element")
        {

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Type = type;
            Properties = new DacProperties();
            Relationships = new DacRelationships();
            Annotations = new DacAnnotations();

        }


        public ElementTypePropertyValue Type
        {
            get => GetValue<ElementTypePropertyValue>("Type");
            private set => Set("Type", value);
        }

        public StringPropertyValue Name
        {
            get => GetValue<StringPropertyValue>("Name");
            set => Set("Name", value);
        }

        public DacProperties Properties { get; }

        public DacRelationships Relationships { get; }

        public DacAnnotations Annotations { get; }

        public DacElement Relationship(RelationshipNamePropertyValue name, Action<DacRelationship> action)
        {
            var e = new DacRelationship()
            {
                Name = name
            };
            Relationships.Add(e);
            action(e);
            return this;
        }

        public DacElement Annotation(AnnotationTypePropertyValue type, Action<DacAnnotation> action)
        {
            var e = new DacAnnotation()
            {
                Type = type
            };
            Annotations.Add(e);
            action(e);
            return this;
        }

        public DacElement AttachedAnnotation(AnnotationTypePropertyValue type, Action<DacAnnotation> action)
        {
            var e = new DacAnnotation("AttachedAnnotation");

            if (type != null)
                e.Type = type;

            Annotations.Add(e);
            action(e);

            return this;
        }

        public DacElement Property(string name, string value)
        {
            var e = new DacProperty()
            {
                Name = name
            };
            e.SetValue<StringPropertyValue>(value)
            ;
            Properties.Add(e);
            return this;
        }

        public DacElement Property(Action<DacProperty> action)
        {
            var e = new DacProperty();
            Properties.Add(e);
            action(e);
            return this;
        }

        public override XElement Serialize()
        {

            var xml = new XElement(XName.Get(Key)
                    , new XAttribute(XName.Get("Type"), Type.Value)
                );

            if (Exists("Name"))
                xml.Add(Get("Name").SerializeToAttribute());


            foreach (var item in Properties)
                xml.Add(item.Serialize());

            foreach (var item in Relationships)
                xml.Add(item.Serialize());

            foreach (var item in Annotations)
                xml.Add(item.Serialize());

            return xml;

        }


    }

}
