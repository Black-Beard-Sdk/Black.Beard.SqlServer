using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class ContentTypes
    {

        public static string GetToString()
        {

            var txt = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                + Environment.NewLine
                + Get()
                .ToString(SaveOptions.OmitDuplicateNamespaces)
                .Replace(" xmlns=\"\"", "");

            return txt;

        }

        public static XDocument Get()
        {

            var doc = new XDocument
            (
                new XElement(XName.Get("Types", "http://schemas.openxmlformats.org/package/2006/content-types")
                    , new XAttribute("xmlns", "http://schemas.openxmlformats.org/package/2006/content-types")
                    , new XElement(XName.Get("Default")
                        , new XAttribute(XName.Get("Extension"), "xml")
                        , new XAttribute(XName.Get("ContentType"), "text/xml")
                    )
                )
            );

            return doc;

        }

    }

}
