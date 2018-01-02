using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Log
{
    public interface IZTLog
    {

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        void Debug(string message);

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        void Info(string message);

        /// <summary>
        /// info
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        void Warn(string message);

        void Error(string message);

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        void Error(string message, Exception ex);

        void Fatal(string message);

        /// <summary>
        /// error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <exception cref="ArgumentNullException">message is null</exception>
        void Fatal(string message, Exception ex);
    }
}
