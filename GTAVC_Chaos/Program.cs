﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GTAVC_Chaos
{
    static class Program
    {
        static private Game[] gameArray;

        static public bool shouldStop = false;
        static public Form currentForm = null;

        static public WelcomeWindow welcomeWindow;
        static public OutputWindow outputWindow;
        static public Game game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetThreadCulture();

            // Set these application settings before creating any type of System.Windows.Forms object.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get information about supported games.
            gameArray = DataFileHandler.InitGamesFromFile();

            // Show the welcome window.
            InitWelcomeWindow();

            if (shouldStop == true) // Exit the application if the welcome window was exited.
            {
                Debug.WriteLine("Exiting application as stop signal was given");
                return;
            }

            // TODO(Ligh): Allow the user to select a game instead of hardcoding.
            Settings.gameName = "Grand Theft Auto: Vice City";

            InitOutputWindow();

            // Get the information we need about the game selected.
            GetGame(Settings.gameName);

            // Run the modules in a separate thread.
            Thread modulesThread = new Thread(game.InitModules);
            modulesThread.IsBackground = true;
            modulesThread.Start();

            while (shouldStop == false)
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }

            Debug.WriteLine("Chaos% shutting down: reached end of Main().");
        }

        /// <summary>
        /// To ensure the decimal separator is always a period, set the culture 
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
            Debug.WriteLine("Game chosen: " + name);
            game = Array.Find(gameArray, g => g.name == name);
            if (game == null)
            {
                throw new Exception("Invalid game chosen, not in games list. Game: " + name);
            }
            DataFileHandler.ReadFilesForGame(game);
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

    }
}