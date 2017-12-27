using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace ZTImage.SchedulerDaemon
{
    public class HelloJob:IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("datetime:" + DateTime.Now.ToString());
        }

       
    }
}
