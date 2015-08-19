using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GTAVC_Chaos
{
    static class Program
    {
        static public bool _shouldStop = false;
        static public Form currentForm = null;

        static public WelcomeWindow welcomeWindow;
        static public NotifyIcon trayIcon;
        static public ContextMenu contextMenu;
        static public Components components;
        static public GameHandler game;

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

            // While the user will be shown the welcome window in this thread, start another thread which will start
            // trying to get a handle to the game.
            Thread gameAccessThread = new Thread(GetGame);
            gameAccessThread.IsBackground = true;
            gameAccessThread.Start();

            // Create the tray icon and context menu.
            InitTrayIcon();

            // Read addresses and effects from xml files.
            components = new Components();
            components.Init();

            // Show the welcome window.
            InitWelcomeWindow();

            // Wait until the thread that gets the game handle is finished before continuing.
            gameAccessThread.Join();
            if (_shouldStop == true) // Exit the application if the welcome window was exited by returning in the main method.
            {
                Debug.WriteLine("Exiting application as stop signal was given");
                return;
            }
            Debug.WriteLine("Continuing main thread as game handle thread is done");

            
            // Start the ModsLoop which will be in charge of activating the different modules.
            // Keep repeating the Update method until the program should stop.
            do
            {
                ModsLoop.Update();
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            } while (_shouldStop == false);
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
        /// Method that gets us access to the game.
        /// </summary>
        static void GetGame() 
        {
            _shouldStop = false;
            Debug.WriteLine("Started attempts to get game handle");
            game = new GameHandler(Settings.gameName);
            if (game.gameFound == true)
                Debug.WriteLine("Game handle found");
            else
                Debug.WriteLine("Search for game handle aborted");

            //byte read = ReadWriteTest<byte>();
            //Debug.WriteLine(String.Format("Read value: {0}", read));
        }

        /*
        static T ReadWriteTest<T>()
        {
            int address = 0x00A0FB75; // TimeHours
            T readValue;
            byte writeValue = 14;
            readValue = game.Read<T>(address, 1);
            game.Write<byte>(address, writeValue, 1);
            return readValue;
        }
        */

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
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItemExit, menuItemRestart});
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
            Debug.WriteLine("Context menu item Exit clicked");
            Application.Exit();
        }

        /// <summary>
        /// Triggered when the user clicks the Exit button in the context menu.
        /// </summary>
        private static void menuItemRestart_Click(object Sender, EventArgs e)
        {
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
            _shouldStop = true;
            if (game != null && game.gameFound == true)
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