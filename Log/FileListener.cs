using System;
using System.IO;

namespace ZTImage.Log
{
    /// <summary>
    /// console trace listener
    /// </summary>
    public sealed class FileListener : ITraceListener
    {
        private LogSplitType _LogSplitType;
        private LogLevel _LogLevel;

        public FileListener(LogLevel level,LogSplitType splitType)
        {
            this._LogLevel = level;
            this._LogSplitType = splitType;
        }

        private object mLockerHelper = new object();
        private void WriteString(string message)
        {
            lock (mLockerHelper)
            {
                StreamWriter writer = new StreamWriter(EnsureStream());
                writer.Write("[");
                writer.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.Write("]");
                writer.Write(message);
                writer.Flush();
            }
        }

        private FileStream lastStream;
        private string lastDir;
        private FileStream EnsureStream()
        {
            string fileName = GetFileName();
            try
            {

                if (lastStream == null)
                {
                    //直接打开
                    lastStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    lastStream.Position = lastStream.Length;
                    lastDir = fileName;
                }
                else
                {
                    if (lastDir == fileName)
                    {
                        //对比一样则直接返回
                        return lastStream;
                    }
                    lastStream.Close();
                    lastStream = null;
                    //关闭旧流
                    //新建流
                    lastStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    lastDir = fileName;
                }
                return lastStream;
            }
            catch(Exception ex)
            {
                File.WriteAllText(fileName, "日志系统出错" + ex);
                throw ex;
            }
            
        }


        private string GetFileName()
        {
            string fileName = string.Empty;

            switch (this._LogSplitType)
            {
                case LogSplitType.Hour:
                    fileName = DateTime.Now.ToString("yyyy_MM_dd_HH");
                    break;
                case LogSplitType.Day:
                    fileName = DateTime.Now.ToString("yyyy_MM-dd");
                    break;
                case LogSplitType.Month:
                    fileName = DateTime.Now.ToString("yyyy_MM");
                    break;
                case LogSplitType.Year:
                    fileName = DateTime.Now.ToString("yyyy");
                    break;
            }
            string dir= Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"zt_log");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Path.Combine(dir, fileName + ".log");
        }
        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            if (this._LogLevel <= LogLevel.DEBUG)
            {
                WriteString(string.Concat(message, Environment.NewLine));
            }
        }


        public void Error(string message)
        {
            WriteString(string.Concat(message, Environment.NewLine));         
        }
        

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(string message, Exception ex)
        {
            if (ex != null)
            {
                WriteString(string.Concat(message, Environment.NewLine, ex.ToString(), Environment.NewLine));
                return;
            }

            WriteString(string.Concat(message));
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            if (this._LogLevel <= LogLevel.INFO)
            {
                WriteString(string.Concat(message, Environment.NewLine));
            }
        }
    }
}