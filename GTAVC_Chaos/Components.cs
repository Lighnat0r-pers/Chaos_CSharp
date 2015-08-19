using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace GTAVC_Chaos
{
    class Components
    {
        public MemoryAddress[] memoryAddresses;
        public TimedEffect[] timedEffects;
        public PermanentEffect[] permanentEffects;
        public StaticEffect[] staticEffects;

        string baseResourceString     = "GTAVC_Chaos.";
        string memoryAddressesFile    = "MemoryAddresses.xml";
        string memoryAddressesSchema  = "MemoryAddressSchema.xsd";
        string timedEffectsFile       = "TimedEffects.xml";
        string timedEffectsSchema     = "TimedEffectSchema.xsd";
        string permanentEffectsFile   = "PermanentEffects.xml";
        string permanentEffectsSchema = "PermanentEffectSchema.xsd";
        string staticEffectsFile      = "StaticEffects.xml";
        string staticEffectsSchema    = "StaticEffectSchema.xsd";

        /// <summary>
        /// Constructor
        /// </summary>
        public Components()
        {
            InitMemoryAddresses();
            InitTimedEffects();
            InitPermanentEffects();
            InitStaticEffects();
        }

        void xmlValidationEventHandler(object sender, ValidationEventArgs e)
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

        XmlSchemaSet getXmlSchemaSet(string filename)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            // NOTE(Ligh): Gets the schema from an external file.
            //XmlReader x = new XmlTextReader(filename);

            // NOTE(Ligh): Gets the schema from an embedded resource.
            Stream x = Assembly.GetExecutingAssembly().GetManifestResourceStream(baseResourceString + filename);

            schemas.Add(XmlSchema.Read(x, null));

            return schemas;
        }

        XmlDocument getXmlDocument(string xmlFilename, string xsdFilename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFilename);
            document.Schemas = getXmlSchemaSet(xsdFilename);
            document.Validate(new ValidationEventHandler(xmlValidationEventHandler));

            return document;
        }

        void InitMemoryAddresses()
        {
            Debug.WriteLine("Initializing memory addresses from file.");
            XmlDocument file = getXmlDocument(memoryAddressesFile, memoryAddressesSchema);
            ReadMemoryAddresses(file);
        }

        void ReadMemoryAddresses(XmlDocument file)
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

            Debug.WriteLine("Read " + count + " memory addresses from file.");
        }

        public MemoryAddress findMemoryAddressByName(string name)
        {
            MemoryAddress result = null;
            foreach (MemoryAddress address in memoryAddresses)
            {
                if (address.name == name)
                {
                    result = address;
                    break;
                }
            }

            if (result == null)
            {
                throw new Exception("Memory address " + name + " not found.");
            }

            return result;
        }

        void InitTimedEffects()
        {
            Debug.WriteLine("Initializing timed effects from file.");
            XmlDocument file = getXmlDocument(timedEffectsFile, timedEffectsSchema);
            ReadTimedEffects(file);
        }

        void ReadTimedEffects(XmlDocument file)
        {
            XmlNodeList nodes = file.SelectNodes("//timedeffects/timedeffect");
            timedEffects = new TimedEffect[nodes.Count];

            int count1 = 0;
            foreach (XmlNode node in nodes)
            {
                string name = node.SelectSingleNode("name").InnerText;
                string category = node.SelectSingleNode("category").InnerText;
                int difficulty = Int32.Parse(node.SelectSingleNode("difficulty").InnerText);

                int duration = 0;
                XmlNode durationNode = node.SelectSingleNode("duration");
                if (durationNode != null)
                {
                    duration = Int32.Parse(durationNode.InnerText);
                }

                XmlNodeList activatornodes = node.SelectNodes("activator");
                EffectActivator[] activators = new EffectActivator[activatornodes.Count];

                int count2 = 0;
                foreach (XmlNode activatornode in activatornodes)
                {
                    string type = activatornode.SelectSingleNode("type").InnerText;
                    string target = activatornode.SelectSingleNode("target").InnerText;
                    string address = activatornode.SelectSingleNode("address").InnerText;

                    activators[count2] = new EffectActivator(type, target, address);

                    count2++;
                }

                timedEffects[count1] = new TimedEffect(name, category, difficulty, duration);

                count1++;
            }

            Debug.WriteLine("Read " + count1 + " timed effects from file.");
        }

        void InitPermanentEffects()
        {
            Debug.WriteLine("Initializing permanent effects from file.");
            XmlDocument file = getXmlDocument(permanentEffectsFile, permanentEffectsSchema);
            ReadPermanentEffects(file);
        }

        void ReadPermanentEffects(XmlDocument file)
        {

        }

        void InitStaticEffects()
        {
            Debug.WriteLine("Initializing static effects from file.");
            XmlDocument file = getXmlDocument(staticEffectsFile, staticEffectsSchema);
            ReadStaticEffects(file);
        }

        void ReadStaticEffects(XmlDocument file)
        {

        }
    }
}
