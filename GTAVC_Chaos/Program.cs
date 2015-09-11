using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GTAVC_Chaos
{
    static class Program
    {
        static private GameList gameList;

        static public bool shouldStop = false;
        static public Form currentForm = null;

        static public WelcomeWindow welcomeWindow;
        static public OutputWindow outputWindow;
        static public Game game;
        static public Modules modules;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetThreadCulture();

            // Enable registering when the process closes.
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit); 

            // Set some application settings BEFORE creating any type of System.Windows.Forms object.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get information about supported games.
            gameList = new GameList(DataFileHandler.InitGamesFromFile());

            modules = new Modules();

            // Show the welcome window.
            InitWelcomeWindow();

            if (shouldStop == true) // Exit the application if the welcome window was exited by returning in the main method.
            {
                Debug.WriteLine("Exiting application as stop signal was given");
                return;
            }

            // Get the information we need about the game selected.
            // TODO(Ligh): Allow the user to select a game instead of hardcoding.
            string gameName = "Grand Theft Auto: Vice City";
            Debug.WriteLine("Game chosen: " + gameName);
            GetGame(gameName);

            InitOutputWindow();

            Thread modsLoopThread = new Thread(modules.Update);
            modsLoopThread.IsBackground = true;
            modsLoopThread.Start();

            // Start the ModsLoop which will be in charge of activating the different modules.
            // Keep repeating the Update method until the program should stop.
            do
            {
                Thread.Sleep(1);
                Application.DoEvents();
            } while (shouldStop == false);

            Debug.WriteLine("ModsLoop ended.");
        }

        /// <summary>
        /// To ensure the decimal separator is always a period and stuff like that, set the culture 
        /// to en-UK if it isn't already.
        /// </summary>
        private static void SetThreadCulture()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "en-UK")
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-UK");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        /// <summary>
        /// Method that gets us access to the game and information for that game.
        /// </summary>
        static void GetGame(string name)
        {
            shouldStop = false;
            Debug.WriteLine("Started attempts to get game handle");
            game = gameList.FindGameByName(name);
            if (game == null)
            {
                throw new Exception("Invalid game chosen, not in games list. Game: " + name);
            }
            game.GetHandle();
            if (game.hasHandle == true)
            {
                Debug.WriteLine("Game handle found");
                DataFileHandler.ReadFilesForGame(game);
                Debug.WriteLine("Done reading files for game.");
            }
            else
            {
                Debug.WriteLine("Search for game handle aborted");
            }
        }

        }

        static void InitWelcomeWindow()
        {
            Debug.WriteLine("Initializing Welcome Window");
            welcomeWindow = new WelcomeWindow();
            welcomeWindow.Show();
            welcomeWindow.Refresh();

            while (welcomeWindow.Visible)
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }
        }

        static void InitOutputWindow()
        {
            Debug.WriteLine("Initializing Output Window");
            outputWindow = new OutputWindow();
            outputWindow.Show();
            outputWindow.Refresh();
        }


        static void OnProcessExit(object Sender, EventArgs e)
        {
            shouldStop = true;
            if (game != null && game.hasHandle == true)
            {
                game.CloseProcess();
            }
            Debug.WriteLine("Event OnProcessExit fired");
        }
    }
}