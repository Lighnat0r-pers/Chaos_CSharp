using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GTAVC_Chaos
{
    static class XmlUtils
    {
        static private string baseResourceString = "GTAVC_Chaos.Resources.";
        static private string dataDirectory = "Data\\";

        static private void xmlValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Debug.WriteLine("WARNING: " + e.Message);
            }
            else
            {
                Debug.WriteLine("ERROR: " + e.Message);
            }
        }

        static private XmlSchemaSet getXmlSchemaSet(string filename)
        {
            var schemas = new XmlSchemaSet();

            // NOTE(Ligh): Gets the schema from an external file.
            //var x = new XmlTextReader(resourceDirectory + filename);

            // NOTE(Ligh): Gets the schema from an embedded resource.
            var x = Assembly.GetExecutingAssembly().GetManifestResourceStream(baseResourceString + filename);

            schemas.Add(XmlSchema.Read(x, null));

            return schemas;
        }

        static public XmlDocument getXmlDocument(string prefix, string filename)
        {
            // TODO(Ligh): Deal with errors (file not found etc) here.

            var document = new XmlDocument();
            document.Load(dataDirectory + prefix + filename + ".xml");
            document.Schemas = getXmlSchemaSet(filename + ".xsd");
            document.Validate(new ValidationEventHandler(xmlValidationEventHandler));

            return document;
        }

        static public XDocument getXDocument(string prefix, string filename)
        {
            // TODO(Ligh): Deal with errors (file not found etc) here.

            var document = XDocument.Load(dataDirectory + prefix + filename + ".xml");
            document.Validate(getXmlSchemaSet(filename + ".xsd"), new ValidationEventHandler(xmlValidationEventHandler));

            return document;
        }
    }
}
