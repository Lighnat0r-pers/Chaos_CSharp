using AccessProcessMemory;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GTAVC_Chaos
{
    static class GameHandlerApi
    {
        [DllImport("user32.dll", CharSet=CharSet.Unicode)] // GetClassName
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int MaxCount);
    }

    static class GameHandler
    {


        //static public Process gameProcess;
        static public Memory memory = null;

        // External function that outputs window class name for a given handle.
        //static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int MaxCount);

        /// <summary>
        /// This function gets all processes. It will then check if any of the processes has the window
        /// name gameWindowName and the window class name gameClassName. If this is the case, it will 
        /// instantiate the Memory class for the (first) matching process to be used for reading and 
        /// writing the memory of the process. In no matching process is found, the method will sleep 
        /// and keep trying until a matching process is found.
        /// </summary>
        static public void OpenGameProcess(string gameName)
        {
            string gameWindowName = Settings.gameWindowNameArray[gameName];
            string gameClassName = Settings.gameWindowClassNameArray[gameName];

            bool gameFound = false;
            do
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.MainWindowTitle == gameWindowName)
                    {
                        StringBuilder foundClassName = new StringBuilder();
                        GameHandlerApi.GetClassName(process.MainWindowHandle, foundClassName, gameClassName.Length+1);
                        if (foundClassName.ToString() == gameClassName)
                        {
                            memory = new Memory(process);
                            gameFound = true;
                            break;
                        }
                    }
                }
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            } while (gameFound == false && Program._shouldStop == false );

            return;
        }
    }
}
