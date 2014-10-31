using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AccessProcessMemory
{
    class AccessProcessMemoryApi
    {
        public const UInt32 PROCESS_ALL_ACCESS = 0x001F0FFF;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            UInt32 dwProcessId
            );

        public static extern Int32 ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [In, Out] byte[] buffer,
            UInt32 size,
            out IntPtr lpNumberOfBytesRead
            );

        public static extern Int32 WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] buffer,
            UInt32 size,
            out IntPtr lpNumberOfBytesWritten
            );


        public static extern Int32 CloseHandle(
            IntPtr hObject
            );


    }

    /// <summary>
    /// Class containing methods to read and write to the memory of the targetProcess.
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// Constructor to initiate Memory class without a target process.
        /// </summary>
        public Memory()
        {

        }

        /// <summary>
        /// Constructor to initiate Memory class with a target process.
        /// </summary>
        public Memory(Process process)
        {
            targetProcess = process;
        }

        /// <summary>
        /// Process that the functions in this library will target. The handle to the process 
        /// is automatically gotten when this is set.
        /// </summary>
        public Process targetProcess
        {
            get
            {
                return m_Process;
            }
            set
            {
                m_Process = value;
                OpenProcess();
            }
        }

        // Private vars containing the target process and its handle.
        private Process m_Process = null;
        private IntPtr m_ProcessHandle = IntPtr.Zero;

        /// <summary>
        /// Gets the process handle for m_Process. Automatically called when the targetProcess
        /// property is set.
        /// </summary>
        void OpenProcess()
        {
            m_ProcessHandle = AccessProcessMemoryApi.OpenProcess(AccessProcessMemoryApi.PROCESS_ALL_ACCESS, 0, (uint)m_Process.Id);
            if (m_ProcessHandle == IntPtr.Zero)
                throw new Exception("OpenProcess failed");
        }

        /// <summary>
        /// Close the handle to the process.
        /// </summary>
        public void CloseProcess()
        {
            int iRetValue = AccessProcessMemoryApi.CloseHandle(m_ProcessHandle);
            if (iRetValue == 0)
                throw new Exception("CloseProcess failed");
        }

        /// <summary>
        /// Read [length] bytes at [address] in the current targetProcess. The byte array is then 
        /// converted to type [T] before being returned. length defaults to 4.
        /// </summary>
        public T Read<T>(int address, uint length = 4)
        {
            byte[] buffer = new byte[length];
            IntPtr ptrBytesReaded;
            AccessProcessMemoryApi.ReadProcessMemory(m_ProcessHandle, (IntPtr)address, buffer, length, out ptrBytesReaded);
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred

            T result = ConvertOutput<T>(buffer);
            return result;
        }

        /// <summary>
        /// Convert the input of type T to a byte array of [length]. If the input is shorter, the rest of the 
        /// array will be filled with zeros. Length defaults to the length of the input after conversion
        /// to byte array. Write the fullInput byte array to [address] in the current targetProcess.
        /// </summary>
        public void Write<T>(int address, T input, uint length = uint.MinValue)
        {
            string dataType = typeof(T).Name;
            byte[] byteInput = ConvertInput<T>(input);
            if (length == uint.MinValue)
                length = (uint)byteInput.Length;
            byte[] fullInput = new byte[length];
            Array.Copy(byteInput, fullInput, byteInput.Length);

            IntPtr ptrBytesWritten;
            AccessProcessMemoryApi.WriteProcessMemory(m_ProcessHandle, (IntPtr)address, fullInput, length, out ptrBytesWritten);
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error()); // Throw exception if error occurred
        }

        /// <summary>
        /// Convert byte array to the type given by the targetDataType parameter. If the parameter
        /// contains an unimplemented type an exception is thrown. The byte array is automatically 
        /// converted to little endian if necessary.
        /// </summary>
        private T ConvertOutput<T>(byte[] output)
        {
            string targetDataType = typeof(T).Name;
            if (BitConverter.IsLittleEndian)
                Array.Reverse(output); // Convert big endian to little endian.

            dynamic result;
            switch (targetDataType)
            {
                case "bool":
                    result = BitConverter.ToBoolean(output, 0);
                    break;
                case "int":
                    result = BitConverter.ToInt32(output, 0);
                    break;
                case "float":
                    result = BitConverter.ToSingle(output, 0);
                    break;
                case "double":
                    result = BitConverter.ToDouble(output, 0);
                    break;
                case "ascii":
                    result = Encoding.ASCII.GetString(output);
                    break;
                case "unicode":
                    result = Encoding.Unicode.GetString(output);
                    break;
                default:
                    throw new Exception(String.Format("Tried to convert memory reading to unknown dataType {0}", targetDataType));
            }
            return result;
        }

        /// <summary>
        /// Convert the type given by the targetDataType parameter to a byte array. If the parameter
        /// contains an unimplemented type an exception is thrown. The byte array is automatically 
        /// converted to little endian if necessary.
        /// </summary>
        byte[] ConvertInput<T>(dynamic input)
        {
            string originalDataType = typeof(T).Name;
            byte[] result;
            switch (originalDataType)
            {
                case "bool":
                    result = BitConverter.GetBytes(input);
                    break;
                case "int":
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
                    throw new Exception(String.Format("Tried to convert memory reading to unknown dataType {0}", originalDataType));
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result); // Convert big endian to little endian.

            return result;
        }
    }
}
