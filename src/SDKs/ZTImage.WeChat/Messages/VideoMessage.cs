using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Messages
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMessage:MessageBase
    {
        public VideoMessage()
        {
            this.MsgType = "video";
        }

        public string MediaId { get; set; }

        public string ThumbMediaId { get; set; }


    }
}
