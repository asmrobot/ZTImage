using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Events
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public class EventBase:PushBase
    {
        public EventBase()
        {
            this.MsgType = "event";
        }

        public string @Event { get; set; }

        public EventType GetEventType()
        {
            EventType t = ZTImage.WeChat.EventType.unknow;
            if (Enum.TryParse<EventType>(this.@Event,true, out t))
            {
                return t;
            }
            return ZTImage.WeChat.EventType.unknow;
        }
    }
}
