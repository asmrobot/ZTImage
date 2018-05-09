using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public class ReplyVideoMessage:ReplyMessageBase
    {
        public ReplyVideoMessage(string fromUserName, string toUserName, Int64 createTime) : base(fromUserName, toUserName, createTime, "video")
        { }



        public ReplyVideoMessage(string fromUserName, string toUserName) : base(fromUserName, toUserName, "video")
        { }



        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string Description { get; set; }


    }
}
