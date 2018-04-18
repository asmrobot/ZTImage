using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Events
{
    /// <summary>
    /// 上报地理位置事件
    /// 用户同意上报地理位置后，每次进入公众号会话时，都会在进入时上报地理位置，
    /// 或在进入会话后每5秒上报一次地理位置，公众号可以在公众平台网站中修改以上设置。
    /// 上报地理位置时，微信会将上报地理位置事件推送到开发者填写的URL。
    /// </summary>
    public class LocationEvent : EventBase
    {
        public LocationEvent()
        {
            this.Event = "location";
        }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        public double Precision { get; set; }

    }
}
