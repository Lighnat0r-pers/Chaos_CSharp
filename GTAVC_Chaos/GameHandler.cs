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
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] // GetClassName
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int MaxCount);
    }

    class GameHandler
    {
        public string name;
        private Memory memory = null;
        public string version;
        public bool gameFound = false;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_name">Game name</param>
        public GameHandler(string _name)
        {
            name = _name;
            memory = OpenProcess();
            if (gameFound)
            {
                version = GetVersion();
            }
        }

        /// <summary>
        /// This function gets all processes. It will then check if any of the processes has the window
        /// name gameWindowName and the window class name gameClassName. If this is the case, it will 
        /// instantiate the Memory class for the (first) matching process to be used for reading and 
        /// writing the memory of the process. In no matching process is found, the method will sleep 
        /// and keep trying until a matching process is found.
        /// </summary>
        private Memory OpenProcess()
        {
            string gameWindowName = Settings.gameWindowNameArray[name];
            string gameClassName = Settings.gameWindowClassNameArray[name];

            Memory _memory = null;

            do
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.MainWindowTitle == gameWindowName)
                    {
                        StringBuilder foundClassName = new StringBuilder();
                        GameHandlerApi.GetClassName(process.MainWindowHandle, foundClassName, gameClassName.Length + 1);
                        if (foundClassName.ToString() == gameClassName)
                        {
                            _memory = new Memory(process);
                            gameFound = true;
                            break;
                        }
                    }
                }
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            } while (gameFound == false && Program._shouldStop == false);

            return _memory;
        }

        public void CloseProcess()
        {
            memory.CloseProcess();
            memory = null;
            version = "";
            gameFound = false;
        }

        private string GetVersion()
        {
            string _version = "";
            switch (name)
            {
                case "GTAVC":
                    byte value = memory.Read<byte>(0x00608578, 1);
                    switch (value)
                    {
                        case 0x5D:
                            _version = "1.0";
                            break;
                        case 0x81:
                            _version = "1.1";
                            break;
                        case 0x5B:
                            _version = "Steam";
                            break;
                        case 0x44:
                            _version = "Japanese";
                            break;
                        default:
                            throw new Exception("Unsupported version of GTA Vice City.");
                    }
                    break;
                default:
                    throw new Exception("Unsupported game.");
            }

            return _version;
        }

        public T Read<T>(long address, int length = 4)
        {
            // TODO(Ligh): Deal with dynamic addresses (in combination with the version offset).

            if (!gameFound)
            {
                throw new Exception("Can't read game memory: Game not found.");
            }

            return memory.Read<T>(getAddressForVersion(address), length);
        }

        public void Write<T>(long address, T input, int length = int.MinValue)
        {
            // TODO(Ligh): Deal with dynamic addresses (in combination with the version offset).

            if (!gameFound)
            {
                throw new Exception("Can't write game memory: Game not found.");
            }

            memory.Write<T>(getAddressForVersion(address), input, length);
        }

        private long getAddressForVersion(long address)
        {

            return address;
        }
    }
}
