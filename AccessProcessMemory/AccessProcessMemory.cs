using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AccessProcessMemory
{
    static class AccessProcessMemoryApi
    {
        public const uint PROCESS_ALL_ACCESS = 0x001F0FFF;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            uint dwDesiredAccess,
            bool bInheritHandle,
            int dwProcessId
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [In, Out] byte[] lpBuffer,
            IntPtr dwSize,
            out IntPtr lpNumberOfBytesRead
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            IntPtr dwSize,
            out IntPtr lpNumberOfBytesWritten
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int CloseHandle(
            IntPtr hObject
            );

    }

    /// <summary>
    /// Class containing methods to read and write the memory of the target process.
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// Initializes a new instance of the Memory class for the specified process.
        /// </summary>
        public Memory(Process process)
        {
            targetProcess = process;
            OpenProcess();
        }

        /// <summary>
        /// Process that the functions in this library will target.
        /// </summary>
        private Process targetProcess;

        /// <summary>
        /// Handle to the targetted process.
        /// </summary>
        private IntPtr m_ProcessHandle = IntPtr.Zero;

        /// <summary>
        /// Whether this class is targetting an existing process.
        /// </summary>
        public bool ValidProcess => targetProcess != null && !targetProcess.HasExited;

        /// <summary>
        /// Gets the process handle for the target process. Needs to be called before accessing the memory.
        /// </summary>
        private void OpenProcess()
        {
            if (m_ProcessHandle == IntPtr.Zero)
            {
                m_ProcessHandle = AccessProcessMemoryApi.OpenProcess(AccessProcessMemoryApi.PROCESS_ALL_ACCESS, true, targetProcess.Id);
                if (m_ProcessHandle == IntPtr.Zero)
                    Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
            }
        }

        /// <summary>
        /// Closes the handle to the process.
        /// </summary>
        public void CloseProcess()
        {
            int iRetValue = AccessProcessMemoryApi.CloseHandle(m_ProcessHandle);
            if (iRetValue == 0)
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
            m_ProcessHandle = IntPtr.Zero;
        }

        /// <summary>
        /// Reads [length] bytes at [address] as [type] in the targeted process.
        /// </summary>
        public dynamic Read(long address, string type, int length)
        {
            var buffer = new byte[length];
            IntPtr ptrBytesReaded;
            AccessProcessMemoryApi.ReadProcessMemory(m_ProcessHandle, (IntPtr)address, buffer, (IntPtr)length, out ptrBytesReaded);
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
            return ConvertOutput(buffer, type);
        }

        /// <summary>
        /// Writes [input] zero-padded up to [length] to [address] in the targeted process.
        /// </summary>
        public void Write(long address, dynamic input, string type, int length = 0)
        {
            byte[] byteInput = ConvertInput(input, type);
            if (length == 0)
            {
                length = byteInput.Length;
            }
            var fullInput = new byte[length];
            Array.Copy(byteInput, fullInput, byteInput.Length);

            IntPtr ptrBytesWritten;
            AccessProcessMemoryApi.WriteProcessMemory(m_ProcessHandle, (IntPtr)address, fullInput, (IntPtr)length, out ptrBytesWritten);
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
        }

        /// <summary>
        /// Converts byte array to the type given by the dataType parameter. Endianness is handled automatically.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="NotSupportedException" />
        private static dynamic ConvertOutput(byte[] output, string dataType)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output), "Error while converting output from memory, no output");

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(output); // Convert little endian to big endian.
            }

            dynamic result;
            switch (dataType)
            {
                case "bool":
                    result = BitConverter.ToBoolean(output, 0);
                    break;
                case "byte":
                    result = output[0];
                    break;
                case "short":
                    result = BitConverter.ToInt16(output, 0);
                    break;
                case "int":
                    result = BitConverter.ToInt32(output, 0);
                    break;
                case "long":
                    result = BitConverter.ToInt64(output, 0);
                    break;
                case "float":
                    result = BitConverter.ToSingle(output, 0);
                    break;
                case "double":
                    result = BitConverter.ToDouble(output, 0);
                    break;
                case "ascii":
                    result = Encoding.ASCII.GetString(output).TrimEnd(('\0'));
                    break;
                case "unicode":
                    result = Encoding.Unicode.GetString(output).TrimEnd(('\0'));
                    break;
                default:
                    throw new NotSupportedException($"Tried to convert memory reading to unknown data type {dataType}");
            }
            return result;
        }

        /// <summary>
        /// Converts the type given by the dataType parameter to a byte array. Endianness is handled automatically.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="NotSupportedException" />
        private static byte[] ConvertInput(dynamic input, string dataType)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Error while converting input for memory, no input");

            byte[] result;
            switch (dataType)
            {
                case "bool":
                    result = BitConverter.GetBytes(input);
                    break;
                case "byte":
                    result = new byte[] { input };
                    break;
                case "short":
                    result = BitConverter.GetBytes(input);
                    break;
                case "int":
                    result = BitConverter.GetBytes(input);
                    break;
                case "long":
                    result = BitConverter.GetBytes(input);
                    break;
                case "float":
                    result = BitConverter.GetBytes(input);
                    break;
                case "double":
                    result = BitConverter.GetBytes(input);
                    break;
                case "ascii":
                    result = Encoding.ASCII.GetBytes(input);
                    break;
                case "unicode":
                    result = Encoding.Unicode.GetBytes(input);
                    break;
                default:
                    throw new NotSupportedException($"Tried to convert memory input to unknown data type {dataType}");
            }

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(result); // Convert little endian to big endian.
            }

            return result;
        }
    }
}
