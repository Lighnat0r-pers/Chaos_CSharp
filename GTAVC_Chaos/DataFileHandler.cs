using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace GTAVC_Chaos
{
    static class DataFileHandler
    {
        static private string gamesFilename = "Games";
        static private string memoryAddressesFilename = "MemoryAddresses";
        static private string limitationsFilename = "Limitations";
        static private string timedEffectsFilename = "TimedEffects";
        //static private string permanentEffectsFilename = "PermanentEffects";
        //static private string staticEffectsFilename = "StaticEffects";

        static public void ReadFilesForGame(Game game)
        {
            Debug.WriteLine("Starting to read files for game.");
            game.memoryAddresses = ReadMemoryAddresses(game);
            //game.baseLimitations = GetBaseLimitationsFromFile(game);
            Debug.WriteLine("Done reading files for game.");
        }

        static public List<Game> ReadGames()
        {
            Debug.WriteLine("Reading games from file.");

            var games =
                (from game in XmlUtils.getXDocument("", gamesFilename).Descendants("game")
                 select new Game
                 (
                     game.Element("name").Value,
                     game.Element("abbreviation").Value,
                     game.Element("windowname").Value,
                     game.Element("windowclass").Value,
                     Int64.Parse(game.Element("versionaddress").Value, NumberStyles.HexNumber),
                     game.Element("baseversion").Value,
                     new List<GameVersion>
                     (from version in game.Descendants("version")
                      select new GameVersion
                      (
                          version.Element("name").Value,
                          Int32.Parse(version.Element("addressvalue").Value, NumberStyles.HexNumber),
                          (version.Element("name").Value == game.Element("baseversion").Value
                          ? new SortedList<long, int>() { { 0, 0 } } // Dummy offset
                          : new SortedList<long, int>
                            (version.Descendants("offset").ToDictionary(
                                c => Int64.Parse(c.Element("startaddress").Value, NumberStyles.HexNumber),
                                c => Int32.Parse(c.Element("amount").Value, NumberStyles.HexNumber) * (c.Element("amount").Attribute("negative")?.Value == "true" ? -1 : 1)
                            ))
                          )
                      ))
                 )).ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return games;
        }

        static private List<MemoryAddress> ReadMemoryAddresses(Game game)
        {
            Debug.WriteLine("Reading memory addresses from file.");
            var file = XmlUtils.getXDocument(game.abbreviation, memoryAddressesFilename);

            // Read static addresses
            var memoryAddresses =
                (from address in file.Descendants("memoryaddress")
                 where address.Element("address").Attribute(XmlUtils.xsiNamespace + "type").Value == "static"
                 select new MemoryAddress
                 (
                     game,
                     address.Element("name").Value,
                     Int64.Parse(address.Element("address").Value, NumberStyles.HexNumber),
                     game.gameVersions.Find(v => v.name == file.Element("addresses").Attribute("gameversion").Value),
                     address.Element("datatype").Value,
                     Int32.Parse(address.Element("length")?.Value ?? "0")
                 )).ToList();

            // Read dynamic addresses
            memoryAddresses.AddRange(
                (from address in file.Descendants("memoryaddress")
                 where address.Element("address").Attribute(XmlUtils.xsiNamespace + "type").Value == "dynamic"
                 select new MemoryAddress
                 (
                     game,
                     address.Element("name").Value,
                     address.Element("address").Element("baseaddress").Value,
                     Int64.Parse(address.Element("address").Element("offset").Value, NumberStyles.HexNumber),
                     address.Element("datatype").Value,
                     Int32.Parse(address.Element("length")?.Value ?? "0")
                 )).ToList());

            // TODO(Ligh): Validate everything has been read correctly.

            return memoryAddresses;
        }

        static private List<Limitation> GetBaseLimitationsFromFile(Game game)
        {
            // TODO(Ligh): Properly catch errors here.

            // TODO(Ligh): Read base limitations here and implement them elsewhere.

            Debug.WriteLine("Reading base limitations from file.");
            var file = XmlUtils.getXmlDocument(game.abbreviation, limitationsFilename);

            var baseLimitations = new List<Limitation>();

            foreach (XmlNode node in file.SelectNodes("//limitations/limitation"))
            {
                ICheck check;
                string name = node.SelectSingleNode("name").InnerText;

                var checks = new List<ICheck>();

                foreach (XmlNode checkNode in node.SelectNodes("checks/check"))
                {
                    switch (checkNode.Attributes["xsi:type"].Value)
                    {
                        case "parameter":
                            var address = game.FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                            dynamic value = checkNode.SelectSingleNode("value").InnerText;
                            check = new ParameterCheck(address, value);
                            break;
                        default:
                            throw new NotSupportedException($"Tried to process unknown limitation check type: {checkNode.Attributes["xsi:type"].Value}");
                    }

                    checks.Add(check);
                }

                baseLimitations.Add(new Limitation(name, checks));
            }

            Debug.WriteLine($"Read {baseLimitations.Count} base limitations from file.");

            return baseLimitations;
        }

        static private Limitation GetLimitationFromFile(Game game, string limitationName)
        {
            // TODO(Ligh): Properly catch errors here.

            // TODO(Ligh): The simple check is actually nothing more than a parameter check with a value that is not changed.
            // It's already handled that way internally, but perhaps there shouldn't be a distinction in the xml either.

            // IMPORTANT(Ligh): This function cannot be called before the memory addresses have all been read. 

            Debug.WriteLine($"Reading limitation {limitationName} from file.");
            var file = XmlUtils.getXmlDocument(game.abbreviation, limitationsFilename);

            var node = file.SelectSingleNode($"//limitations/limitation[name = '{limitationName}']");
            ICheck check;
            string name = node.SelectSingleNode("name").InnerText;

            var checks = new List<ICheck>();

            foreach (XmlNode checkNode in node.SelectNodes("checks/check"))
            {
                switch (checkNode.Attributes["xsi:type"].Value)
                {
                    case "parameter":
                        var address = game.FindMemoryAddressByName(checkNode.SelectSingleNode("address").InnerText);
                        string value = checkNode.SelectSingleNode("default")?.InnerText;
                        check = new ParameterCheck(address, value);
                        break;
                    case "limitation":
                        var limitation = GetLimitationFromFile(game, checkNode.SelectSingleNode("limitation").InnerText);
                        limitation.Target = Boolean.Parse(checkNode.SelectSingleNode("target").InnerText);

                        var parameterNodes = checkNode.SelectNodes("parameters/parameter");
                        if (parameterNodes.Count > 0)
                        {
                            var parameters = new Dictionary<string, string>();

                            foreach (XmlNode parameterNode in parameterNodes)
                            {
                                string parameterName = parameterNode.SelectSingleNode("name").InnerText;
                                string parameterValue = parameterNode.SelectSingleNode("value").InnerText;
                                parameters.Add(parameterName, parameterValue);
                            }

                            limitation.SetParameters(parameters);
                        }

                        check = new LimitationCheck(limitation);
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
                        throw new NotSupportedException($"Tried to process unknown limitation check type: {checkNode.Attributes["xsi:type"].Value}");
                }

                checks.Add(check);
            }

            return new Limitation(name, checks);
        }

        static public List<TimedEffect> ReadTimedEffects(Game game)
        {
            // IMPORTANT(Ligh): This function cannot be called before the memory addresses have all been read. 

            Debug.WriteLine("Reading timed effects from file.");

            var timedEffects =
                (from effect in XmlUtils.getXDocument(game.abbreviation, timedEffectsFilename).Descendants("timedeffect")
                 select new TimedEffect
                 (
                     effect.Element("name").Value,
                     effect.Element("category").Value,
                     Int32.Parse(effect.Element("difficulty").Value),
                     new List<EffectActivator>
                     (from activator in effect.Descendants("activator")
                      select new EffectActivator
                      (
                          activator.Attribute("type").Value,
                          activator.Element("target").Value,
                          game.FindMemoryAddressByName(activator.Element("address").Value)
                     )).ToList(),
                     UInt32.Parse(effect.Element("duration")?.Value ?? "0"),
                     new List<Limitation>
                     (from limitation in effect.Descendants("limitation")
                      select GetLimitationFromFile(game, limitation.Element("name").Value)
                      ).ToList()
                 )).ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return timedEffects;
        }
    }
}
