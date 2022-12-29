using System.Xml.Linq;
using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.Dacpacs
{
    public partial class DacDataSchemaModel : ModelBase
    {

        public DacDataSchemaModel()
            : base("DataSchemaModel")
        {

            FileFormatVersion = FileFormatVersionPropertyValue.Version12;
            SchemaVersion = SchemaVersionPropertyValue.Version29;
            DspName = DspNamePropertyValue.Value;
            CollationLcid = CollationLcidPropertyValue.Collation1033;
            CollationCaseSensitive = BooleanPropertyValue.False;

            this.Header = new DacHeader();
            this.Model = new DacModel();

        }

        public FileFormatVersionPropertyValue FileFormatVersion
        {
            get => GetValue<FileFormatVersionPropertyValue>("FileFormatVersion");
            set => Set("FileFormatVersion", value);
        }

        public SchemaVersionPropertyValue SchemaVersion
        {
            get => GetValue<SchemaVersionPropertyValue>("SchemaVersion");
            set => Set("SchemaVersion", value);
        }

        public DspNamePropertyValue DspName
        {
            get => GetValue<DspNamePropertyValue>("DspName");
            private set => Set("DspName", value);
        }

        public CollationLcidPropertyValue CollationLcid
        {
            get => GetValue<CollationLcidPropertyValue>("CollationLcid");
            private set => Set("CollationLcid", value);
        }

        public BooleanPropertyValue CollationCaseSensitive
        {
            get => GetValue<BooleanPropertyValue>("CollationCaseSensitive");
            private set => Set("CollationCaseSensitive", value);
        }

        public DacHeader Header { get; }

        public DacModel Model { get; }

        public XDocument SerializeRoot()
        {
            XDocument xml = new XDocument();
            xml.Add(Serialize());
            return xml;
        }

        public override XElement Serialize()
        {

            var xml = new XElement(XName.Get(this.Key, "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02"));

            xml.Add(Get("FileFormatVersion").SerializeToAttribute());
            xml.Add(Get("SchemaVersion").SerializeToAttribute());
            xml.Add(Get("DspName").SerializeToAttribute());
            xml.Add(Get("CollationLcid").SerializeToAttribute());
            xml.Add(Get("CollationCaseSensitive").SerializeToAttribute());

            xml.Add(new XAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02"));

            xml.Add(Header.Serialize());
            
            xml.Add(Model.Serialize());

            return xml;
        }

    }



}
