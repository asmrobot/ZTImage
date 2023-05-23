using System;
using System.Collections.Generic;

using System.Linq;
using ZTImage.DbLite;
using System.Data;
using Demo;

namespace ZTImage.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoUnit demo = new TaskQueueDemo();
            demo.Do();
            new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
