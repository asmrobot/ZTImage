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

namespace ZTImage.SchedulerDaemon
{
    class Program
    {
        static IScheduler scheduler;
        static ISchedulerFactory factory;
        static Program()
        {
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();
        }

        static void Main(string[] args)
        {
            ZTImage.Log.Trace.EnableConsole();
            //LoadAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DIDI.dll"));

            //加载dll

            var info=ConfigHelper.GetInstance<SortOutConfigInfo>();
            for(int j=0;j<info.Jobs.Length;j++)
            {
                JobInfo job = info.Jobs[j];
                Type type = null;
                try
                {
                    Assembly asm = GetAssembly(job.JobAssembly);
                    if (asm == null)
                    {
                        throw new ArgumentException("程序集未找到");
                    }

                    type = asm.GetType(job.JobType);
                    if (type == null)
                    {
                        throw new ArgumentException("类型未找到");
                    }
                }
                catch(Exception ex)
                {
                    ZTImage.Log.Trace.Error("查找类型失败");
                    continue;
                }


                try
                {
                    IJobDetail jobDetail = JobBuilder.Create(type).WithIdentity(job.Name, JobKey.DefaultGroup).WithDescription(job.Description).Build();
                    jobDetail.JobDataMap.Put("data", job.Data);

                    ITrigger trigger = TriggerBuilder.Create()
                       .WithIdentity("trigger1", TriggerKey.DefaultGroup)
                       .WithCronSchedule(job.Cron)
                       .Build();

                    scheduler.ScheduleJob(jobDetail, trigger);
                }
                catch (Exception ex)
                {
                    ZTImage.Log.Trace.Error("添加任务调度失败");
                    continue;
                }
            }
            
            scheduler.Start();
                        
            

            
            Console.WriteLine("complete");
            Console.ReadKey();
            scheduler.Shutdown(true);
        }

        /// <summary>
        /// 确认加载程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        static Assembly GetAssembly(string assemblyName)
        {
            assemblyName = assemblyName.ToUpper();

            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblys.Length; i++)
            {
                if (assemblys[i].GetName().Name.ToUpper() == assemblyName)
                {
                    return assemblys[i];
                }
            }
            
            return null;
        }


        static bool LoadAssembly(string path)
        {
            try
            {
                Assembly.LoadFile(path);
            }
            catch(Exception ex)
            {
                ZTImage.Log.Trace.Error("未能加载程序集:" + path,ex);
            }
            return false;
            
        }
        
    }
}
