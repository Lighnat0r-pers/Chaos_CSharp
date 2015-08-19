using System;
using System.Xml;
using System.Xml.Schema;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace GTAVC_Chaos
{
    static class Components
    {
        static public MemoryAddress[] memoryAddresses;
        static public TimedEffect[] timedEffects;
        static public PermanentEffect[] permanentEffects;
        static public StaticEffect[] staticEffects;

        static string baseResourceString     = "GTAVC_Chaos.";
        static string memoryAddressesFile    = "MemoryAddresses.xml";
        static string memoryAddressesSchema  = "MemoryAddressSchema.xsd";
        static string timedEffectsFile       = "TimedEffects.xml";
        static string timedEffectsSchema     = "TimedEffectSchema.xsd";
        static string permanentEffectsFile   = "PermanentEffects.xml";
        static string permanentEffectsSchema = "PermanentEffectSchema.xsd";
        static string staticEffectsFile      = "StaticEffects.xml";
        static string staticEffectsSchema    = "StaticEffectSchema.xsd";


        static public void Init()
        {
            InitMemoryAddresses();
            InitTimedEffects();
            InitPermanentEffects();
            InitStaticEffects();
        }

        static void xmlValidationEventHandler(object sender, ValidationEventArgs e)
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

        static XmlSchemaSet getXmlSchemaSet(string filename)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            // NOTE(Ligh): Gets the schema from an external file.
            //XmlReader x = new XmlTextReader(filename);

            // NOTE(Ligh): Gets the schema from an embedded resource.
            Stream x = Assembly.GetExecutingAssembly().GetManifestResourceStream(baseResourceString + filename);

            schemas.Add(XmlSchema.Read(x, null));

            return schemas;
        }

        static XmlDocument getXmlDocument(string xmlFilename, string xsdFilename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFilename);
            document.Schemas = getXmlSchemaSet(xsdFilename);
            document.Validate(new ValidationEventHandler(xmlValidationEventHandler));

            return document;
        }

        static void InitMemoryAddresses()
        {
            XmlDocument file = getXmlDocument(memoryAddressesFile, memoryAddressesSchema);
            ReadMemoryAddresses(file);
        }

        static void ReadMemoryAddresses(XmlDocument file)
        {
            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//addresses/memoryaddress");
            memoryAddresses = new MemoryAddress[nodes.Count];

            int count = 0;
            foreach (XmlNode node in nodes)
            {
                string name = node.SelectSingleNode("name").InnerText;
                long address = Int64.Parse(node.SelectSingleNode("address").InnerText, NumberStyles.HexNumber);
                string type = node.SelectSingleNode("type").InnerText;

                int size = 0;
                XmlNode sizeNode = node.SelectSingleNode("size");
                if (sizeNode != null)
                {
                    size = Int32.Parse(sizeNode.InnerText);
                }

                memoryAddresses[count] = new MemoryAddress(name, address, type, size);

                count++;
            }
        }

        static void InitTimedEffects()
        {
            XmlDocument file = getXmlDocument(timedEffectsFile, timedEffectsSchema);
            ReadTimedEffects(file);
        }

        static void ReadTimedEffects(XmlDocument file)
        {
            XmlNodeList effects = file.SelectNodes("//timedeffects/timedeffect");
            foreach (XmlNode effect in effects)
            {
                XmlNode nameNode = effect.SelectSingleNode("name");
                XmlNode categoryNode = effect.SelectSingleNode("category");
                XmlNode difficultyNode = effect.SelectSingleNode("difficulty");
                XmlNode durationNode = effect.SelectSingleNode("duration");
                if (durationNode == null)
                {

                }

                XmlNodeList activators = effect.SelectNodes("activator");
                foreach (XmlNode activator in activators)
                {
                    XmlNode typeNode = activator.SelectSingleNode("type");
                    XmlNode datatypeNode = activator.SelectSingleNode("datatype");
                    XmlNode addressNode = activator.SelectSingleNode("address");
                    XmlNode targetNode = activator.SelectSingleNode("target");
                    XmlNode originalNode = activator.SelectSingleNode("original");
                }

            }
        }

        static void InitPermanentEffects()
        {
            XmlDocument file = getXmlDocument(permanentEffectsFile, permanentEffectsSchema);
            ReadPermanentEffects(file);
        }

        static void ReadPermanentEffects(XmlDocument file)
        {

        }

        static void InitStaticEffects()
        {
            XmlDocument file = getXmlDocument(staticEffectsFile, staticEffectsSchema);
            ReadStaticEffects(file);
        }

        static void ReadStaticEffects(XmlDocument file)
        {

        }
    }
}
