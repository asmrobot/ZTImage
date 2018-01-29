using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
#if NETCOREAPP
using System.Runtime.Loader;
#endif
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZTImage.Configuration;


namespace ZTImage.Services.Schedulers
{
    public class PluginEngine
    {

        static PluginEngine()
        {
            IgnoreDlls.Add("sni.dll".ToUpper(), true);
            IgnoreDlls.Add("NLog.dll".ToUpper(), true);
            IgnoreDlls.Add("Quartz.dll".ToUpper(), true);
            IgnoreDlls.Add("ZTImage.dll".ToUpper(), true);

            IgnoreDlls.Add("ZTImage.Log.dll".ToUpper(), true);
            IgnoreDlls.Add("Common.Logging.dll".ToUpper(), true);
            IgnoreDlls.Add("Common.Logging.Core.dll".ToUpper(), true);

            IgnoreDlls.Add("ZTImage.Services.dll".ToUpper(), true);
            IgnoreDlls.Add("System.Configuration.ConfigurationManager.dll".ToUpper(), true);
            
            IgnoreDlls.Add("System.Data.SqlClient.dll".ToUpper(), true);
            IgnoreDlls.Add("System.Security.Cryptography.ProtectedData.dll".ToUpper(), true);
            IgnoreDlls.Add("System.Text.Encoding.CodePages.dll".ToUpper(), true);

            
        }
        public PluginEngine():this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"))
        {}

        public PluginEngine(string pluginsDir)
        {
            Initialize(pluginsDir);
        }

        private readonly static Dictionary<string, bool> IgnoreDlls = new Dictionary<string, bool>();
        private Int32 InitializeState = 0;
        private ISchedulerFactory factory;
        private IScheduler scheduler;
        private SchedulersConfigInfo mConfig;

        #region 周期操作
        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <returns></returns>
        private void Initialize(string pluginsDir)
        {
            if (Interlocked.CompareExchange(ref InitializeState, 1, 0) == 0)
            {
                FindAssembly(pluginsDir);
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
#if NET45   
        public List<IJobDetail> GetJobList()
#else
        public async Task<List<IJobDetail>> GetJobList()
#endif
        {
            List<IJobDetail> details = new List<IJobDetail>();
#if NET45
            Quartz.Collection.ISet<JobKey> jobs = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
#else
            IReadOnlyCollection<JobKey> jobs = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
#endif
            foreach (var item in jobs)
            {
#if NET45
                IJobDetail detail = scheduler.GetJobDetail(item);
#else
                IJobDetail detail = await scheduler.GetJobDetail(item);
#endif
                details.Add(detail);
            }
            return details;
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        private void FindAssembly(string pluginsDir)
        {
            //搜索插件目录下bin目录下的所有*.dll 将这些.dll 文件拷贝到一个缓存目录
            var target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_data", "plugins-cache");
            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
            }
            Directory.CreateDirectory(target);

            IEnumerable<string> dlls;
            string filename = string.Empty;
            var dirs = Directory.EnumerateDirectories(pluginsDir);
            foreach (var dir in dirs)
            {
                if (Directory.Exists(dir))
                {
                    dlls = Directory.EnumerateFiles(dir, "*.dll");
                    
                    foreach (var dll in dlls)
                    {
                        string dirDetail = dll.Replace("\\", "/");
                        filename = dirDetail.Substring(dirDetail.LastIndexOf('/') + 1);
                        if (IgnoreDlls.ContainsKey(filename.ToUpper()))
                        {
                            continue;
                        }
                        File.Copy(dirDetail, Path.Combine(target, Path.GetFileName(dll)), true);
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
#if NET45
                Assembly.LoadFrom(path);
#else
                AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
#endif
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
#if NET45
        private void LoadJobs()
#else
        private async void LoadJobs()
#endif
        
        {
            factory = new StdSchedulerFactory();
#if NET45
            scheduler = factory.GetScheduler();
#else
            scheduler = await factory.GetScheduler();
#endif


            mConfig = ConfigHelper.GetInstance<SchedulersConfigInfo>();
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
