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
        private string windowName;
        private string windowClass;
        private long versionAddress;
        private GameVersion currentVersion;
        private List<MemoryAddress> memoryAddresses;

        public List<BaseCheck> BaseChecks { get; private set; }
        public string Abbreviation { get; private set; }
        public Memory Memory { get; private set; }
        public List<GameVersion> Versions { get; private set; }
        public string Name { get; private set; }

        private bool IsRunning => (bool)Memory?.ValidProcess;

        public Game(string name, string abbreviation, string windowName, string windowClass, long versionAddress, List<GameVersion> gameVersions)
        {
            Name = name;
            Abbreviation = abbreviation;
            Versions = gameVersions;

            this.windowName = windowName;
            this.windowClass = windowClass;
            this.versionAddress = versionAddress;
        }

        private void GetHandle()
        {
            Debug.WriteLine("Starting attempts to get game handle.");
            OpenProcess();
            if (Memory != null)
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
            while (Memory == null && Program.ShouldStop == false)
            {
                foreach (var process in Array.FindAll(Process.GetProcesses(), p => p.MainWindowTitle == windowName))
                {
                    var foundClassName = new System.Text.StringBuilder();
                    ProcessHandlerApi.GetClassName(process.MainWindowHandle, foundClassName, windowClass.Length + 1);
                    if (foundClassName.ToString() == windowClass)
                    {
                        Memory = new Memory(process);
                        break;
                    }
                }
                Thread.Sleep(250);
            }
        }

        private void FreeHandle()
        {
            Memory.CloseProcess();
            Memory = null;
            currentVersion = null;

            Debug.WriteLine("Game handle freed.");
        }

        private void GetVersion()
        {
            byte value = Memory.Read(versionAddress, "byte", 1);
            currentVersion = Versions.Find(v => v.versionAddressValue == value);

            if (currentVersion == null)
            {
                throw new InvalidOperationException($"Failed to determine the game version: Unknown version. Version address value was {value}");
            }

            Debug.WriteLine($"Detected game version: {currentVersion.name}");
        }

        public void StartModulesLoop()
        {
            ReadFiles();
            ResolveReferences();

            DoModulesLoop();
        }

        private void ReadFiles()
        {
            Debug.WriteLine("Starting to read files for game.");

            memoryAddresses = DataFileHandler.ReadMemoryAddresses(this);
            BaseChecks = DataFileHandler.ReadBaseChecks(this);

            Debug.WriteLine("Done reading files for game.");
        }

        private void DoModulesLoop()
        {
            while (!Program.ShouldStop)
            {
                GetHandle();

                var modules = new Modules();

                while (IsRunning && !Program.ShouldStop)
                {
                    modules.Update();
                    //DebugReadAddresses();
                }

                modules.Shutdown();

                FreeHandle();
            }
        }

        public MemoryAddress FindMemoryAddressByName(string name)
        {
            return memoryAddresses.Find(p => p.Name == name);
        }

        private void ResolveReferences()
        {
            if (memoryAddresses == null)
            {
                throw new InvalidOperationException("Cannot resolve references before reference objects are initialized.");
            }

            // Set base address for all dynamic addresses.
            foreach (var address in memoryAddresses.FindAll(m => m.IsDynamic == true))
            {
                address.BaseAddress = FindMemoryAddressByName(address.BaseAddressName);

                if (address.BaseAddress == null)
                {
                    throw new ArgumentNullException("baseAddress", "Base address for dynamic address is not defined.");
                }
            }
        }

        private void DebugReadAddresses()
        {
            foreach (MemoryAddress address in memoryAddresses)
            {
                Debug.WriteLine($"Address: {address.Name}, Value: {address.Read() as object}");
            }
        }

    }
}
