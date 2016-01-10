using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using AccessProcessMemory;

namespace ChaosMod
{
    static class DataFileHandler
    {
        // TODO(Ligh): All reads from files are prone to exceptions, so wrap them in try blocks and
        // properly report errors to the user (what went wrong, how to fix etc).
        // Also try to recover from errors as much as possible.
        static private string gamesFilename => "Games";
        static private string memoryAddressesFilename => "MemoryAddresses";
        static private string limitationsFilename => "Limitations";
        static private string timedEffectsFilename => "TimedEffects";
        //static private string permanentEffectsFilename => "PermanentEffects";
        //static private string staticEffectsFilename => "StaticEffects";

        static private bool memoryAddressesRead = false;

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

        static public List<MemoryAddress> ReadMemoryAddresses(Game game)
        {
            Debug.WriteLine("Reading memory addresses from file.");
            var file = XmlUtils.getXDocument(game.Abbreviation, memoryAddressesFilename);

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
                                game.Versions.Find(v => v.Name == file.Element("addresses").Attribute("gameversion").Value),
                                (DataType)Enum.Parse(typeof(DataType), address.Element("datatype").Value, true),
                                Int32.Parse(address.Element("length")?.Value ?? "0")
                            );
                        case "dynamic":
                            return new MemoryAddress
                            (
                                game,
                                address.Element("name").Value,
                                address.Element("address").Element("baseaddress").Value,
                                Int64.Parse(address.Element("address").Element("offset").Value, NumberStyles.HexNumber),
                                (DataType)Enum.Parse(typeof(DataType), address.Element("datatype").Value, true),
                                Int32.Parse(address.Element("length")?.Value ?? "0")
                            );
                        default:
                            throw new NotSupportedException($"Tried to process unknown address type: {address.Element("address").Attribute(XmlUtils.xsiNamespace + "type").Value}");
                    }
                })
                .ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            memoryAddressesRead = true;

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
                    (FailType)Enum.Parse(typeof(FailType), check.Element("failType")?.Value, true)
                ))
                .ToList();

            // TODO(Ligh): Validate everything has been read correctly.

            return baseChecks;
        }

        static public List<TimedEffect> ReadTimedEffects(Game game)
        {
            if (!memoryAddressesRead)
            {
                throw new InvalidOperationException("Cannot read timed effects before reading memory addresses.");
            }

            Debug.WriteLine("Reading timed effects from file.");

            var timedEffects =
                XmlUtils.getXDocument(game.Abbreviation, timedEffectsFilename).Descendants("timedeffect")
                .Select(effect => new TimedEffect
                (
                    effect.Element("name").Value,
                    effect.Element("category").Value,
                    Int32.Parse(effect.Element("difficulty").Value),
                    effect.Descendants("activator")
                    .Select(activator =>
                    new EffectActivator
                    (
                        activator.Element("target").Value,
                        game.FindMemoryAddressByName(activator.Element("address").Value),
                        (ActivationType)Enum.Parse(typeof(ActivationType), activator.Element("activation")?.Value ?? default(ActivationType).ToString(), true),
                        (DeactivationType)Enum.Parse(typeof(DeactivationType), activator.Element("deactivation")?.Value ?? default(DeactivationType).ToString(), true)
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

        static private Limitation ReadLimitation(Game game, string limitationName)
        {
            if (!memoryAddressesRead)
            {
                throw new InvalidOperationException("Cannot read timed effects before reading memory addresses.");
            }

            Debug.WriteLine($"Reading limitation {limitationName} from file.");
            var file = XmlUtils.getXDocument(game.Abbreviation, limitationsFilename);

            var limitation =
                file.Descendants("limitation")
                .Where(limit => limit.Element("name")?.Value == limitationName)
                .Select(limit => new Limitation
                (
                    limitationName,
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
