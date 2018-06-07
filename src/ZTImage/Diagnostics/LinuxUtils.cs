using System;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Reflection;

namespace ZTImage.Diagnostics
{
    /// <summary>
    /// linux帮助类
    /// </summary>
    public class LinuxUtils
    {
        /// <summary>
        /// 以linux守护进程的方式运行,再启动时给进程添加parameterTag参数
        /// </summary>
        /// <param name="args"></param>
        /// <param name="parameterTag"></param>
        public static void DaemonRun(string[] args, string parameterTag)
        {
            // 如果还没有进入daemon状态，就作daemon处理 
            int pid = fork();
            if (pid != 0)
            {
                exit(0);
            }
            setsid();
            pid = fork();
            if (pid != 0)
            {
                exit(0);
            }

            umask(0);


            //这儿已经进入“守护进程”工作状态了,关闭所有打开的文件描述符
            int max = open("/dev/null", 0);
            for (var i = 0; i <= max; i++) { close(i); }
            

            //为execp参数重组参数
            var args1 = args == null ? new string[4] : new string[args.Length + 4];

            args1[0] = "dotnet";
            args1[1] = Path.Combine(Environment.CurrentDirectory, Assembly.GetEntryAssembly().GetName().Name + ".dll");

            if (args1.Length > 2)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    args1[i + 2] = args[i];
                }
            }

            args1[args1.Length - 2] = parameterTag;
            args1[args1.Length - 1] = null;

            //守护状态下重新加载和运行本程序
            execvp("dotnet", args1);
        }


        #region P/Invoke
        [DllImport("libc", SetLastError = true)]
        static extern int fork();


        [DllImport("libc", SetLastError = true)]
        static extern int setsid();

        [DllImport("libc", SetLastError = true)]
        static extern int umask(int mask);

        [DllImport("libc", SetLastError = true)]
        static extern int open([MarshalAs(UnmanagedType.LPStr)]string pathname, int flags);

        [DllImport("libc", SetLastError = true)]
        static extern int close(int fd);

        [DllImport("libc", SetLastError = true)]
        static extern int exit(int code);

        [DllImport("libc", SetLastError = true)]
        static extern int execvp([MarshalAs(UnmanagedType.LPStr)]string file, string[] argv);
        #endregion
    }
}
