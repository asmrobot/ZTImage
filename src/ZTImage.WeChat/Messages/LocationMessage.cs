using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Messages
{
    /// <summary>
    /// 地理位置
    /// </summary>
    public class LocationMessage:MessageBase
    {
        public LocationMessage()
        {
            this.MsgType = "location";
        }

        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
    }
}
