using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public abstract class ReplyMessageBase
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public Int64 CreateTime { get; set; }

        public string MsgType { get; set; }


    }
}
