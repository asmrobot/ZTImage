using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public class ReplyNewsMessage:ReplyMessageBase
    {
        public ReplyNewsMessage(string fromUserName, string toUserName, Int64 createTime) : base(fromUserName, toUserName, createTime, "news")
        {
            this.Articles = new List<ReplyNewsMessageItem>();
        }



        public ReplyNewsMessage(string fromUserName, string toUserName) : base(fromUserName, toUserName, "news")
        {
            this.Articles = new List<ReplyNewsMessageItem>();
        }



        /// <summary>
        /// 图文消息个数，限制为8条以内
        /// </summary>
        public int ArticleCount { get; set; }

        /// <summary>
        /// 多条图文消息信息，默认第一个item为大图,注意，如果图文数超过8，则将会无响应
        /// </summary>
        public List<ReplyNewsMessageItem> Articles { get; set; }

    }


    /// <summary>
    /// 图文消息子项
    /// </summary>
    public class ReplyNewsMessageItem
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
    }
}
