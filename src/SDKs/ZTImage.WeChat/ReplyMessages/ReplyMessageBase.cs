using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.ReplyMessages
{
    public abstract class ReplyMessageBase
    {
        public ReplyMessageBase(string fromUserName,string toUserName,Int64 createTime,string msgType)
        {
            this.FromUserName = fromUserName;
            this.ToUserName = toUserName;
            this.CreateTime = createTime;
            this.MsgType = msgType;
        }

        public ReplyMessageBase(string fromUserName, string toUserName, string msgType):this(fromUserName,toUserName,TypeConverter.DateToLong(DateTime.Now),msgType)
        {

        }



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

        /// <summary>
        /// 回复消息的类型
        /// </summary>
        public string MsgType { get; set; }


    }
}
