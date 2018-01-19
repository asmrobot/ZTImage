using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.ServiceController
{
    public class ServiceHelper
    {
        public static void Run(IServiceAction service, params string[] args)
        {
            
            if (Environment.UserInteractive)
            {
                string serviceName = string.Empty;
                if (ContainsInstall(args, out serviceName))
                {
                    RunAsAdministrator(args);
                    ZTImage.Log.Trace.Info(string.Format("installing {0}", serviceName));
                    ZTServiceInstaller.Install(serviceName);
                    ZTImage.Log.Trace.Info(string.Format("starting {0}", serviceName));
                    ZTServiceInstaller.Start(serviceName);
                    ZTImage.Log.Trace.Info("install&start ok");
                }
                else if (ContainsUninstall(args, out serviceName))
                {
                    RunAsAdministrator(args);
                    ZTServiceInstaller.Uninstall(serviceName);
                    ZTImage.Log.Trace.Info("uninstall ok");
                }
                else
                {
                    service.Start();
                    new System.Threading.ManualResetEvent(false).WaitOne();
                }
            }
            else
            {
                ServiceBase.Run(new ZTServiceBase(service));
            }
        }



        private static bool ContainsInstall(string[] args, out string serviceName)
        {
            return GetParameter(args, "-i", "--install", out serviceName);
        }


        private static bool ContainsUninstall(string[] args, out string serviceName)
        {
            return GetParameter(args, "-u", "--uninstall", out serviceName);
        }


        private static bool GetParameter(string[] args, string key1,string key2, out string serviceName)
        {
            serviceName = string.Empty;
            string cmd = string.Empty;
            key1 = key1.ToUpper();
            key2 = key2.ToUpper();
            for (int i = 0; i < args.Length; i++)
            {
                cmd = args[i].Trim().ToUpper();
                if (cmd == key1 || cmd == key2)
                {
                    //得到serviceName
                    if (i == args.Length - 1)
                    {
                        return false;
                    }
                    serviceName = args[i + 1];
                    if (serviceName.StartsWith("-"))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }


        private static void RunAsAdministrator(string[] args)
        {
            //以管理员身份运行
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (!principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                info.FileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                info.Arguments = String.Join(" ", args);
                info.Verb = "runas";
                System.Diagnostics.Process.Start(info);
                Environment.Exit(0);
            }
        }
    }
}
