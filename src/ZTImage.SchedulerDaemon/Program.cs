using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using ZTImage.Configuration;
using System.Threading;

namespace ZTImage.SchedulerDaemon
{
    class Program
    {
        static PluginEngine mEngine;

        static void Main(string[] args)
        {
            ZTImage.Log.Trace.EnableConsole();
            
            mEngine = new PluginEngine();
            mEngine.Initialize();

            mEngine.Start();
            ThreadPool.QueueUserWorkItem(Do);

            Console.WriteLine("starting");
            Console.ReadKey();
            mEngine.Stop(true);


            
            new System.Threading.ManualResetEvent(true).WaitOne();
        }


        static void Do(object val)
        {
            Thread.Sleep(3000);

            mEngine.GetJobList();
        }
        
        
    }
}
