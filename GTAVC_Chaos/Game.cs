using AccessProcessMemory;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GTAVC_Chaos
{
    static class ProcessHandlerApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] // GetClassName
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int MaxCount);
    }

    class Game
    {
        private static Random debugRandom;

        private long versionAddress;
        private string baseVersion;

        public string name;
        public string abbreviation;
        public string windowName;
        public string windowClass;

        public bool hasHandle = false;
        public Memory memory;
        public GameVersion currentVersion;

        private GameVersion[] gameVersions;
        private MemoryAddress[] memoryAddresses;
        private Limitation[] limitations;

        public TimedEffect[] timedEffects;
        public PermanentEffect[] permanentEffects;
        public StaticEffect[] staticEffects;

        public Game(string name, string abbreviation, string windowName, string windowClass, long versionAddress, string baseVersion, GameVersion[] gameVersions)
        {
            this.name = name;
            this.abbreviation = abbreviation;
            this.windowName = windowName;
            this.windowClass = windowClass;
            this.versionAddress = versionAddress;
            this.baseVersion = baseVersion;
            this.gameVersions = gameVersions;
            Game.debugRandom = new Random(1234);
        }

        public TimedEffect DebugGetTimedEffect()
        {
            int index = debugRandom.Next(timedEffects.Length);
            index = 3;
            return timedEffects[index];
        }

        public void SetMemoryAddresses(MemoryAddress[] memoryAddresses, string oldVersionName)
        {
            this.memoryAddresses = memoryAddresses;

            GameVersion oldVersion = FindGameVersionByName(oldVersionName);

            for (int i = 0; i < this.memoryAddresses.Length; i++)
            {
                this.memoryAddresses[i].SetMemoryHandle(memory);

                if (this.memoryAddresses[i].address != 0)
                {
                    // NOTE(Ligh): Static address
                    this.memoryAddresses[i].UpdateForVersion(currentVersion, oldVersion);
                }
                else
                {
                    // NOTE(Ligh): Dynamic address
                    this.memoryAddresses[i].ResolveBaseAddress();
                }
            }
        }

        public void SetLimitations(Limitation[] limitations)
        {
            this.limitations = limitations;

            foreach (Limitation limitation in limitations)
            {
                for (int i = 0; i < limitation.checks.Length; i++)
                {
                    if (limitation.checks[i] is LimitationCheck)
                    {
                        (limitation.checks[i] as LimitationCheck).ResolveLimitation();
                    }
                }
            }

        }

        public void GetHandle()
        {
            OpenProcess();
            if (hasHandle)
            {
                InitVersion();
            }
        }

        /// <summary>
        /// This function gets all processes. It will then check if any of the processes has the right
        /// window name and window class. If this is the case, it will instantiate the Memory class for
        /// the (first) matching process to be used for reading and writing the memory of the process.
        /// If no matching process is found, the method will sleep and keep trying until a matching process is found.
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
                        ProcessHandlerApi.GetClassName(process.MainWindowHandle, foundClassName, windowClass.Length + 1);
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
                    Debug.WriteLine("Detected game version: " + gameVersion.name);
                    currentVersion = gameVersion;
                    break;
                }
            }

            if (currentVersion == null)
            {
                throw new Exception("Failed to determine the game version: Unknown version. Version address value was " + value);
            }
        }

        public MemoryAddress FindMemoryAddressByName(string name)
        {
            // TODO(Ligh): Handle not found case.
            return Array.Find(memoryAddresses, p => p.name == name);
        }

        public Limitation FindLimitationByName(string name)
        {
            // TODO(Ligh): Handle not found case.
            return Array.Find(limitations, p => p.name == name);
        }

        public GameVersion FindGameVersionByName(string name)
        {
            // TODO(Ligh): Handle not found case.
            return Array.Find(gameVersions, p => p.name == name);
        }
    }
}
