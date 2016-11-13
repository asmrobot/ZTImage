using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ZTImage.Diagnostics
{
    public static class CodeTimer
    {
        private static ProcessPriorityClass m_processPriority=ProcessPriorityClass .Normal;
        private static ThreadPriority m_threadPriority=ThreadPriority.Normal;


        static CodeTimer()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Timer("", 1, () => { });

        }

        public static void ResetPriority()
        {
            Process.GetCurrentProcess().PriorityClass = m_processPriority;
            Thread.CurrentThread.Priority = m_threadPriority;
        }

        /// <summary>
        /// 性能查看器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iternator"></param>
        /// <param name="action"></param>
        public static void Timer(string name, Int32 iteration, Action action)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            ConsoleColor foreColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);
            Console.ForegroundColor = foreColor;


            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            UInt64 cpuCycle = GetCycleTime();

            //查询各代已经回收了多少次
            Int32[] gcCount = new Int32[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCount[i] = GC.CollectionCount(i);
            }



            //运行任务
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iteration; i++)
            {
                action();
            }
            watch.Stop();

            Console.WriteLine("\tTime Elapse:\t" + watch.ElapsedMilliseconds.ToString() + "毫秒");
            Console.WriteLine("\tCpu Cycles:\t" + (GetCycleTime() - cpuCycle).ToString());
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                Console.WriteLine("\tGen " + i.ToString() + ":\t\t" + (GC.CollectionCount(i) - gcCount[i]).ToString());
            }
        }


        /// <summary>
        /// 得到线程时钟周期数
        /// </summary>
        /// <returns></returns>
        private static UInt64 GetCycleTime()
        {
            UInt64 cycle = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycle);
            return cycle;
        }



        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
        public static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

    }
}
