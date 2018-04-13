using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Messages
{
    public enum MsgType:int
    {
        unknow=0,//未知
        location=1,//地理位置
        shortvideo=2,//小视频
        video=3,//视频
        voice=4,//音频
        image=5,//图片
        text=6,//文本
        link = 7,//链接

    }
}
