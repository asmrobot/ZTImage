using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZTImage.Configuration;

namespace ZTImage.SchedulerDaemon
{
    public class PluginEngine
    {
        public PluginEngine()
        {

        }

        private Int32 InitializeState = 0;
        private ISchedulerFactory factory;
        private IScheduler scheduler;
        private SortOutConfigInfo mConfig;

        #region 周期操作
        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            if (Interlocked.CompareExchange(ref InitializeState, 1, 0) == 0)
            {
                LoadAssembly();

                LoadJobs();
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (InitializeState == 1)
            {
                if (!scheduler.IsStarted)
                {
                    scheduler.Start();
                }
            }            
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="waitToComplete"></param>
        public void Stop(bool waitToComplete)
        {
            if (!scheduler.IsShutdown)
            {
                scheduler.Shutdown(waitToComplete);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (scheduler.IsStarted)
            {
                scheduler.PauseAll();
            }
        }

        /// <summary>
        /// 恢复运行
        /// </summary>
        public void Resume()
        {
            scheduler.ResumeAll();
        }
        #endregion

        /// <summary>
        /// 得到任务列表
        /// </summary>
        public void GetJobList()
        {
            
        }


        /// <summary>
        /// 加载程序集
        /// </summary>
        private void LoadAssembly()
        {
            var plugins =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Plugins");
            
            //搜索插件目录下bin目录下的所有*.dll 将这些.dll 文件拷贝到一个缓存目录
            var target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_data", "plugins-cache");
            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }
            Directory.CreateDirectory(target);

            IEnumerable<string> dlls;
            var dirs = Directory.EnumerateDirectories(plugins);
            foreach (var dir in dirs)
            {
                if (Directory.Exists(dir))
                {
                    dlls = Directory.EnumerateFiles(dir, "*.dll");
                    foreach (var dll in dlls)
                    {
                        File.Copy(dll, Path.Combine(target, Path.GetFileName(dll)), true);
                    }
                }
            }

            //加载缓存目录下的.dll，得到 Assembly 对象,注册这些 Assembly为mvc的部分模块            
            dlls = Directory.EnumerateFiles(target, "*.dll");
            foreach (var dll in dlls)
            {
                LoadAssembly(dll);
            }
        }


        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool LoadAssembly(string path)
        {
            try
            {
                Assembly.LoadFile(path);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("未能加载程序集:" + path, ex);
            }
            return false;

        }

        /// <summary>
        /// 加载任务
        /// </summary>
        private void LoadJobs()
        {
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();


            mConfig = ConfigHelper.GetInstance<SortOutConfigInfo>();
            for (int j = 0; j < mConfig.Jobs.Length; j++)
            {
                JobInfo job = mConfig.Jobs[j];
                Type type = null;
                try
                {
                    Assembly asm = GetAssembly(job.JobAssembly);
                    if (asm == null)
                    {
                        throw new ArgumentException("程序集未找到:"+job.JobAssembly);
                    }

                    type = asm.GetType(job.JobType);
                    if (type == null)
                    {
                        throw new ArgumentException("类型未找到:"+job.JobType);
                    }
                }
                catch (Exception ex)
                {
                    ZTImage.Log.Trace.Error("查找类型失败:"+job.JobType);
                    continue;
                }


                try
                {
                    IJobDetail jobDetail = JobBuilder.Create(type).WithIdentity(job.Name, JobKey.DefaultGroup).WithDescription(job.Description).Build();
                    jobDetail.JobDataMap.Put("data", job.Data);

                    ITrigger trigger = TriggerBuilder.Create()
                       .WithIdentity(job.Name+"_trigger", TriggerKey.DefaultGroup)
                       .WithCronSchedule(job.Cron)
                       .Build();

                    scheduler.ScheduleJob(jobDetail, trigger);
                }
                catch (Exception ex)
                {
                    ZTImage.Log.Trace.Error("添加任务调度失败:"+job.Name,ex);
                    continue;
                }
            }
        }


        /// <summary>
        /// 查找程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private Assembly GetAssembly(string assemblyName)
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

        
    }
}
