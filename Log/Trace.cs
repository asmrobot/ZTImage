using System;
using System.Collections.Generic;

namespace ZTImage.Log
{
    /// <summary>
    /// trace
    /// </summary>
    static public class Trace
    {
        private static List<ITraceListener> _list = new List<ITraceListener>();

        /// <summary>
        /// 启用输出到控制台
        /// </summary>
        static public void EnableConsole(LogLevel level=LogLevel.DEBUG)
        {
            _list.Add(new ConsoleListener(level));
        }
        /// <summary>
        /// 启用输出到诊断程序
        /// </summary>
        static public void EnableDiagnostic(LogLevel level = LogLevel.DEBUG)
        {
            _list.Add(new DiagnosticListener(level));
        }

        /// <summary>
        /// 启动输出到文件
        /// </summary>
        static public void EnableFile(LogLevel level = LogLevel.DEBUG,LogSplitType splitType=LogSplitType.Day)
        {
            _list.Add(new FileListener(level,splitType));
        }

        /// <summary>
        /// add listener
        /// </summary>
        /// <param name="listener"></param>
        /// <exception cref="ArgumentNullException">listener is null</exception>
        static public void AddListener(ITraceListener listener)
        {
            if (listener == null) throw new ArgumentNullException("listener");
            _list.Add(listener);
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        static public void Debug(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _list.ForEach(c => c.Debug(message));
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        static public void Info(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _list.ForEach(c => c.Info(message));
        }

        public static void Error(string message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _list.ForEach(c => c.Error(message));
        }
        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        static public void Error(string message, Exception ex)
        {
            if (message == null) throw new ArgumentNullException("message");
            _list.ForEach(c => c.Error(message, ex));
        }
    }
}