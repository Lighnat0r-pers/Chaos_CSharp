using System.Collections.Generic;

namespace ChaosMod
{
    static class Settings
    {
        static private int defaultWaitTime = 250;
        static private int seedValidLength = 4;
        static private float programVersion = 2.0f; // Converted in interface to 2 decimal places.
        static private string programName = "Chaos%";

        // Define private variables.
        static private int sanicModeMultiplier = 10;
        static private int baseTimeMultiplier = 1;

        // Define public variables.
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

        static public string gameName;

        static public Dictionary<string, int> difficultiesArray;

#if DEBUG
        public const bool DEBUG_MODE_ENABLED = true;
#else
        public const bool DEBUG_MODE_ENABLED = false;
#endif

        static public int TimeMultiplier
        {
            get { return sanicModeEnabled ? baseTimeMultiplier / sanicModeMultiplier : baseTimeMultiplier; }
        }

        public static float PROGRAM_VERSION
        {
            get { return programVersion; }
        }

        public static int SEED_VALID_LENGTH
        {
            get { return seedValidLength; }
        }

        public static int DEFAULT_WAIT_TIME
        {
            get { return defaultWaitTime; }
        }

        public static string PROGRAM_NAME
        {
            get { return programName; }
        }

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