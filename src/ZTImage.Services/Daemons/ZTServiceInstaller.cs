#if NET45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Configuration.Install;


namespace ZTImage.Services.Daemons
{
    public class ZTServiceInstaller
    {
        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void Install(string serviceName)
        {
            CreateInstaller(serviceName).Install(new System.Collections.Hashtable());
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void Uninstall(string serviceName)
        {
            CreateInstaller(serviceName).Uninstall(null);
        }

        private static Installer CreateInstaller(string serviceName)
        {
            var installer = new TransactedInstaller();
            installer.Installers.Add(new ServiceInstaller
            {
                ServiceName = serviceName,
                DisplayName = serviceName,
                StartType = ServiceStartMode.Automatic
            });

            installer.Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });

            var installContext = new InstallContext(serviceName + ".install.log", null);
            installContext.Parameters["assemblypath"] = System.Reflection.Assembly.GetEntryAssembly().Location;
            installer.Context = installContext;
            return installer;
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void Start(string serviceName)
        {
            using (System.ServiceProcess.ServiceController control = new System.ServiceProcess.ServiceController(serviceName))
            {
                if (control.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void Stop(string serviceName)
        {
            using (System.ServiceProcess.ServiceController control = new System.ServiceProcess.ServiceController(serviceName))
            {
                if (control.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }

        /// <summary>
        /// 得到服务状态
        /// Stopped = 1,
        /// StartPending = 2,
        /// StopPending = 3,
        /// Running = 4,
        /// ContinuePending = 5,        
        /// PausePending = 6,
        /// Paused = 7
        /// </summary>
        /// <param name="serviceName"></param>
        public static int GetServiceStatus(string serviceName)
        {
            using (System.ServiceProcess.ServiceController control = new System.ServiceProcess.ServiceController(serviceName))
            {
                return (int)control.Status;
            }
        }
    }
}
#endif