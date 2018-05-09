using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public class ReplyVoiceMessage:ReplyMessageBase
    {
        public ReplyVoiceMessage(string fromUserName, string toUserName, Int64 createTime) : base(fromUserName, toUserName, createTime, "voice")
        { }
        
        public ReplyVoiceMessage(string fromUserName, string toUserName) : base(fromUserName, toUserName, "voice")
        { }

        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id
        /// </summary>
        public string MediaId { get; set; }

    }
}
