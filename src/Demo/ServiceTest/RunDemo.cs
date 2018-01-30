using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Services.Schedulers;

namespace ServiceTest
{
    public class RunDemo:JobBase
    {
        public override void Execute(string datas)
        {
            ZTImage.Log.Trace.Info("runing,data:"+datas);
        }
    }
}
