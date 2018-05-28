using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Payments
{
    /// <summary>
    /// 订单查询结果
    /// </summary>
    public class PayQueryResult:PayResult
    {
        /// <summary>
        /// 交易状态
        /// </summary>
        public TradeState TradeState { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string TradeStateDesc { get; set; }
    }
}
