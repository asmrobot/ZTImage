using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.ServiceController;

namespace ServiceTest
{
    public class RunDemo:IServiceAction
    {
        public void Start()
        {
            ZTImage.Log.Trace.Error("Start");
        }

        public void Stop()
        {
            ZTImage.Log.Trace.Error("Stop");
        }

        public void Pause()
        {
            ZTImage.Log.Trace.Error("Pause");
        }

        public void Continue()
        {
            ZTImage.Log.Trace.Error("Continue");
        }
    }
}
