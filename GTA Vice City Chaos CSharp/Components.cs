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
        static public TimedEffect[] timedEffectsArray;
        static public PermanentEffect[] permanentEffectsArray;
        static public StaticEffect[] staticEffectsArray;
        static XmlDocument xmlFile;

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
            Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GTAVC_Chaos.TimedEffectSchema.xsd");
            XmlReader schemaReader = XmlReader.Create(schemaStream);

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, schemaReader);
            xmlFile = new XmlDocument();
            xmlFile.Schemas = schemas;
            xmlFile.LoadXml("TimeEffects.xml");
            xmlFile.Validate(new ValidationEventHandler(settingsValidationEventHandler));
            ReadTimedEffects();


        }

        static void ReadTimedEffects()
        {
            XmlNodeList effects = xmlFile.SelectNodes("//timedeffects/timedeffect");
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
