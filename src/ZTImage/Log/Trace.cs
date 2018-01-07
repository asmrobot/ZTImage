using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZTImage.Log
{
    public class Trace
    {
        private static IZTLog log=null;


        /// <summary>
        /// 初始化日志记录器
        /// </summary>
        /// <param name="listener"></param>
        public static void EnableListener(IZTLog listener)
        {
            log = listener;
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public static void Debug(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Debug(message);
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public static void Info(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Info(message);
        }

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public static void Warn(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Warn(message);
        }

        public static void Error(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Error(message);
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public static void Error(string message, Exception ex)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Error( message, ex);
        }

        public static void Fatal(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Fatal(message);
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        public static void Fatal(string message, Exception ex)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (log == null)
            {
                return;
            }
            log.Fatal(message,ex);
        }
    }
}
