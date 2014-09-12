using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace GTAVC_Chaos
{
    static class Program
    {
        static public WelcomeWindow welcomeWindow;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Debug.WriteLine("Started up");
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Debug.WriteLine("Event OnApplicationExit added");
            InitWelcomeWindow();

        }

        static void InitWelcomeWindow()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            welcomeWindow = new WelcomeWindow();
            Debug.WriteLine("Event OnWelcomeWindowClose added");
            Application.Run(welcomeWindow);
        }

        static void OnApplicationExit(object Sender, EventArgs e)
        {
            Debug.WriteLine("Event OnApplicationExit fired");
        }
    }
}
