using System.Collections.Generic;

namespace ChaosMod
{
    static class Settings
    {
#if DEBUG
        static public bool DEBUG_MODE_ENABLED => true;
#else
        static public bool DEBUG_MODE_ENABLED => false;
#endif

        static public bool TimedEffectsEnabled { get; set; }
        static public bool PermanentEffectsEnabled { get; set; }
        static public bool StaticEffectsEnabled { get; set; }
        static public bool SanicModeEnabled { get; set; }
        static public int Seed { get; set; }
        static public Difficulty Difficulty { get; set; }
        static public Game Game { get; set; }
        static public List<Game> SupportedGames { get; set; }
        static public TimedEffect CurrentEffect { get; set; }

        static private int sanicModeMultiplier => 10;
        static private int baseTimeMultiplier => 1;

        static public int TimeMultiplier => SanicModeEnabled ? baseTimeMultiplier / sanicModeMultiplier : baseTimeMultiplier;
        static public double ProgramVersion => 2.0;
        static public int SeedValidLength => 4;
        static public string ProgramName => "Chaos%";
        static public bool timedEffectsEnabledDefault => true;
        static public bool staticEffectsEnabledDefault => true;
        static public bool sanicModeEnabledDefault => false;

        static public Difficulty defaultDifficulty => new Difficulty("Medium", 7);
        static public List<Difficulty> difficulties =>
            new List<Difficulty>()
            {
                new Difficulty("Easy", 4),
                defaultDifficulty,
                new Difficulty("Hard", 10),
            };
    }

    struct Difficulty
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Difficulty(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}