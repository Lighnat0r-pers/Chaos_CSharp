using System;

namespace GTAVC_Chaos
{
    static class ModsLoop
    {

        /// <summary>
        /// Property gameStatus which calls the CheckGameStatus() method to return what the game is up to.
        /// </summary>
        static public int gameStatus
        {
            get { return CheckGameStatus(); }
        }

        /// <summary>
        /// Main method that should be called continuously. This method is in charge of
        /// activating the different modules in the mod (e.g. TimedEffects, StaticEffects etc)
        /// </summary>
        static public void Update()
        {

            if (CheckGameStatus() == 1) //Game not running.
            {
                //Deactivate everything
            }
            else
            {
                //Do loops
            }
        }

        static public int CheckGameStatus()
        {

            return 0;
        }
    }
}
