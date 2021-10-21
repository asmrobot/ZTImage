using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Exceptions
{
    /// <summary>
    /// 数据库错误
    /// </summary>
    public class DatabaseException:System.Exception
    {
        public DatabaseException():base()
        {
                
        }

        public DatabaseException(string msg):base(msg)
        {

        }
    }
}
