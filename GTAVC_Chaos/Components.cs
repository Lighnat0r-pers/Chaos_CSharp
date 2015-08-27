using System;
using System.Collections.Generic;
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
        public Limitation[] limitations;
        public TimedEffect[] timedEffects;
        public PermanentEffect[] permanentEffects;
        public StaticEffect[] staticEffects;

        private List<MemoryAddress> memoryAddressesToResolve = new List<MemoryAddress>();
        private List<LimitationCheck> limitationChecksToResolve = new List<LimitationCheck>();

        private string baseResourceString = "GTAVC_Chaos.";
        private string xmlFileExtension = ".xml";
        private string xmlSchemaFileExtension = ".xsd";
        private string memoryAddressesFilename = "MemoryAddresses";
        private string limitationsFilename = "Limitations";
        private string timedEffectsFilename = "TimedEffects";
        private string permanentEffectsFilename = "PermanentEffects";
        private string staticEffectsFilename = "StaticEffects";

        public void Init()
        {
            InitMemoryAddresses();
            InitLimitations();
            InitTimedEffects();
            //InitPermanentEffects();
            //InitStaticEffects();
        }

        private void xmlValidationEventHandler(object sender, ValidationEventArgs e)
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

        private XmlSchemaSet getXmlSchemaSet(string filename)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            // NOTE(Ligh): Gets the schema from an external file.
            //XmlReader x = new XmlTextReader(filename);

            // NOTE(Ligh): Gets the schema from an embedded resource.
            Stream x = Assembly.GetExecutingAssembly().GetManifestResourceStream(baseResourceString + filename);

            schemas.Add(XmlSchema.Read(x, null));

            return schemas;
        }

        private XmlDocument getXmlDocument(string xmlFilename, string xsdFilename)
        {
            // TODO(Ligh): Deal with errors (file not found etc) here.

            XmlDocument document = new XmlDocument();
            document.Load(xmlFilename);
            document.Schemas = getXmlSchemaSet(xsdFilename);
            document.Validate(new ValidationEventHandler(xmlValidationEventHandler));

            return document;
        }

        private void InitMemoryAddresses()
        {
            Debug.WriteLine("Initializing memory addresses from file.");
            XmlDocument file = getXmlDocument(memoryAddressesFilename + xmlFileExtension, memoryAddressesFilename + xmlSchemaFileExtension);
            ReadMemoryAddresses(file);
        }

        private void ReadMemoryAddresses(XmlDocument file)
        {
            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//addresses/memoryaddress");
            memoryAddresses = new MemoryAddress[nodes.Count];

            // TODO(Ligh): Make use of the game version gotten here so that the GameHandler knows about it.
            string gameVersion = file.SelectSingleNode("//addresses").Attributes["gameversion"].Value;

            int count = 0;
            foreach (XmlNode node in nodes)
            {
                MemoryAddress addressObj;

                string name = node.SelectSingleNode("name").InnerText;
                string type = node.SelectSingleNode("type").InnerText;
                int size = 0;
                XmlNode sizeNode = node.SelectSingleNode("length");
                if (sizeNode != null)
                {
                    size = Int32.Parse(sizeNode.InnerText);
                }

                XmlNode addressNode = node.SelectSingleNode("address");

                if (addressNode.Attributes["xsi:type"].Value == "static")
                {
                    long address = Int64.Parse(addressNode.InnerText, NumberStyles.HexNumber);
                    addressObj = new MemoryAddress(name, address, type, size);
                }
                else
                {
                    string baseAddressName = addressNode.SelectSingleNode("baseaddress").InnerText;
                    long offset = Int64.Parse(addressNode.SelectSingleNode("offset").InnerText, NumberStyles.HexNumber);
                    addressObj = new MemoryAddress(name, baseAddressName, offset, type, size);
                    memoryAddressesToResolve.Add(addressObj);
                }

                memoryAddresses[count] = addressObj;

                count++;
            }

            Debug.WriteLine("Read " + count + " memory addresses from file.");

            ResolveDynamicMemoryAddressBases();
        }

        private void ResolveDynamicMemoryAddressBases()
        {
            foreach (MemoryAddress addressObj in memoryAddressesToResolve)
            {
                addressObj.ResolveBaseAddress();
            }
        }

        public MemoryAddress FindMemoryAddressByName(string name)
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

        private void InitLimitations()
        {
            Debug.WriteLine("Initializing limitations from file.");
            XmlDocument file = getXmlDocument(limitationsFilename + xmlFileExtension, limitationsFilename + xmlSchemaFileExtension);
            ReadLimitations(file);
        }

        private void ReadLimitations(XmlDocument file)
        {
            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//limitations/limitation");
            limitations = new Limitation[nodes.Count];

            int count = 0;
            foreach (XmlNode node in nodes)
            {
                ICheck check;
                string name = node.SelectSingleNode("name").InnerText;

                XmlNodeList checkNodes = node.SelectNodes("checks/check");

                ICheck[] checks = new ICheck[checkNodes.Count];

                int count2 = 0;
                foreach (XmlNode checkNode in checkNodes)
                {
                    MemoryAddress address;
                    switch (checkNode.Attributes["xsi:type"].Value)
                    {
                        case "simple":
                            address = FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                            string value = checkNode.SelectSingleNode("value").InnerText;
                            check = new SimpleCheck(address, value);
                            break;
                        case "parameter":
                            string defaultValue = null;
                            address = FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                            if (checkNode.SelectSingleNode("default") != null)
                            {
                                defaultValue = checkNode.SelectSingleNode("default").InnerText;
                            }

                            check = new ParameterCheck(address, defaultValue);
                            break;
                        case "limitation":
                            string limitation = checkNode.SelectSingleNode("limitation").InnerText;
                            bool target = Boolean.Parse(checkNode.SelectSingleNode("target").InnerText);
                            Dictionary<string, dynamic> parameters = null;

                            XmlNodeList parameterNodes = checkNode.SelectNodes("parameters/parameter");
                            if (parameterNodes.Count != 0)
                            {
                                parameters = new Dictionary<string, object>();

                                foreach (XmlNode parameterNode in parameterNodes)
                                {
                                    string parameterName = parameterNode.SelectSingleNode("name").InnerText;
                                    string parameterValue = parameterNode.SelectSingleNode("value").InnerText;
                                    parameters.Add(parameterName, parameterValue);
                                }
                            }

                            check = new LimitationCheck(limitation, target, parameters);
                            limitationChecksToResolve.Add((LimitationCheck)check);
                            break;
                        case "comparison":
                            XmlNodeList addressNodes = checkNode.SelectNodes("addresses/address");
                            MemoryAddress[] addresses = new MemoryAddress[addressNodes.Count];
                            int count3 = 0;
                            foreach (XmlNode addressNode in addressNodes)
                            {
                                addresses[count3++] = FindMemoryAddressByName(addressNode.InnerText);
                            }
                            bool equal = Boolean.Parse(checkNode.SelectSingleNode("equal").InnerText);
                            check = new ComparisonCheck(addresses, equal);
                            break;
                        default:
                            throw new Exception("Tried to process unknown limitation check type" + checkNode.Attributes["xsi:type"].Value);
                    }

                    checks[count2++] = check;

                }

                limitations[count++] = new Limitation(name, checks);

            }

            Debug.WriteLine("Read " + count + " limitations from file.");

            ResolveLimitationChecks();
        }

        private void ResolveLimitationChecks()
        {
            foreach (LimitationCheck limitationCheck in limitationChecksToResolve)
            {
                limitationCheck.ResolveLimitation();
            }
        }

        public Limitation FindLimitationByName(string name)
        {
            Limitation result = null;
            foreach (Limitation limitation in limitations)
            {
                if (limitation.name == name)
                {
                    result = limitation.Clone();
                    break;
                }
            }

            if (result == null)
            {
                throw new Exception("Limitation " + name + " not found.");
            }

            return result;
        }

        private void InitTimedEffects()
        {
            Debug.WriteLine("Initializing timed effects from file.");
            XmlDocument file = getXmlDocument(timedEffectsFilename + xmlFileExtension, timedEffectsFilename + xmlSchemaFileExtension);
            ReadTimedEffects(file);
        }

        private void ReadTimedEffects(XmlDocument file)
        {
            // TODO(Ligh): Properly catch errors here.

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

                XmlNodeList activatorNodes = node.SelectNodes("activators/activator");
                EffectActivator[] activators = new EffectActivator[activatorNodes.Count];

                int count2 = 0;
                foreach (XmlNode activatorNode in activatorNodes)
                {
                    string type = activatorNode.Attributes["type"].Value;
                    string target = activatorNode.SelectSingleNode("target").InnerText;
                    string address = activatorNode.SelectSingleNode("address").InnerText;

                    activators[count2++] = new EffectActivator(type, target, address);
                }

                XmlNodeList limitationNodes = node.SelectNodes("limitations/limitation");
                Limitation[] effectLimitations = new Limitation[limitationNodes.Count];

                int count3 = 0;
                foreach (XmlNode limitationNode in limitationNodes)
                {
                    string limitationName = limitationNode.SelectSingleNode("name").InnerText;
                    Limitation limitation = FindLimitationByName(limitationName);

                    limitation.setTarget(Boolean.Parse(limitationNode.SelectSingleNode("target").InnerText));

                    XmlNodeList parameterNodes = limitationNode.SelectNodes("parameters/parameter");
                    if (parameterNodes.Count != 0)
                    {
                        Dictionary<string, dynamic> parameters = new Dictionary<string, object>();

                        foreach (XmlNode parameterNode in parameterNodes)
                        {
                            string parameterName = parameterNode.SelectSingleNode("name").InnerText;
                            string parameterValue = parameterNode.SelectSingleNode("value").InnerText;
                            parameters.Add(parameterName, parameterValue);
                        }

                        limitation.setParameters(parameters);
                    }

                    effectLimitations[count3++] = limitation;
                }

                timedEffects[count1++] = new TimedEffect(name, category, difficulty, duration, effectLimitations);
            }

            Debug.WriteLine("Read " + count1 + " timed effects from file.");
        }

        private void InitPermanentEffects()
        {
            Debug.WriteLine("Initializing permanent effects from file.");
            XmlDocument file = getXmlDocument(permanentEffectsFilename + xmlFileExtension, permanentEffectsFilename + xmlSchemaFileExtension);
            ReadPermanentEffects(file);
        }

        private void ReadPermanentEffects(XmlDocument file)
        {

        }

        private void InitStaticEffects()
        {
            Debug.WriteLine("Initializing static effects from file.");
            XmlDocument file = getXmlDocument(staticEffectsFilename + xmlFileExtension, staticEffectsFilename + xmlSchemaFileExtension);
            ReadStaticEffects(file);
        }

        private void ReadStaticEffects(XmlDocument file)
        {

        }
    }
}
