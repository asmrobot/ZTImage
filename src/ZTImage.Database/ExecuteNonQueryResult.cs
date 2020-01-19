using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Database
{
    /// <summary>
    /// ExecuteNon Query执行结果
    /// 当使用async/await 时，需要返回最新添加数据ID时，需要用此结构
    /// </summary>
    public struct ExecuteNonQueryResult
    {
        public ExecuteNonQueryResult(Int32 affectRowCount,object outID)
        {
            this.AffectRowCount = affectRowCount;
            this.OutID = outID;
        }

        public Int32 AffectRowCount { get; private set; }

        public object OutID { get; private set; }
    }
}
