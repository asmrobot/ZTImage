using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using NLog.Config;

namespace ZTImage.Log
{
    public class NLog:IZTLog
    {
        private ILogger log=null;

        private NLog()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"configs", "nlog.config");
            if (!File.Exists(configPath))
            {
                throw new NLogConfigurationException("配置文件未找到");
            }
            
            LogManager.Configuration = new XmlLoggingConfiguration(configPath);
            log = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public void Debug(string message)
        {
            log.Debug(message);
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public void Info(string message)
        {
            log.Info(message);
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public void Warn(string message)
        {
            log.Warn(message);
        }

        public void Error(string message)
        {
            log.Error(message);
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public void Error(string message, Exception ex)
        {
            log.Error(ex, message);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public void Fatal(string message, Exception ex)
        {
            log.Fatal(ex, message);
        }


        private static NLog mInstance;
        private static object mLockHelper = new object();

        public static NLog Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mLockHelper)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new NLog();
                        }
                    }
                }

                return mInstance;
            }
        }
    }
}
