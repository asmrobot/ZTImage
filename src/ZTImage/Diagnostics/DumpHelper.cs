using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ZTImage.Diagnostics
{
    public enum MiniDumpType
    {
        None = 0x00010000,
        Normal = 0x00000000,
        WithDataSegs = 0x00000001,
        WithFullMemory = 0x0000002,
        WithHandleData = 0x00000004,
        FilterMemory = 0x00000008,
        ScanMemory = 0x00000010,
        WithUnloadedModules = 0x00000020,
        WithIndirectlyReferencedMemory = 0x00000040,
        FilterModulePaths = 0x00000080,
        WithProcessThreadData = 0x00000100,
        WithPrivateReadWriteMemory = 0x00000200,
        WithoutOptionalData = 0x00000400,
        WithFullMemoryInfo = 0x00000800,
        WithThreadInfo = 0x00001000,
        WithCodeSegs = 0x00002000

    }

    public class DumpHelper
    {
        [DllImport("DbgHelp.dll")]
        private static extern Boolean MiniDumpWriteDump(
            IntPtr hProcess,
            Int32 processid,
            IntPtr fileHandle,
            MiniDumpType dumpType,
            IntPtr exinfo,
            IntPtr userInfo,
            IntPtr extInfo);

        /// <summary>
        /// dump,C# Invoke
        /// </summary>
        /// <param name="dmpPath"></param>
        /// <param name="dmpType"></param>
        /// <returns></returns>
        public static Boolean TryDump(string dmpPath, MiniDumpType dmpType)
        {
            using (FileStream stream = new FileStream(dmpPath, FileMode.Create))
            {
                Process process = Process.GetCurrentProcess();

                Boolean res = MiniDumpWriteDump(process.Handle,
                    process.Id,
                    stream.SafeFileHandle.DangerousGetHandle(),
                    dmpType,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero);

                stream.Flush();
                stream.Close();
                return res;
            }
        }
    }
}
