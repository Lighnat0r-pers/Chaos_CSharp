using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ChaosMod
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
            game.BaseChecks = ReadBaseChecks(game);
            Debug.WriteLine("Done reading files for game.");
        }

        static public List<Game> ReadGames()
        {
            Debug.WriteLine("Reading games from file.");

            var games =
                XmlUtils.getXDocument("", gamesFilename).Descendants("game")
                .Select(game => new Game
                (
                    game.Element("name").Value,
                    game.Element("abbreviation").Value,
                    game.Element("windowname").Value,
                    game.Element("windowclass").Value,
                    Int64.Parse(game.Element("versionaddress").Value, NumberStyles.HexNumber),
                    game.Descendants("version")
                    .Select(version => new GameVersion
                    (
                        version.Element("name").Value,
                        Int32.Parse(version.Element("addressvalue").Value, NumberStyles.HexNumber),
                        new SortedList<long, int>
                        (
                            version.Descendants("offset").ToDictionary
                            (
                                c => Int64.Parse(c.Element("startaddress").Value, NumberStyles.HexNumber),
                                c => Int32.Parse(c.Element("amount").Value, NumberStyles.HexNumber) * (c.Element("amount").Attribute("negative")?.Value == "true" ? -1 : 1)
                            )
                        )
                    ))
                    .ToList()
                ))
                .ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return games;
        }

        static private List<MemoryAddress> ReadMemoryAddresses(Game game)
        {
            Debug.WriteLine("Reading memory addresses from file.");
            var file = XmlUtils.getXDocument(game.abbreviation, memoryAddressesFilename);

            var memoryAddresses =
                file.Descendants("memoryaddress")
                .Select(address =>
                {
                    switch (address.Element("address").Attribute(XmlUtils.xsiNamespace + "type").Value)
                    {
                        case "static":
                            return new MemoryAddress
                            (
                                game,
                                address.Element("name").Value,
                                Int64.Parse(address.Element("address").Value, NumberStyles.HexNumber),
                                game.gameVersions.Find(v => v.name == file.Element("addresses").Attribute("gameversion").Value),
                                address.Element("datatype").Value,
                                Int32.Parse(address.Element("length")?.Value ?? "0")
                            );
                        case "dynamic":
                            return new MemoryAddress
                            (
                                game,
                                address.Element("name").Value,
                                address.Element("address").Element("baseaddress").Value,
                                Int64.Parse(address.Element("address").Element("offset").Value, NumberStyles.HexNumber),
                                address.Element("datatype").Value,
                                Int32.Parse(address.Element("length")?.Value ?? "0")
                            );
                        default:
                            throw new NotSupportedException($"Tried to process unknown address type: {address.Element("address").Attribute(XmlUtils.xsiNamespace + "type").Value}");
                    }
                })
                .ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return memoryAddresses;
        }

        static public List<BaseCheck> ReadBaseChecks(Game game)
        {
            Debug.WriteLine("Reading base checks from file.");
            var file = XmlUtils.getXDocument(game.Abbreviation, limitationsFilename);

            var baseChecks =
                file.Descendants("base").First().Descendants("check")
                .Select(check => new BaseCheck
                (
                    game.FindMemoryAddressByName(check.Element("address").Value),
                    check.Element("failCase").Value,
                    check.Element("onFail").Value
                ))
                .ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return baseChecks;
        }

        static private Limitation ReadLimitation(Game game, string limitationName)
        {
            // IMPORTANT(Ligh): This function cannot be called before the memory addresses have all been read. 

            Debug.WriteLine($"Reading limitation {limitationName} from file.");
            var file = XmlUtils.getXDocument(game.abbreviation, limitationsFilename);

            var limitation =
                file.Descendants("limitation")
                .Where(limit => limit.Element("name")?.Value == limitationName)
                .Select(limit => new Limitation
                (
                    limit.Element("name").Value,
                    limit.Descendants("check")
                    .Select<XElement, ICheck>(check =>
                    {
                        switch (check.Attribute(XmlUtils.xsiNamespace + "type").Value)
                        {
                            case "parameter":
                                return new ParameterCheck
                                (
                                    game.FindMemoryAddressByName(check.Element("address").Value),
                                    check.Element("value")?.Value
                                );
                            case "limitation":
                                return new LimitationCheck
                                (
                                    ReadLimitation(game, check.Element("limitation").Value),
                                    Boolean.Parse(check.Element("target").Value),
                                    ReadParameters(check)
                                );
                            case "comparison":
                                return new ComparisonCheck
                                (
                                    check.Descendants("address").Select(c => game.FindMemoryAddressByName(c.Value)).ToList(),
                                    Boolean.Parse(check.Element("equal").Value)
                                );
                            default:
                                throw new NotSupportedException($"Tried to process unknown limitation check type: {check.Attribute(XmlUtils.xsiNamespace + "type").Value}");
                        }
                    })
                    .ToList()
                ))
                .FirstOrDefault();

            // TODO(Ligh): Validate everything has been read correctly.

            return limitation;
        }

        static public List<TimedEffect> ReadTimedEffects(Game game)
        {
            // IMPORTANT(Ligh): This function cannot be called before the memory addresses have all been read. 

            Debug.WriteLine("Reading timed effects from file.");

            var timedEffects =
                XmlUtils.getXDocument(game.abbreviation, timedEffectsFilename).Descendants("timedeffect")
                .Select(effect => new TimedEffect
                (
                    effect.Element("name").Value,
                    effect.Element("category").Value,
                    Int32.Parse(effect.Element("difficulty").Value),
                    effect.Descendants("activator")
                    .Select(activator =>
                    new EffectActivator
                    (
                        activator.Attribute("type").Value,
                        activator.Element("target").Value,
                        game.FindMemoryAddressByName(activator.Element("address").Value)
                    ))
                    .ToList(),
                    UInt32.Parse(effect.Element("duration")?.Value ?? "0"),
                    effect.Descendants("limitation")
                    .Select(limit =>
                    {
                        var limitation = ReadLimitation(game, limit.Element("name").Value);
                        limitation.Target = Boolean.Parse(limit.Element("target").Value);
                        limitation.SetParameters(ReadParameters(limit));
                        return limitation;
                    })
                    .ToList()
                ))
                .ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return timedEffects;
        }

        static private Dictionary<string, string> ReadParameters(XElement parentNode)
        {
            return parentNode.Descendants("parameter").ToDictionary
            (
                c => c.Element("name").Value,
                c => c.Element("value").Value
            );
        }
    }
}
