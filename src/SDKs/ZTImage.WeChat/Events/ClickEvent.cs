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
    public class ClickEvent : EventBase
    {
        public ClickEvent()
        {
            this.Event = "click";
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        

    }
}
