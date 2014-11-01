using System.Collections.Generic;

namespace GTAVC_Chaos
{
    static class Settings
    {
        // Define private variables.
        static int sanicModeMultiplier = 10;
        static int baseTimeMultiplier = 1;

        // Define public variables.
        static public bool timedEffectsEnabledDefault = true;
        static public bool staticEffectsEnabledDefault = true;
        static public bool sanicModeEnabledDefault = false;
        static public string gameName = "GTAVC";

        static public bool timedEffectsEnabled;
        static public bool staticEffectsEnabled;
        static public bool sanicModeEnabled;
        static public uint seed;

        static public Dictionary<string, string> gameWindowNameArray = new Dictionary<string, string>();
        static public Dictionary<string, string> gameWindowClassNameArray = new Dictionary<string, string>();

        public const int DEFAULT_WAIT_TIME = 250;
        public const int SEED_VALID_LENGTH = 4;
        public const float PROGRAM_VERSION = 2.0f; // Converted in interface to 2 decimal places.
        public const string PROGRAM_NAME = "GTA VC Chaos%";

        #if DEBUG
            const bool DEBUG_MODE_ENABLED = true;
        #else
            const bool DEBUG_MODE_ENABLED = false;
        #endif

        /// <summary>
        /// Readonly property returning the finalised multiplier for time.
        /// </summary>
        static public int timeMultiplier
        {
            get
            {
                if (Settings.sanicModeEnabled)
                    return baseTimeMultiplier * sanicModeMultiplier;
                else
                    return baseTimeMultiplier;
            }
        }

        /// <summary>
        /// Static constructor for the Settings class that will run once at the start of the program.
        /// </summary>
        static Settings()
        {
            gameWindowNameArray.Add(gameName, "GTA: Vice City");
            gameWindowClassNameArray.Add(gameName, "Grand theft auto 3");
        }
    }
}