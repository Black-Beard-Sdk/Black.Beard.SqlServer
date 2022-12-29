using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacOrigin
    {

        public static string GetToString(Guid identity, DateTime start, DateTime end, string checksum)
        {

            var txt = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                + Environment.NewLine
                + Get(identity, start, end, checksum)
                .ToString(SaveOptions.OmitDuplicateNamespaces)
                .Replace(" xmlns=\"\"", "");

            return txt;

        }

        public static XDocument Get(Guid identity, DateTime start, DateTime end, string checksum)
        {

            var doc = new XDocument
            (
                new XElement(XName.Get("DacOrigin", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")

                    , new XAttribute("xmlns", "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")

                    , new XElement(XName.Get("PackageProperties")

                        , new XElement(XName.Get("Version"), "3.0.0.0")
                        , new XElement(XName.Get("ContainsExportedData"), "false")

                        , new XElement(XName.Get("StreamVersions")
                            , new XElement(XName.Get("Version"), new XAttribute(XName.Get("StreamName"), "Data"), "2.0.0.0")
                            , new XElement(XName.Get("Version"), new XAttribute(XName.Get("StreamName"), "DeploymentContributors"), "1.0.0.0")
                        )

                    )

                    , new XElement(XName.Get("Operation")

                        , new XElement(XName.Get("Identity"), identity.ToString("D"))
                        , new XElement(XName.Get("Start"), start.ToString("o"))         //  "2022-04-12T20:15:24.6994236+02:00"
                        , new XElement(XName.Get("End"), end.ToString("o"))
                        , new XElement(XName.Get("ProductName"), "Microsoft.Data.Tools.Schema.Tasks.Sql, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
                        , new XElement(XName.Get("ProductVersion"), "17.0.62203.25070")
                        , new XElement(XName.Get("ProductSchema"), @"http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")
                    )

                    , new XElement(XName.Get("Checksums")
                        , new XElement(XName.Get("Checksum")
                        , new XAttribute(XName.Get("Uri"), "/model.xml"), checksum)
                    )

                    , new XElement(XName.Get("ModelSchemaVersion"), "2.9")

                )

            );

            return doc;

        }

    }

    // 
}
