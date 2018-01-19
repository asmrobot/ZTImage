using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ZTImage.ServiceController;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //xx.exe [-i myservicename|--install myservicename]
            //xx.exe [-u myservicename|--uninstall myservicename]
            ServiceHelper.Run(new RunDemo(), args);

        }
    }
}
