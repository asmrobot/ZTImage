﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Messages
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class VoiceMessage:MessageBase
    {
        public VoiceMessage()
        {
            this.MsgType = "voice";
        }

        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 语音识别结果，UTF8编码
        /// 开启语语音识别后会存在
        /// </summary>
        public string Recognition { get; set; }



    }
}
