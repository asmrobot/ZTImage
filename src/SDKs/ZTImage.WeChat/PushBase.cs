﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.WeChat.Messages;

namespace ZTImage.WeChat
{
    /// <summary>
    /// 推送父类
    /// </summary>
    public abstract class PushBase
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public Int64 CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 得到消息类型
        /// </summary>
        /// <returns></returns>
        public MsgType GetMsgType()
        {
            MsgType t = ZTImage.WeChat.MsgType.text;
            if (Enum.TryParse<MsgType>(this.MsgType,true, out t))
            {
                return t;
            }
            return ZTImage.WeChat.MsgType.unknow;
        }

    }
}
