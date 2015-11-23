using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace ChaosMod
{
    static class Program
    {
        static private List<Game> gameArray;

        static public bool shouldStop = false;
        static public Game game;

        [STAThread]
        static void Main()
        {
            App app = new App();

            // IMPORTANT(Ligh): All windows need to be created before the first one is shown.
            var welcomeWindow = new WelcomeWindow();
            var outputWindow = new OutputWindow();

            SetUKCultureInfo();

            // Get information about supported games.
            gameArray = DataFileHandler.ReadGames();

            // Show the welcome window.
            Debug.WriteLine("Showing Welcome Window");
            welcomeWindow.ShowDialog();

            if (shouldStop == true) // Exit the application if the welcome window was exited.
            {
                Debug.WriteLine("Exiting application as stop signal was given");
                return;
            }

            // TODO(Ligh): Allow the user to select a game instead of hardcoding.
            Settings.gameName = "Grand Theft Auto: Vice City";

            // Run the modules in a separate thread.
            Thread modulesThread = new Thread(RunGameLoop);
            modulesThread.Start(Settings.gameName);

            // Show the output window.
            Debug.WriteLine("Showing Output Window");
            outputWindow.ShowDialog();

            Debug.WriteLine("Chaos% shutting down: reached end of Main().");
            modulesThread.Abort();
        }

        /// <summary>
        /// To ensure the decimal separator is always a period, set the culture to en-UK.
        /// </summary>
        private static void SetUKCultureInfo()
        {
            if (CultureInfo.CurrentCulture.Name != "en-UK")
            {
                var culture = CultureInfo.CreateSpecificCulture("en-UK");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
        }

        static void RunGameLoop(object gameName)
        {
            string name = (string)gameName;
            Debug.WriteLine($"Game chosen: {name}");
            game = gameArray.Find(g => g.name == name);

            if (game == null)
            {
                throw new ArgumentOutOfRangeException(nameof(game), $"Invalid game chosen, not in games list. Game: {name}");
            }

            DataFileHandler.ReadFilesForGame(game);
            game.ResolveReferences();

            game.DoModulesLoop();
        }
    }
}
