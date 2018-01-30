﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace ZTImage.Services.Schedulers
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract class JobBase : IJob
    {
#if NET45
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                string ds = string.Empty;
                Object data=context.JobDetail.JobDataMap.Get("data");
                if (data != null)
                {
                    ds = data.ToString();
                }
                Execute(ds);
            }
            catch (Exception error)
            {
                ZTImage.Log.Trace.Error("execute error", error);
            }
        }

        public abstract void Execute(string datas);
#else
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                return Execute(string.Empty);
            }
            catch (Exception error)
            {
                ZTImage.Log.Trace.Error("execute error", error);
                return Task.Delay(0);
            }
        }

        public abstract Task Execute(string datas);
#endif



    }
}
