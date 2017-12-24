using System;

namespace ZTImage.Log
{
    /// <summary>
    /// diagnostic listener
    /// </summary>
    public sealed class DiagnosticListener : ITraceListener
    {
        private LogLevel _LogLevel;
        public DiagnosticListener(LogLevel level)
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
                System.Diagnostics.Trace.WriteLine(message);
            }
        }


        public void Error(string message)
        {
            System.Diagnostics.Trace.TraceError(message);
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
                System.Diagnostics.Trace.TraceError(string.Concat(message,ex.ToString()));
                return;
            }
            System.Diagnostics.Trace.TraceError(message);
        }
        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            if (this._LogLevel <= LogLevel.INFO)
            {
                System.Diagnostics.Trace.TraceInformation(message);
            }
        }
    }
}