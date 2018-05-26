using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Payments
{
    /// <summary>
    /// 支付通知
    /// </summary>
    public class PayNotifyResult
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// 如果不成功返回信息
        /// </summary>
        public string Message { get; set; }


        public string AppID { get; set; }

        public string MchID { get; set; }


        public string DeviceInfo { get; set; }
    }
}
