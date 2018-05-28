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
    public class PayResult
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
        /// 公众账号ID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MchID { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        public string IsSubscribe { get; set; }

        /// <summary>
        /// 交易类型
        /// JSAPI,NATIVE,APP
        /// </summary>
        public TradeType TradeType { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string BankType { get; set; }

        /// <summary>
        /// 订单金额,单位为分
        /// </summary>
        public Int32 TotalFee { get; set; }

        /// <summary>
        /// 应结订单金额
        /// </summary>
        public Int32 SettlementTotalFee { get; set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 现金支付金额
        /// </summary>
        public Int32 CashFee { get; set; }

        /// <summary>
        /// 现金支付货币类型
        /// </summary>
        public string CashFeeType { get; set; }

        /// <summary>
        /// 总代金券金额
        /// </summary>
        public Int32 CouponFee { get; set; }

        /// <summary>
        /// 代金券使用数量
        /// </summary>
        public Int32 CouponCount { get; set; }

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商家数据包
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// yyyyMMddHHmmss
        /// </summary>
        public string TimeEnd { get; set; }
    }
}
