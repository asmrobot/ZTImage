using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Messages
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMessage:MessageBase
    {
        public TextMessage()
        {
            this.MsgType = "text";
        }
        public string Content { get; set; }
    }
}
