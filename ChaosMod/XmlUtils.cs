using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ChaosMod
{
    static class XmlUtils
    {
        static public XNamespace xsiNamespace => "http://www.w3.org/2001/XMLSchema-instance";

        static private string baseResourceString => "ChaosMod.Resources.";
        static private string dataDirectory => "Data\\";

        static private void xmlValidationEventHandler(object sender, ValidationEventArgs e)
        {
            // TODO(Ligh): Do more with the error here.

            Debug.WriteLine($"ERROR: {e.Message}");

            throw new System.Exception($"Xml validation error: {e.Message}");
        }

        static private XmlSchemaSet getXmlSchemaSet(string filename)
        {
            var x = Assembly.GetExecutingAssembly().GetManifestResourceStream(baseResourceString + filename);
            var schemas = new XmlSchemaSet();
            schemas.Add(XmlSchema.Read(x, null));

            return schemas;
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
