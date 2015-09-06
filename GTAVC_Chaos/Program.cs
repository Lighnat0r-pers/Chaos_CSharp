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
        static public NotifyIcon trayIcon;
        static public ContextMenu contextMenu;
        static public Game game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetThreadCulture();

            // Enable registering when the application closes.
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            // Set some application settings BEFORE creating any type of System.Windows.Forms object.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get information about supported games.
            gameList = new GameList(DataFileHandler.InitGamesFromFile());

            // Create the tray icon and context menu.
            InitTrayIcon();

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

            // Start the ModsLoop which will be in charge of activating the different modules.
            // Keep repeating the Update method until the program should stop.
            do
            {
                Debug.WriteLine(GameFunctions.GetCurrentMission());
                ModsLoop.Update();
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            } while (shouldStop == false);

            Debug.WriteLine("ModsLoop ended.");
        }

        /// <summary>
        /// To ensure the decimal separator is always a period and stuff like that, we set the culture 
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

        /// <summary>
        /// Create the tray icon.
        /// </summary>
        static void InitTrayIcon()
        {
            Debug.WriteLine("Creating tray icon");
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Properties.Resources.SunriseIcon;
            trayIcon.Text = Settings.PROGRAM_NAME;
            CreateContextMenu();
            trayIcon.ContextMenu = contextMenu;
            trayIcon.Visible = true;
        }

        /// <summary>
        /// Creates the context menu and populates it.
        /// </summary>
        static void CreateContextMenu()
        {
            contextMenu = new ContextMenu();
            MenuItem menuItemExit = new MenuItem();
            MenuItem menuItemRestart = new MenuItem();
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItemExit, menuItemRestart });
            menuItemExit.Index = 1;
            menuItemExit.Text = "Exit";
            menuItemExit.Click += new EventHandler(menuItemExit_Click);
            menuItemRestart.Index = 0;
            menuItemRestart.Text = "Restart Program";
            menuItemRestart.Click += new EventHandler(menuItemRestart_Click);
        }

        /// <summary>
        /// Triggered when the user clicks the Exit button in the context menu.
        /// </summary>
        private static void menuItemExit_Click(object Sender, EventArgs e)
        {
            // TODO(Ligh): Due to changes is handling the application, this no longer shuts down the program. It should.
            Debug.WriteLine("Context menu item Exit clicked");
            Application.Exit();
        }

        /// <summary>
        /// Triggered when the user clicks the Restart Program button in the context menu.
        /// </summary>
        private static void menuItemRestart_Click(object Sender, EventArgs e)
        {
            // TODO(Ligh): Due to changes is handling the application, this no longer works correctly. It should.
            Debug.WriteLine("Context menu item Restart Program clicked");
            Application.Restart();
        }

        /// <summary>
        /// Initialise an instance of the welcome window class and make it visible.
        /// </summary>
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

        /// <summary>
        /// When the program exits, we need to release and remove some objects manually to ensure correct behaviour.
        /// Later on, we also need to do some 'garbage collection' to restore the game to its normal values.
        /// This will be extended when the program is in a further stage of development where it actually changes the game.
        /// </summary>
        static void OnApplicationExit(object Sender, EventArgs e)
        {
            // TODO(Ligh): Due to changes is handling the application, this is no longer called on exit. It should be.
            shouldStop = true;
            if (game != null && game.hasHandle == true)
            {
                game.CloseProcess();
            }
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
            }
            Debug.WriteLine("Event OnApplicationExit fired");
        }
    }
}