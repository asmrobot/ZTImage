using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public class ReplyImageMessage:ReplyMessageBase
    {
        public ReplyImageMessage()
        {
            this.MsgType = "image";
        }

        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id。
        /// </summary>
        public string MediaId { get; set; }

    }
}
