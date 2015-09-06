using AccessProcessMemory;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GTAVC_Chaos
{
    static class ProcessHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] // GetClassName
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int MaxCount);
    }

    class Game
    {
        private long versionAddress;
        private string baseVersion;

        public string name;
        public string abbreviation;
        public string windowName;
        public string windowClass;

        public bool hasHandle = false;
        public Memory memory;
        public GameVersion currentVersion;

        public GameVersion[] gameVersions;

        public Game(string name, string abbreviation, string windowName, string windowClass, long versionAddress, string baseVersion, GameVersion[] gameVersions)
        {
            this.name = name;
            this.abbreviation = abbreviation;
            this.windowName = windowName;
            this.windowClass = windowClass;
            this.versionAddress = versionAddress;
            this.baseVersion = baseVersion;
            this.gameVersions = gameVersions;
        }

        public void GetHandle()
        {
            OpenProcess();
            InitVersion();
        }

        /// <summary>
        /// This function gets all processes. It will then check if any of the processes has the window
        /// name gameWindowName and the window class name gameClassName. If this is the case, it will 
        /// instantiate the Memory class for the (first) matching process to be used for reading and 
        /// writing the memory of the process. In no matching process is found, the method will sleep 
        /// and keep trying until a matching process is found.
        /// </summary>
        private void OpenProcess()
        {
            do
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.MainWindowTitle == windowName)
                    {
                        StringBuilder foundClassName = new StringBuilder();
                        ProcessHandler.GetClassName(process.MainWindowHandle, foundClassName, windowClass.Length + 1);
                        if (foundClassName.ToString() == windowClass)
                        {
                            memory = new Memory(process);
                            hasHandle = true;
                            break;
                        }
                    }
                }
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            } while (hasHandle == false && Program.shouldStop == false);
        }

        public void CloseProcess()
        {
            memory.CloseProcess();
            memory = null;
            currentVersion = null;
            hasHandle = false;
        }

        public void InitVersion()
        {
            if (memory == null)
            {
                throw new Exception("Tried to determine the version without a handle to the game.");
            }

            byte value = memory.Read(versionAddress, "byte", 1);

            foreach (GameVersion gameVersion in gameVersions)
            {
                if (gameVersion.addressValue == value)
                {
                    currentVersion = gameVersion;
                    break;
                }
            }

            if (currentVersion == null)
            {
                throw new Exception("Failed to determine the game version: Unknown version. Version address value was " + value);
            }
        }

        public dynamic Read(MemoryAddress address)
        {
            // TODO(Ligh): Deal with dynamic addresses (in combination with the version offset).

            if (memory == null)
            {
                throw new Exception("Tried to read the game memory without a handle to the game.");
            }

            return memory.Read(getAddressForVersion(address.address), address.type, address.size);
        }

        public void Write(MemoryAddress address, dynamic input)
        {
            // TODO(Ligh): Deal with dynamic addresses (in combination with the version offset).

            if (memory == null)
            {
                throw new Exception("Tried to write to the game memory without a handle to the game.");
            }

            memory.Write(getAddressForVersion(address.address), input, address.type, address.size);
        }

        private long getAddressForVersion(long address)
        {
            if (currentVersion == null)
            {
                throw new Exception("Tried to get address for version but version is not defined.");
            }

            return currentVersion.GetAddressForVersion(address);
        }
    }
}
