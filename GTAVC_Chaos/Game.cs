using AccessProcessMemory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace GTAVC_Chaos
{
    static class ProcessHandlerApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] // GetClassName
        public static extern int GetClassName(IntPtr hwnd, System.Text.StringBuilder lpClassName, int MaxCount);
    }

    class Game
    {
        private long versionAddress;
        private string baseVersion;

        public string name;
        public string abbreviation;
        public string windowName;
        public string windowClass;

        public Memory memory;
        public GameVersion currentVersion;

        public List<GameVersion> gameVersions;
        public List<MemoryAddress> memoryAddresses;
        public List<Limitation> limitations;

        public bool IsRunning
        {
            get { return memory != null && memory.HasValidProcess(); }
        }

        public Game(string name, string abbreviation, string windowName, string windowClass, long versionAddress, string baseVersion, List<GameVersion> gameVersions)
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
            Debug.WriteLine("Starting attempts to get game handle");
            OpenProcess();
            if (memory != null)
            {
                Debug.WriteLine("Game handle found");
                GetVersion();

                foreach (var memoryAddress in memoryAddresses)
                {
                    memoryAddress.UpdateForVersion(currentVersion);
                }
            }
            else
            {
                Debug.WriteLine("Search for game handle aborted");
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
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME);
            }
        }

        public void FreeHandle()
        {
            memory.CloseProcess();
            memory = null;
            currentVersion = null;
        }

        public void GetVersion()
        {
            byte value = memory.Read(versionAddress, "byte", 1);
            currentVersion = gameVersions.Find(v => v.versionAddressValue == value);

            if (currentVersion == null)
            {
                throw new ArgumentOutOfRangeException("version", String.Format("Failed to determine the game version: Unknown version. Version address value was {0}", value));
            }

            Debug.WriteLine(String.Format("Detected game version: {0}", currentVersion.name));
        }

        public void DoModulesLoop()
        {
            GetHandle();

            var modules = new Modules();

            while (this.IsRunning && !Program.shouldStop)
            {
                modules.Update();
            }

            if (this.IsRunning)
            {
                // The modules affect the game state, so if the game is still running,
                // we shut them down to restore the game to its unaltered state.
                modules.Shutdown();
            }

            FreeHandle();
        }

        public MemoryAddress FindMemoryAddressByName(string name)
        {
            return memoryAddresses.Find(p => p.name == name);
        }

        public Limitation FindLimitationByName(string name)
        {
            return limitations.Find(p => p.name == name);
        }

        public void ResolveReferences()
        {
            if (memoryAddresses == null || limitations == null)
            {
                throw new InvalidOperationException("Cannot resolve references before reference objects are initialized.");
            }

            // Set base address for all dynamic addresses.
            foreach (var address in memoryAddresses.FindAll(m => m.IsDynamic == true))
            {
                address.baseAddress = FindMemoryAddressByName(address.baseAddressName);

                if (address.baseAddress == null)
                {
                    throw new ArgumentOutOfRangeException("baseAddress", "Base address for dynamic address is not defined.");
                }
            }

            // Set references for all checks that are not limitation checks.
            foreach (Limitation limitation in limitations)
            {
                foreach (var check in limitation.checks.FindAll(c => (c is LimitationCheck) == false))
                {
                    check.ResolveReferences();
                }
            }

            // Set references for all limitation checks. As they can depend on other checks, they need to be set after those have been resolved.
            // TODO(Ligh): Check how this handles multiple levels of limitation checks.
            foreach (Limitation limitation in limitations)
            {
                foreach (var check in limitation.checks.FindAll(c => c is LimitationCheck))
                {
                    check.ResolveReferences();
                }
            }
        }
    }
}
