using System;

namespace ZTImage.Log
{
    /// <summary>
    /// console trace listener
    /// </summary>
    public sealed class ConsoleListener : ITraceListener
    {
        private LogLevel _LogLevel;
        public ConsoleListener(LogLevel level)
        {
            this._LogLevel = level;
        }

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            if (this._LogLevel <= LogLevel.DEBUG)
            {
                Console.WriteLine(string.Concat(message, Environment.NewLine));
            }
        }

        public void Error(string message)
        {
            Console.WriteLine(string.Concat(message, Environment.NewLine));
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
                Console.WriteLine(string.Concat(message, Environment.NewLine, ex.ToString(), Environment.NewLine));
                return;
            }
            Console.WriteLine(string.Concat(message, Environment.NewLine));
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            if (this._LogLevel <= LogLevel.INFO)
            {
                Console.WriteLine(string.Concat(message, Environment.NewLine));
            }
        }
    }
}