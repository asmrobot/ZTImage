using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Log
{
    public class FileLogOutput : ILogOutput
    {

        public FileLogOutput(string path)
        {
            this._LogFilePath = path;
            
            //this._Writer= new StreamWriter(this._LogFilePath,true,Encoding.UTF8);
            
        }


        private string _LogFilePath
        {
            get;set;
        }
       

        //private StreamWriter _Writer
        //{
        //    get;
        //    set;
        //}



        public void Write(LogLevel.LogLevelInfo logLevel, string tag, string message)
        {
            if (logLevel.Priority >= Log.Level.Priority)
            {
                File.WriteAllText(this._LogFilePath, string.Format("level:{0},tag:{1},message:{2}", logLevel.Value, tag, message), Encoding.UTF8);
                
            }
        }

        public void WriteAndPromptLog(LogLevel.LogLevelInfo logLevel, string tag, string message)
        {
            if (logLevel.Priority >= Log.Level.Priority)
            {
                File.WriteAllText(this._LogFilePath, string.Format("level:{0},tag:{1},message:{2}", logLevel.Value, tag, message), Encoding.UTF8);
            }
        }
    }
}
