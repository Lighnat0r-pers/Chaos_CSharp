using System.Collections.Generic;

namespace ChaosMod
{
    static class Settings
    {
        static private int defaultWaitTime = 250;
        static private int seedValidLength = 4;
        static private float programVersion = 2.0f; // Converted in interface to 2 decimal places.
        static private string programName = "Chaos%";
        static private int sanicModeMultiplier = 10;
        static private int baseTimeMultiplier = 1;

        static public bool timedEffectsEnabledDefault = true;
        static public bool staticEffectsEnabledDefault = true;
        static public bool sanicModeEnabledDefault = false;
        static public int defaultDifficulty;
        static public bool timedEffectsEnabled;
        static public bool permanentEffectsEnabled;
        static public bool staticEffectsEnabled;
        static public bool sanicModeEnabled;
        static public int seed;
        static public int difficulty;
        static public string difficultyName;
        static public Game game;
        static public TimedEffect currentEffect;

        static public Dictionary<string, int> difficultiesArray;
        static private List<Game> supportedGames;

#if DEBUG
        public const bool DEBUG_MODE_ENABLED = true;
#else
        public const bool DEBUG_MODE_ENABLED = false;
#endif

        static public List<Game> SupportedGames
        {
            get { return supportedGames; }
            set { supportedGames = value; }
        }

        static public TimedEffect CurrentEffect
        {
            get { return currentEffect; }
            set { currentEffect = value; }
        }

        static public int TimeMultiplier => sanicModeEnabled ? baseTimeMultiplier / sanicModeMultiplier : baseTimeMultiplier;
        static public float ProgramVersion => programVersion;
        static public int SeedValidLength => seedValidLength;
        static public int DefaultWaitTime => defaultWaitTime;
        static public string ProgramName => programName;

        /// <summary>
        /// Static constructor for the Settings class that will run once at the start of the program.
        /// </summary>
        static Settings()
        {
            difficultiesArray = new Dictionary<string, int>()
            {
                {"Easy", 4},
                {"Medium", 7},
                {"Hard", 10},
            };

            defaultDifficulty = difficultiesArray["Medium"];
        }
    }
}