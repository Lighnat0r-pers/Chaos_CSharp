using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

namespace GTAVC_Chaos
{
    static class DataFileHandler
    {
        static private string gamesFilename = "Games";
        static private string memoryAddressesFilename = "MemoryAddresses";
        static private string limitationsFilename = "Limitations";
        static private string timedEffectsFilename = "TimedEffects";
        static private string permanentEffectsFilename = "PermanentEffects";
        static private string staticEffectsFilename = "StaticEffects";

        static public void ReadFilesForGame(Game game)
        {
            InitMemoryAddresses(game);
            InitLimitations(game);
            InitTimedEffects(game);
            //InitPermanentEffects(game);
            //InitStaticEffects(game);
        }

        static public Game[] InitGamesFromFile()
        {
            Debug.WriteLine("Reading games from file.");
            XmlDocument file = XmlUtils.getXmlDocument("", gamesFilename);
            return ReadGames(file);
        }

        static private Game[] ReadGames(XmlDocument file)
        {
            bool baseVersionDefined;

            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//games/game");
            Game[] games = new Game[nodes.Count];

            int count = 0;
            foreach (XmlNode node in nodes)
            {
                baseVersionDefined = false;

                // TODO(Ligh): Properly catch errors here.

                string name = node.SelectSingleNode("name").InnerText;
                string abbreviation = node.SelectSingleNode("abbreviation").InnerText;
                string windowName = node.SelectSingleNode("windowname").InnerText;
                string windowClass = node.SelectSingleNode("windowclass").InnerText;
                long versionAddress = Int64.Parse(node.SelectSingleNode("versionaddress").InnerText, NumberStyles.HexNumber);
                string baseVersion = node.SelectSingleNode("baseversion").InnerText;

                Debug.WriteLine("Reading game information for " + name);

                XmlNodeList versionNodes = node.SelectNodes("versions/version");

                GameVersion[] versions = new GameVersion[versionNodes.Count];

                int count2 = 0;
                foreach (XmlNode versionNode in versionNodes)
                {
                    string versionName = versionNode.SelectSingleNode("name").InnerText;
                    int addressValue = Int32.Parse(versionNode.SelectSingleNode("addressvalue").InnerText, NumberStyles.HexNumber);

                    SortedList<long, int> offsets = new SortedList<long, int>();

                    if (versionName == baseVersion)
                    {
                        baseVersionDefined = true;

                        offsets.Add(0, 0); // Dummy offset

                        Debug.WriteLine("Read default version (" + versionName + ") from file for " + name + ".");
                    }
                    else
                    {
                        XmlNodeList offsetNodes = versionNode.SelectNodes("offsets/offset");

                        foreach (XmlNode offsetNode in offsetNodes)
                        {
                            long startAddress = Int64.Parse(offsetNode.SelectSingleNode("startaddress").InnerText, NumberStyles.HexNumber);

                            XmlNode amountNode = offsetNode.SelectSingleNode("amount");
                            int offsetAmount = Int32.Parse(amountNode.InnerText, NumberStyles.HexNumber);

                            if (amountNode.Attributes["negative"] != null && amountNode.Attributes["negative"].Value == "true")
                            {
                                offsetAmount = -offsetAmount;
                            }

                            offsets.Add(startAddress, offsetAmount);
                        }

                        Debug.WriteLine("Read " + offsetNodes.Count + " offsets from file for version " + versionName + " of " + name + ".");

                        if (offsetNodes.Count == 0)
                        {
                            throw new Exception("No offsets defined in the list of versions  for version " + versionName + " of " + name + ".");
                        }
                    }

                    versions[count2++] = new GameVersion(versionName, addressValue, offsets);
                }

                Debug.WriteLine("Read " + count2 + " versions from file for " + name + ".");

                if (!baseVersionDefined)
                {
                    throw new Exception("Base version not defined in the list of versions  for " + name + ".");
                }

                games[count++] = new Game(name, abbreviation, windowName, windowClass, versionAddress, baseVersion, versions);
            }

            Debug.WriteLine("Read " + count + " games from file.");

            return games;
        }

        static private void InitMemoryAddresses(Game game)
        {
            Debug.WriteLine("Reading memory addresses from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, memoryAddressesFilename);
            game.SetMemoryAddresses(ReadMemoryAddresses(file), ReadMemoryAddressesGameVersion(file));
        }

        static private string ReadMemoryAddressesGameVersion(XmlDocument file)
        {
            // TODO(Ligh): Properly catch errors here.
            return file.SelectSingleNode("//addresses").Attributes["gameversion"].Value;
        }

        static private MemoryAddress[] ReadMemoryAddresses(XmlDocument file)
        {
            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//addresses/memoryaddress");
            MemoryAddress[] memoryAddresses = new MemoryAddress[nodes.Count];

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
                }

                memoryAddresses[count++] = addressObj;
            }

            Debug.WriteLine("Read " + count + " memory addresses from file.");

            return memoryAddresses;
        }

        static private void InitLimitations(Game game)
        {
            Debug.WriteLine("Reading limitations from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, limitationsFilename);
            game.SetLimitations(ReadLimitations(file, game));
        }

        static private Limitation[] ReadLimitations(XmlDocument file, Game game)
        {
            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//limitations/limitation");
            Limitation[] limitations = new Limitation[nodes.Count];

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
                            address = game.FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                            string value = checkNode.SelectSingleNode("value").InnerText;
                            check = new SimpleCheck(address, value);
                            break;
                        case "parameter":
                            string defaultValue = null;
                            address = game.FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
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
                            break;
                        case "comparison":
                            XmlNodeList addressNodes = checkNode.SelectNodes("addresses/address");
                            MemoryAddress[] addresses = new MemoryAddress[addressNodes.Count];
                            int count3 = 0;
                            foreach (XmlNode addressNode in addressNodes)
                            {
                                addresses[count3++] = game.FindMemoryAddressByName(addressNode.InnerText);
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

            return limitations;
        }

        static private void InitTimedEffects(Game game)
        {
            Debug.WriteLine("Reading timed effects from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, timedEffectsFilename);
            game.timedEffects = ReadTimedEffects(file, game);
        }

        static private TimedEffect[] ReadTimedEffects(XmlDocument file, Game game)
        {
            // TODO(Ligh): Properly catch errors here.

            XmlNodeList nodes = file.SelectNodes("//timedeffects/timedeffect");
            TimedEffect[] timedEffects = new TimedEffect[nodes.Count];

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
                    MemoryAddress address = game.FindMemoryAddressByName(activatorNode.SelectSingleNode("address").InnerText);

                    activators[count2++] = new EffectActivator(type, target, address);
                }

                XmlNodeList limitationNodes = node.SelectNodes("limitations/limitation");
                Limitation[] effectLimitations = new Limitation[limitationNodes.Count];

                int count3 = 0;
                foreach (XmlNode limitationNode in limitationNodes)
                {
                    string limitationName = limitationNode.SelectSingleNode("name").InnerText;
                    Limitation limitation = game.FindLimitationByName(limitationName);

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

                timedEffects[count1++] = new TimedEffect(name, category, difficulty, activators, duration, effectLimitations);
            }

            Debug.WriteLine("Read " + count1 + " timed effects from file.");

            return timedEffects;
        }

        static private void InitPermanentEffects(Game game)
        {
            Debug.WriteLine("Reading permanent effects from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, permanentEffectsFilename);
            game.permanentEffects = ReadPermanentEffects(file);
        }

        static private PermanentEffect[] ReadPermanentEffects(XmlDocument file)
        {
            return new PermanentEffect[0];
        }

        static private void InitStaticEffects(Game game)
        {
            Debug.WriteLine("Reading static effects from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, staticEffectsFilename);
            game.staticEffects = ReadStaticEffects(file);
        }

        static private StaticEffect[] ReadStaticEffects(XmlDocument file)
        {
            return new StaticEffect[0];
        }
    }
}
