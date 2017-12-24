using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    /// <summary>
    /// 数据库错误
    /// </summary>
    public class DatabaseException:Exception
    {
        public DatabaseException():base()
        {
                
        }

        public DatabaseException(string msg):base(msg)
        {

        }
    }
}
