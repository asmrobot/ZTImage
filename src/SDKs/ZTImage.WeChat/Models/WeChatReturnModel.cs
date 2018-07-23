using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
{
    /// <summary>
    /// 微信调用返回信息模型
    /// </summary>
    public class WeChatReturnModel
    {
        /// <summary>
        /// 0为成功，其它为失败
        /// </summary>
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string msgid { get; set; }

    }
}
