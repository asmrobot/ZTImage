using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Events
{
    /// <summary>
    /// 取消订阅
    /// </summary>
    public class UnsubscribeEvent : EventBase
    {
        public UnsubscribeEvent()
        {
            this.Event = "unsubscribe";
        }
    }
}
