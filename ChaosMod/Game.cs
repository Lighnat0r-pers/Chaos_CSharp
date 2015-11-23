using AccessProcessMemory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ChaosMod
{
    static class ProcessHandlerApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] // GetClassName
        public static extern int GetClassName(IntPtr hwnd, System.Text.StringBuilder lpClassName, int MaxCount);
    }

    class Game
    {
        private long versionAddress;
        private string name;

        public string abbreviation;
        public string windowName;
        public string windowClass;

        public Memory memory;
        public GameVersion currentVersion;

        public List<GameVersion> gameVersions;
        public List<MemoryAddress> memoryAddresses;
        public List<Limitation> baseLimitations;

        public bool IsRunning => memory != null && memory.HasValidProcess();

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Game(string name, string abbreviation, string windowName, string windowClass, long versionAddress, List<GameVersion> gameVersions)
        {
            this.Name = name;
            this.abbreviation = abbreviation;
            this.windowName = windowName;
            this.windowClass = windowClass;
            this.versionAddress = versionAddress;
            this.gameVersions = gameVersions;
        }

        public void GetHandle()
        {
            Debug.WriteLine("Starting attempts to get game handle.");
            OpenProcess();
            if (memory != null)
            {
                Debug.WriteLine("Game handle found.");
                GetVersion();

                foreach (var memoryAddress in memoryAddresses)
                {
                    memoryAddress.UpdateForVersion(currentVersion);
                }
            }
            else
            {
                Debug.WriteLine("Search for game handle aborted.");
                Thread.CurrentThread.Abort();
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
            while (memory == null && Program.shouldStop == false)
            {
                foreach (var process in Array.FindAll(Process.GetProcesses(), p => p.MainWindowTitle == windowName))
                {
                    var foundClassName = new System.Text.StringBuilder();
                    ProcessHandlerApi.GetClassName(process.MainWindowHandle, foundClassName, windowClass.Length + 1);
                    if (foundClassName.ToString() == windowClass)
                    {
                        memory = new Memory(process);
                        break;
                    }
                }
                Thread.Sleep(Settings.DefaultWaitTime);
            }
        }

        public void FreeHandle()
        {
            memory.CloseProcess();
            memory = null;
            currentVersion = null;

            Debug.WriteLine("Game handle freed.");
        }

        public void GetVersion()
        {
            byte value = memory.Read(versionAddress, "byte", 1);
            currentVersion = gameVersions.Find(v => v.versionAddressValue == value);

            if (currentVersion == null)
            {
                throw new InvalidOperationException($"Failed to determine the game version: Unknown version. Version address value was {value}");
            }

            Debug.WriteLine($"Detected game version: {currentVersion.name}");
        }

        public void StartModulesLoop()
        {
            DataFileHandler.ReadFilesForGame(this);
            ResolveReferences();

            DoModulesLoop();
        }

        public void DoModulesLoop()
        {
            while (!Program.shouldStop)
            {
                GetHandle();

                var modules = new Modules();

                while (IsRunning && !Program.shouldStop)
                {
                    modules.Update();
                }

                modules.Shutdown();

                FreeHandle();
            }
        }

        public MemoryAddress FindMemoryAddressByName(string name)
        {
            return memoryAddresses.Find(p => p.name == name);
        }

        public void ResolveReferences()
        {
            if (memoryAddresses == null)
            {
                throw new InvalidOperationException("Cannot resolve references before reference objects are initialized.");
            }

            // Set base address for all dynamic addresses.
            foreach (var address in memoryAddresses.FindAll(m => m.IsDynamic == true))
            {
                address.baseAddress = FindMemoryAddressByName(address.baseAddressName);

                if (address.baseAddress == null)
                {
                    throw new ArgumentNullException("baseAddress", "Base address for dynamic address is not defined.");
                }
            }
        }
    }
}
