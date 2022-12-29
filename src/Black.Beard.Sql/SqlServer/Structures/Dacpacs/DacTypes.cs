using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacTypes
    {

        public static string GetToString(string name, string version)
        {

            var txt = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                + Environment.NewLine
                + Get(name, version)
                .ToString(SaveOptions.OmitDuplicateNamespaces)
                .Replace(" xmlns=\"\"", "");

            return txt;

        }

        public static XDocument Get(string name, string version)
        {

            var doc = new XDocument
            (
                new XElement(XName.Get("DacType", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")
                    , new XAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")
                    , new XElement(XName.Get("Name"), name)
                    , new XElement(XName.Get("Version"), version)

                )
            );

            return doc;

        }

    }

}
