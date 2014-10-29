using System.Collections.Generic;

namespace GTAVC_Chaos
{
    static class Settings
    {
        // Define private variables.
        static int sanicModeMultiplier = 10;
        static int baseTimeMultiplier = 1;

        // Define public variables.
        static public bool timedEffectsEnabled = true;
        static public bool staticEffectsEnabled = true;
        static public bool sanicModeEnabled = false;


        static public Dictionary<string, string> gameWindowNameArray = new Dictionary<string, string>();
        static public Dictionary<string, string> gameWindowClassNameArray = new Dictionary<string, string>();

        public const int DEFAULT_WAIT_TIME = 250;

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
            gameWindowNameArray.Add("GTAVC", "GTA: Vice City");
            gameWindowClassNameArray.Add("GTAVC", "Grand theft auto 3");
        }
    }
}