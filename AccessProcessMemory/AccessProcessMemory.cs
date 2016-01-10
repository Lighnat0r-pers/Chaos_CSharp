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
    /// Data types supported for memory reads and writes.
    /// </summary>
    public enum DataType
    {
        Bool,
        Byte,
        Short,
        Int,
        Long,
        Float,
        Double,
        Ascii,
        Unicode,
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
        public dynamic Read(long address, DataType type, int length)
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
        public void Write(long address, dynamic input, DataType type, int length = 0)
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
        private static dynamic ConvertOutput(byte[] output, DataType dataType)
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
                case DataType.Bool:
                    result = BitConverter.ToBoolean(output, 0);
                    break;
                case DataType.Byte:
                    result = output[0];
                    break;
                case DataType.Short:
                    result = BitConverter.ToInt16(output, 0);
                    break;
                case DataType.Int:
                    result = BitConverter.ToInt32(output, 0);
                    break;
                case DataType.Long:
                    result = BitConverter.ToInt64(output, 0);
                    break;
                case DataType.Float:
                    result = BitConverter.ToSingle(output, 0);
                    break;
                case DataType.Double:
                    result = BitConverter.ToDouble(output, 0);
                    break;
                case DataType.Ascii:
                    result = Encoding.ASCII.GetString(output).TrimEnd(('\0'));
                    break;
                case DataType.Unicode:
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
        private static byte[] ConvertInput(dynamic input, DataType dataType)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Error while converting input for memory, no input");

            byte[] result;
            switch (dataType)
            {
                case DataType.Bool:
                    result = BitConverter.GetBytes(input);
                    break;
                case DataType.Byte:
                    result = new byte[] { input };
                    break;
                case DataType.Short:
                    result = BitConverter.GetBytes(input);
                    break;
                case DataType.Int:
                    result = BitConverter.GetBytes(input);
                    break;
                case DataType.Long:
                    result = BitConverter.GetBytes(input);
                    break;
                case DataType.Float:
                    result = BitConverter.GetBytes(input);
                    break;
                case DataType.Double:
                    result = BitConverter.GetBytes(input);
                    break;
                case DataType.Ascii:
                    result = Encoding.ASCII.GetBytes(input);
                    break;
                case DataType.Unicode:
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
