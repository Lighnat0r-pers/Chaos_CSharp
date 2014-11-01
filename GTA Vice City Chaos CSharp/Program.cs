using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using AccessProcessMemory;

namespace GTAVC_Chaos
{
    static class Program
    {
        #if DEBUG
            const bool DEBUG_MODE_ENABLED = true;
        #else
            const bool DEBUG_MODE_ENABLED = false;
        #endif

        public const int SEED_VALID_LENGTH = 4;
        public const float PROGRAM_VERSION = 1.21f;

        static public bool _shouldStop = false;

        static public WelcomeWindow welcomeWindow;



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // So that the decimal separator is always a period and stuff like that we set the cultureinfo 
            // to en-UK if it isn't already.
            if (Thread.CurrentThread.CurrentCulture.Name != "en-UK")
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-UK");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }

            // Enable registering when the application closes.
            Application.ApplicationExit += new EventHandler(OnApplicationExit);


            // While the user will be choosing settings in this thread, start another thread which will start
            // trying to get a handle to the game. Calling GameHandler.OpenGameProcess directly doesn't work
            // for some reason related to it requiring a parameter, so GetGame() is called which in turn calls
            // GameHandler.OpenGameProcess with the parameter.
            Thread gameAccessThread = new Thread(GetGame);
            gameAccessThread.IsBackground = true;
            gameAccessThread.Start();

            // Show the welcome window.
            InitWelcomeWindow();

            // Wait until the thread that gets the game handle is finished before continuing.
            gameAccessThread.Join();
            Debug.WriteLine("Continuing main thread as game handle thread is done");

            // Exit the application if the welcome window was exited.
            if (_shouldStop == true)
                Application.Exit();
        }

        /// <summary>
        /// Function that calls the function that opens a handle to the game in GameHandler so that this can be 
        /// done in a separate thread. The handle will be stored in GameHandler.memory
        /// </summary>
        static void GetGame() 
        {
            _shouldStop = false;
            Debug.WriteLine("Started attempts to get game handle");
            GameHandler.OpenGameProcess(Settings.gameName);
            if (GameHandler.memory != null)
                Debug.WriteLine("Game handle found");
            else
                Debug.WriteLine("Search for game handle aborted");
        }

        /// <summary>
        /// Initialise an instance of the welcome window class and make it visible.
        /// </summary>
        static void InitWelcomeWindow()
        {
            Debug.WriteLine("Initializing Welcome Window");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            welcomeWindow = new WelcomeWindow();
            Application.Run(welcomeWindow);
        }

        /// <summary>
        /// When the program exits, we need to do some 'garbage collection' to restore the game to its normal values.
        /// This will be extended when the program is in a further stage of development where it actually changes the game.
        /// </summary>
        static void OnApplicationExit(object Sender, EventArgs e)
        {
            _shouldStop = true;
            Debug.WriteLine("Event OnApplicationExit fired");
        }
    }
}