using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Events
{
    public class SubscribeEvent:EventBase
    {
        public SubscribeEvent()
        {
            this.Event = "subscribe";
        }





        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// 扫描二维码，并且未关注的时候才会有,已关注扫描见：scanevent
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// 扫描二维码，并且未关注的时候才会有,已关注扫描见：scanevent
        /// </summary>
        public string Ticket { get; set; }
    }
}
