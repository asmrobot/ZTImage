using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Services.Schedulers;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ZTImage.Log.Trace.EnableListener(ZTImage.Log.NLog.Instance);
            //xx.exe [-i myservicename|--install myservicename]
            //xx.exe [-u myservicename|--uninstall myservicename]

            PluginEngine engine = new PluginEngine();
            engine.Start();

            ZTImage.Log.Trace.Info("engine complete");
            new System.Threading.ManualResetEvent(false).WaitOne();
            engine.Stop(true);
        }
    }
}
