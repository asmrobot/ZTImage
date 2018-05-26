using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Payments
{
    /// <summary>
    /// 预支付结果 
    /// </summary>
    public class PrepayResult
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// 如果不成功返回信息
        /// </summary>
        public string Message { get; set; }

        
        /// <summary>
        /// 交易类型
        /// </summary>
        public TradeType TradeType { get; set; }

        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string PrepayID { get; set; }

        /// <summary>
        /// 二维码链接
        /// trade_type为NATIVE时有返回，用于生成二维码，展示给用户进行扫码支付
        /// </summary>
        public string CodeUrl { get; set; }
    }
}
