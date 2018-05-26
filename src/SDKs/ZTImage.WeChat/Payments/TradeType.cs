using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Payments
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradeType
    {
        JSAPI,//公众号支付
        NATIVE,//扫码支付
        APP //app支付
    }
}
