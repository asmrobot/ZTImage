using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
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
        @event=8,//事件
    }

    public enum EventType : int
    {
        unknow=0,//未知
        subscribe=1,//订阅
        unsubscribe=2,//取消订阅
        scan=3,//扫描带参数二维码
        location=4,//上报地理位置
        click=5,//点击自定义菜单
        view=6,//点击菜单跳转链接
    }
}
