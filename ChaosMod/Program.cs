using System;
using System.Diagnostics;
using System.Threading;

namespace ChaosMod
{
    static class Program
    {
        static public bool ShouldStop { get; set; }

        [STAThread]
        static void Main()
        {
            App app = new App();

            // IMPORTANT(Ligh): All windows need to be created before the first one is shown.
            var welcomeWindow = new WelcomeWindow();
            var outputWindow = new OutputWindow();

            // Get information about supported games.
            Settings.SupportedGames = DataFileHandler.ReadGames();

            // Show the welcome window.
            Debug.WriteLine("Showing Welcome Window");
            welcomeWindow.ShowDialog();

            if (ShouldStop == true) // Exit the application if the welcome window was exited.
            {
                Debug.WriteLine("Exiting application as stop signal was given");
                outputWindow.Close();
                return;
            }

            // Run the modules in a separate thread.
            Thread modulesThread = new Thread(Settings.Game.StartModulesLoop);
            modulesThread.Start();

            // Show the output window.
            Debug.WriteLine("Showing Output Window");
            outputWindow.ShowDialog();

            Debug.WriteLine("Program shutting down: reached end of Main().");
        }
    }
}
