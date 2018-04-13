using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public class ReplyVideoMessage:ReplyMessageBase
    {
        public ReplyVideoMessage()
        {
            this.MsgType = "video";
        }

        public string MediaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


    }
}
