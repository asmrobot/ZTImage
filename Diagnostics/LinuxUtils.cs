using System;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace ZTImage.Diagnostics
{
    /// <summary>
    /// linux帮助类
    /// </summary>
    public class LinuxUtils
    {
        private const string DAEMON_TAG = "--daemon.";
        /// <summary>
        /// 守护方式运行
        /// </summary>
        /// <param name="args"></param>
        public static void DaemonRun(ThreadStart thread)
        {
            /*usage
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                bool isDaemon = false;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToUpper() == "--DAEMON")
                    {
                        isDaemon = true;
                        break;
                    }
                }

                if (isDaemon)
                {
                    LinuxUtils.DaemonRun(new ThreadStart(Dowork));
                    return;
                }
            }

            Dowork();
            return;
             */
             
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                return;
            }
            
            // 判断是否已经进入Daemon状态，如果是，就直接执行后台主函数
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(DAEMON_TAG)))
            {
                Environment.SetEnvironmentVariable(DAEMON_TAG, null);
                (new Thread(thread) { IsBackground = true }).Start();
                (new AutoResetEvent(false)).WaitOne();//阻止daemon进程退出
                return;
            }


            // 如果还没有进入daemon状态，就作daemon处理
            int pid = fork();
            if (pid != 0) exit(0);
            setsid();
            pid = fork();
            if (pid != 0) exit(0);
            umask(0);


            // 这儿已经进入“守护进程”工作状态了!关闭所有打开的文件描述符
            int max = open("/dev/null", 0);
            for (var i = 0; i <= max; i++) { close(i); }
            
            // 设置标记，防止重复运行进入
            Environment.SetEnvironmentVariable(DAEMON_TAG, "yes");

            //为execp参数重组参数
            var args = Environment.GetCommandLineArgs().Length <= 1 ? new string[2] : new string[Environment.GetCommandLineArgs().Length + 1];

            args[0] = "ztimage_daemon";
            args[1] = Path.Combine(Environment.CurrentDirectory, Thread.GetDomain().FriendlyName);


            if (args.Length > 2)
            {
                for (var i = 2; i < args.Length; i++)
                {
                    args[i] = Environment.GetCommandLineArgs()[i - 1];
                }
            }

            //守护状态下重新加载和运行本程序
            execvp("mono", args);
        }
        



        #region linux p/invoke

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
