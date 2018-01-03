using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Log;

namespace DemoFramework
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Trace.EnableListener(NLog.Instance);
            
            Trace.Debug("Debug");
            Trace.Info("Info");
            Trace.Warn("Warn");
            Trace.Error("Error");
            Trace.Fatal("Fatal");

            Console.WriteLine("complete");

            Console.ReadKey();
        }
    }
}
