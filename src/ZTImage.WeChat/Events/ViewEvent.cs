using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Events
{
    /// <summary>
    /// 点击菜单拉取消息时的事件推送
    /// 用户点击自定义菜单后，微信会把点击事件推送给开发者，请注意，点击菜单弹出子菜单，不会产生上报。
    /// </summary>
    public class ViewEvent : EventBase
    {
        public ViewEvent()
        {
            this.Event = "view";
        }

        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
        

    }
}
