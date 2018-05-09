using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Messages
{
    public abstract class MessageBase:PushBase
    {
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public Int64 MsgID { get; set; }
    }
}
