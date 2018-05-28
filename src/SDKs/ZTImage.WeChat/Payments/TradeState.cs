using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Payments
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public enum TradeState
    {
        SUCCESS,//—支付成功

        REFUND,//—转入退款

        NOTPAY,//—未支付

        CLOSED,//—已关闭

        REVOKED,//—已撤销（刷卡支付）

        USERPAYING,//--用户支付中

        PAYERROR,//--支付失败(其他原因，如银行返回失败)
    }
}
