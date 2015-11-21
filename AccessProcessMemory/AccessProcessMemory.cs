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
        /// Constructor to initiate Memory class with a target process.
        /// </summary>
        public Memory(Process process)
        {
            targetProcess = process;
        }

        /// <summary>
        /// Process that the functions in this library will target.
        /// </summary>
        public Process targetProcess
        {
            get { return m_Process; }
            set { m_Process = value; }
        }

        // Private vars containing the target process and its handle.
        private Process m_Process = null;
        private IntPtr m_ProcessHandle = IntPtr.Zero;

        /// <summary>
        /// Gets the process handle for m_Process. Needs to be called before accessing the memory.
        /// </summary>
        public void OpenProcess()
        {
            if (m_ProcessHandle == IntPtr.Zero)
            {
                m_ProcessHandle = AccessProcessMemoryApi.OpenProcess(AccessProcessMemoryApi.PROCESS_ALL_ACCESS, true, m_Process.Id);
                if (m_ProcessHandle == IntPtr.Zero)
                    Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
            }
        }

        /// <summary>
        /// Close the handle to the process.
        /// </summary>
        public void CloseProcess()
        {
            int iRetValue = AccessProcessMemoryApi.CloseHandle(m_ProcessHandle);
            if (iRetValue == 0)
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
            m_ProcessHandle = IntPtr.Zero;
        }

        public bool HasValidProcess()
        {
            return m_Process != null && !m_Process.HasExited;
        }

        /// <summary>
        /// Read [length] bytes at [address] in the current targetProcess. The byte array is then 
        /// cast to type before being returned.
        /// </summary>
        public dynamic Read(long address, string type, int length)
        {
            OpenProcess();
            var buffer = new byte[length];
            IntPtr ptrBytesReaded;
            AccessProcessMemoryApi.ReadProcessMemory(m_ProcessHandle, (IntPtr)address, buffer, (IntPtr)length, out ptrBytesReaded);
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
            return ConvertOutput(buffer, type);
        }

        /// <summary>
        /// Converts the input of the type specified in type to a byte array of [length]. If the input is shorter,
        /// the rest of the array will be filled with zeros. Length defaults to the length of the input after conversion
        /// to byte array. Writes the fullInput byte array to [address] in the current targetProcess.
        /// </summary>
        public void Write(long address, dynamic input, string type, int length = int.MinValue)
        {
            OpenProcess();
            byte[] byteInput = ConvertInput(input, type);
            if (length == int.MinValue)
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
        /// Converts byte array to the type given by the dataType parameter. If the parameter
        /// contains an unimplemented type an exception is thrown. The byte array is automatically 
        /// converted to big endian if necessary.
        /// </summary>
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
        /// Converts the type given by the dataType parameter to a byte array. If the parameter
        /// contains an unimplemented type an exception is thrown. The byte array is automatically 
        /// converted to big endian if necessary.
        /// </summary>
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
