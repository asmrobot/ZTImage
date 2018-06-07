using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Settings
{
    public class Global
    {
        static Global()
        {
            ExceptionLevel = ExceptionLevel.None;
        }


        /// <summary>
        /// 异常记录级别，默认不记录
        /// </summary>
        public static ExceptionLevel ExceptionLevel { get; set; }
        
    }
}
