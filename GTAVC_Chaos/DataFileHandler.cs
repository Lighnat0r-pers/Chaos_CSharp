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
            Debug.WriteLine("Starting to read files for game.");
            game.memoryAddresses = GetMemoryAddressesFromFile(game);
            game.SetLimitations(GetLimitationsFromFile(game));
            Debug.WriteLine("Done reading files for game.");
        }

        static public List<Game> ReadGames()
        {
            // TODO(Ligh): Properly catch errors here.

            Debug.WriteLine("Reading games from file.");
            var file = XmlUtils.getXmlDocument("", gamesFilename);

            var games = new List<Game>();

            foreach (XmlNode node in file.SelectNodes("//games/game"))
            {

                string name = node.SelectSingleNode("name").InnerText;
                string abbreviation = node.SelectSingleNode("abbreviation").InnerText;
                string windowName = node.SelectSingleNode("windowname").InnerText;
                string windowClass = node.SelectSingleNode("windowclass").InnerText;
                long versionAddress = Int64.Parse(node.SelectSingleNode("versionaddress").InnerText, NumberStyles.HexNumber);
                string baseVersion = node.SelectSingleNode("baseversion").InnerText;

                Debug.WriteLine("Reading game information for {0}", name);

                var versions = new List<GameVersion>();
                bool baseVersionDefined = false;

                foreach (XmlNode versionNode in node.SelectNodes("versions/version"))
                {
                    string versionName = versionNode.SelectSingleNode("name").InnerText;
                    int addressValue = Int32.Parse(versionNode.SelectSingleNode("addressvalue").InnerText, NumberStyles.HexNumber);

                    var offsets = new SortedList<long, int>();

                    if (versionName == baseVersion)
                    {
                        baseVersionDefined = true;
                        offsets.Add(0, 0); // Dummy offset

                        Debug.WriteLine("Read default version ({0}) from file for {1}.", versionName, name);
                    }
                    else
                    {
                        foreach (XmlNode offsetNode in versionNode.SelectNodes("offsets/offset"))
                        {
                            long startAddress = Int64.Parse(offsetNode.SelectSingleNode("startaddress").InnerText, NumberStyles.HexNumber);

                            var amountNode = offsetNode.SelectSingleNode("amount");
                            int offsetAmount = Int32.Parse(amountNode.InnerText, NumberStyles.HexNumber);

                            if (amountNode.Attributes["negative"] != null && amountNode.Attributes["negative"].Value == "true")
                            {
                                offsetAmount *= -1;
                            }

                            offsets.Add(startAddress, offsetAmount);
                        }

                        Debug.WriteLine("Read {0} offsets from file for version {1} of {2}.", offsets.Count, versionName, name);

                        if (offsets.Count == 0)
                        {
                            throw new Exception("No offsets defined in the list of versions for version " + versionName + " of " + name + ".");
                        }
                    }

                    versions.Add(new GameVersion(versionName, addressValue, offsets));
                }

                Debug.WriteLine("Read {0} versions from file for {1}.", versions.Count, name);

                if (!baseVersionDefined)
                {
                    throw new Exception("Base version not defined in the list of versions  for " + name + ".");
                }

                games.Add(new Game(name, abbreviation, windowName, windowClass, versionAddress, baseVersion, versions));
            }

            Debug.WriteLine("Read {0} games from file.", games.Count);

            return games;
        }

        static private List<MemoryAddress> GetMemoryAddressesFromFile(Game game)
        {
            // TODO(Ligh): Properly catch errors here.

            // TODO(Ligh): Handle not found cases for FindMemoryAddressByName.

            Debug.WriteLine("Reading memory addresses from file.");
            var file = XmlUtils.getXmlDocument(game.abbreviation, memoryAddressesFilename);

            var gameVersion = game.gameVersions.Find(v => v.name == file.SelectSingleNode("//addresses").Attributes["gameversion"].Value);

            if (gameVersion == null)
            {
                throw new Exception("Unable to read memory addresses: No known game version set.");
            }

            var memoryAddresses = new List<MemoryAddress>();

            foreach (XmlNode node in file.SelectNodes("//addresses/memoryaddress"))
            {
                MemoryAddress addressObj;

                string name = node.SelectSingleNode("name").InnerText;
                string datatype = node.SelectSingleNode("datatype").InnerText;

                var sizeNode = node.SelectSingleNode("length");
                int size = sizeNode != null ? Int32.Parse(sizeNode.InnerText) : 0;

                var addressNode = node.SelectSingleNode("address");

                if (addressNode.Attributes["xsi:type"].Value == "static")
                {
                    long address = Int64.Parse(addressNode.InnerText, NumberStyles.HexNumber);
                    addressObj = new MemoryAddress(game, name, address, gameVersion, datatype, size);
                }
                else
                {
                    string baseAddressName = addressNode.SelectSingleNode("baseaddress").InnerText;
                    long offset = Int64.Parse(addressNode.SelectSingleNode("offset").InnerText, NumberStyles.HexNumber);
                    addressObj = new MemoryAddress(game, name, baseAddressName, offset, datatype, size);
                }

                memoryAddresses.Add(addressObj);
            }

            // Set base address for all dynamic addresses. Note that this cannot be done before all addresses have been read.
            foreach (var address in memoryAddresses.FindAll(m => m.IsDynamic == true))
            {
                address.baseAddress = memoryAddresses.Find(m => m.name == address.baseAddressName);
            }

            Debug.WriteLine("Read {0} memory addresses from file.", memoryAddresses.Count);

            return memoryAddresses;
        }

        static private List<Limitation> GetLimitationsFromFile(Game game)
        {
            // TODO(Ligh): Properly catch errors here.

            // TODO(Ligh): Read base limitations here and implement them elsewhere.

            // TODO(Ligh): Handle not found cases for FindMemoryAddressByName.

            Debug.WriteLine("Reading limitations from file.");
            var file = XmlUtils.getXmlDocument(game.abbreviation, limitationsFilename);

            var limitations = new List<Limitation>();

            foreach (XmlNode node in file.SelectNodes("//limitations/limitation"))
            {
                ICheck check;
                string name = node.SelectSingleNode("name").InnerText;

                var checks = new List<ICheck>();

                foreach (XmlNode checkNode in node.SelectNodes("checks/check"))
                {
                    MemoryAddress address;
                    switch (checkNode.Attributes["xsi:type"].Value)
                    {
                        case "simple":
                            address = game.FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                            dynamic value = address.ConvertToRightDataType(checkNode.SelectSingleNode("value").InnerText);
                            check = new SimpleCheck(address, value);
                            break;
                        case "parameter":
                            address = game.FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                            var defaultValueNode = checkNode.SelectSingleNode("default");
                            dynamic defaultValue = defaultValueNode != null ? address.ConvertToRightDataType(defaultValueNode.InnerText) : null;

                            check = new ParameterCheck(address, defaultValue);
                            break;
                        case "limitation":
                            string limitation = checkNode.SelectSingleNode("limitation").InnerText;
                            bool target = Boolean.Parse(checkNode.SelectSingleNode("target").InnerText);
                            Dictionary<string, string> parameters = null;

                            var parameterNodes = checkNode.SelectNodes("parameters/parameter");
                            if (parameterNodes.Count > 0)
                            {
                                parameters = new Dictionary<string, string>();

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
                            var addresses = new List<MemoryAddress>();
                            foreach (XmlNode addressNode in checkNode.SelectNodes("addresses/address"))
                            {
                                addresses.Add(game.FindMemoryAddressByName(addressNode.InnerText));
                            }
                            bool equal = Boolean.Parse(checkNode.SelectSingleNode("equal").InnerText);
                            check = new ComparisonCheck(addresses, equal);
                            break;
                        default:
                            throw new Exception("Tried to process unknown limitation check type" + checkNode.Attributes["xsi:type"].Value);
                    }

                    checks.Add(check);

                }

                limitations.Add(new Limitation(name, checks));
            }

            Debug.WriteLine("Read {0} limitations from file.", limitations.Count);

            return limitations;
        }

        static public List<TimedEffect> GetTimedEffectsFromFile(Game game)
        {
            // TODO(Ligh): Properly catch errors here.

            // TODO(Ligh): Handle not found cases for FindMemoryAddressByName.

            // TODO(Ligh): Handle not found cases for FindLimitationByName.

            Debug.WriteLine("Reading timed effects from file.");
            var file = XmlUtils.getXmlDocument(game.abbreviation, timedEffectsFilename);

            var timedEffects = new List<TimedEffect>();

            foreach (XmlNode node in file.SelectNodes("//timedeffects/timedeffect"))
            {
                string name = node.SelectSingleNode("name").InnerText;
                string category = node.SelectSingleNode("category").InnerText;
                int difficulty = Int32.Parse(node.SelectSingleNode("difficulty").InnerText);

                var durationNode = node.SelectSingleNode("duration");
                uint duration = durationNode != null ? UInt32.Parse(durationNode.InnerText) : 0;

                var activators = new List<EffectActivator>();

                foreach (XmlNode activatorNode in node.SelectNodes("activators/activator"))
                {
                    string type = activatorNode.Attributes["type"].Value;
                    MemoryAddress address = game.FindMemoryAddressByName(activatorNode.SelectSingleNode("address").InnerText);
                    dynamic target = address.ConvertToRightDataType(activatorNode.SelectSingleNode("target").InnerText);

                    activators.Add(new EffectActivator(type, target, address));
                }

                var effectLimitations = new List<Limitation>();

                foreach (XmlNode limitationNode in node.SelectNodes("limitations/limitation"))
                {
                    string limitationName = limitationNode.SelectSingleNode("name").InnerText;
                    Limitation limitation = game.FindLimitationByName(limitationName);

                    limitation.Target = Boolean.Parse(limitationNode.SelectSingleNode("target").InnerText);

                    XmlNodeList parameterNodes = limitationNode.SelectNodes("parameters/parameter");
                    if (parameterNodes.Count != 0)
                    {
                        Dictionary<string, string> parameters = new Dictionary<string, string>();

                        foreach (XmlNode parameterNode in parameterNodes)
                        {
                            string parameterName = parameterNode.SelectSingleNode("name").InnerText;
                            string parameterValue = parameterNode.SelectSingleNode("value").InnerText;
                            parameters.Add(parameterName, parameterValue);
                        }

                        limitation.setParameters(parameters);
                    }

                    effectLimitations.Add(limitation);
                }

                timedEffects.Add(new TimedEffect(name, category, difficulty, activators, duration, effectLimitations));
            }

            Debug.WriteLine("Read {0} timed effects from file.", timedEffects.Count);

            return timedEffects;
        }

        static public List<PermanentEffect> InitPermanentEffectsFromFile(Game game)
        {
            Debug.WriteLine("Reading permanent effects from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, permanentEffectsFilename);
            return new List<PermanentEffect>();
        }

        static public List<StaticEffect> InitStaticEffectsFromFile(Game game)
        {
            Debug.WriteLine("Reading static effects from file.");
            XmlDocument file = XmlUtils.getXmlDocument(game.abbreviation, staticEffectsFilename);
            return new List<StaticEffect>();
        }
    }
}
