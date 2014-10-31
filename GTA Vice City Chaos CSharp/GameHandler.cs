using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using AccessProcessMemory;

namespace GTAVC_Chaos
{
    [DllImport("user32.dll")] // GetClassName

    static class GameHandler
    {
        //static public Process gameProcess;
        static public Memory memory = null;

        // External function that outputs window class name for a given handle.
        static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int MaxCount);

        /// <summary>
        /// This function gets all processes with the name in gameName. It will then check if any of
        /// those processes has the window class name in gameClassName. If this is the case, it will 
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
                Process[] processes = Process.GetProcessesByName(gameName);
                foreach (Process process in processes)
                {
                    StringBuilder foundClassName = new StringBuilder();
                    GetClassName(process.Handle, foundClassName, gameClassName.Length);
                    if (foundClassName.ToString() == gameClassName)
                    {
                        memory = new Memory(process);
                        gameFound = true;
                        break;
                    }
                }
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            } while (gameFound == false);

            return;
        }
    }
}
