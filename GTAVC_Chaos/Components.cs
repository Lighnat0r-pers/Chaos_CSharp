using System;
using System.Xml;
using System.Xml.Schema;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace GTAVC_Chaos
{
    static class Components
    {
        static public MemoryAddress[] memoryAddresses;
        static public TimedEffect[] timedEffects;
        static public PermanentEffect[] permanentEffects;
        static public StaticEffect[] staticEffects;
        static XmlDocument timedEffectsFile;

        static public void Init()
        {

            InitTimedEffects();
            InitPermanentEffects();
            InitStaticEffects();

        }

        static void settingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Debug.Write("WARNING: ");
                Debug.WriteLine(e.Message);
            }
            else
            {
                Debug.Write("ERROR: ");
                Debug.WriteLine(e.Message);
            }
        }

        static void InitTimedEffects()
        {
           // /*
            // NOTE(Ligh): Get the schema from an external file.
            XmlSchemaCollection schemas = new XmlSchemaCollection();
            XmlReader schemaReader = new XmlTextReader("TimedEffectSchema.xsd");
            XmlSchema schema = XmlSchema.Read(schemaReader, null);
            schemas.Add(schema);
            // */




            XmlReader readerDoc = new XmlTextReader("TimedEffects.xml");
            XmlValidatingReader newReader = new XmlValidatingReader(readerDoc);
            newReader.Schemas.Add(schemas);
            //newReader.ValidationEventHandler += new ValidationEventHandler(OnValidate);

            while (newReader.Read())
            {
                switch (newReader.NodeType)
                {
                    case XmlNodeType.Element:
                        Debug.WriteLine("<" + newReader.Name + ">");
                        break;
                    case XmlNodeType.Text:
                        Debug.WriteLine(newReader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        Debug.WriteLine("</" + newReader.Name + ">");
                        break;
                }
            }
            newReader.Close();
            /*

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, schemaReader);
            timedEffectsFile = new XmlDocument();
            timedEffectsFile.Schemas = schemas;
            timedEffectsFile.LoadXml("TimedEffects.xml");
            timedEffectsFile.Validate(new ValidationEventHandler(settingsValidationEventHandler));
            ReadTimedEffects();
            */

        }

        static void ReadTimedEffects()
        {
            XmlNodeList effects = timedEffectsFile.SelectNodes("//timedeffects/timedeffect");
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

        }

        static void InitStaticEffects()
        {

        }
    }
}
