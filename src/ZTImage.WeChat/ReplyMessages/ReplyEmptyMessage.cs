using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    /// <summary>
    /// 空白消息
    /// </summary>
    public class ReplyEmptyMessage : ReplyMessageBase
    {
        public ReplyEmptyMessage(string fromUserName, string toUserName, Int64 createTime) : base(fromUserName, toUserName, createTime, "text")
        {
            this.Content = string.Empty;
        }



        public ReplyEmptyMessage(string fromUserName, string toUserName) : base(fromUserName, toUserName, "text")
        {
            this.Content = string.Empty;
        }



        /// <summary>
        /// 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        public string Content { get; set; }
    }
}
