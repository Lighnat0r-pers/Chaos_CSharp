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
    }
}