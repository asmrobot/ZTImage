using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZTImage.WeChat.Utility;

namespace ZTImage.WeChat.Payments
{
    public class PaymentProvider
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MchID { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NotifyURL { get; set; }

        /// <summary>
        /// 支付秘钥
        /// </summary>
        private string KEY { get; set; }

        private const string Unified_Order_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";//统一下单
        private const string Order_QUERY_Url = "https://api.mch.weixin.qq.com/pay/orderquery";//订单查询


        public PaymentProvider(string appID, string mchID,string notifyURL,string key)
        {
            ParamCheckHelper.WhiteSpaceThrow(appID, "appID");
            ParamCheckHelper.WhiteSpaceThrow(mchID, "mchID");
            ParamCheckHelper.WhiteSpaceThrow(notifyURL, "notifyURL");
            ParamCheckHelper.WhiteSpaceThrow(key, "key");
        
            this.AppID = appID;           
            this.MchID = mchID;            
            this.NotifyURL = notifyURL;            
            this.KEY = key;
        }

        /// <summary>
        /// 统一下单
        /// </summary>
        /// <param name="tradeType">交易类型</param>
        /// <param name="body">商品描述，128</param>
        /// <param name="attach">附加数据</param>
        /// <param name="tradeNo">商户订单号</param>
        /// <param name="totalFee">标价金额，以‘分’为单位</param>
        /// <param name="clientIP">客户端IP，如果没有客户端，传服务器IP</param>
        /// <param name="productID">商品ID，商户自定义，native时必传</param>
        /// <param name="openid">用户标识，jsapi时必传</param>
        /// <returns></returns>
        public PrepayResult UnifiedOrder(TradeType tradeType,string body,string attach, string tradeNo, int totalFee, string clientIP,string productID,string openid,string notifyUrl=null)
        {
            ParamCheckHelper.LimitLengthThrow(body, 128, "body");
            ParamCheckHelper.LimitLengthThrow(attach, 127, "attach");
            ParamCheckHelper.LimitLengthThrow(tradeNo, 32, "tradeNo");

            if (totalFee <= 0)
            {
                throw new ArgumentOutOfRangeException("totalFee");
            }

            if (!ZTImage.Text.Valid.IsIP(clientIP))
            {
                throw new ArgumentOutOfRangeException("clientIP");
            }

            if (tradeType == TradeType.NATIVE)
            {
                ParamCheckHelper.LimitLengthThrow(productID, 32,"productID");
            }

            if (tradeType == TradeType.JSAPI)
            {
                ParamCheckHelper.LimitLengthThrow(openid, 128, "openid");
            }

            string nonce_str = Guid.NewGuid().ToString().Replace("-", "");
            
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml>");            
            builder.Append("<appid>"+this.AppID+"</appid>");
            parameters.Add("appid", this.AppID);

            builder.Append("<attach>"+attach+"</attach>");
            parameters.Add("attach", attach);

            builder.Append("<body>"+body+"</body>");
            parameters.Add("body", body);

            builder.Append("<mch_id>"+this.MchID+"</mch_id>");
            parameters.Add("mch_id", this.MchID);

            builder.Append("<nonce_str>"+nonce_str+"</nonce_str>");
            parameters.Add("nonce_str", nonce_str);

            builder.Append("<notify_url>"+(string.IsNullOrEmpty(notifyUrl)?this.NotifyURL:notifyUrl)+"</notify_url>");
            parameters.Add("notify_url", (string.IsNullOrEmpty(notifyUrl) ? this.NotifyURL : notifyUrl));

            if (!string.IsNullOrEmpty(productID))
            {
                builder.Append("<product_id>"+productID+"</product_id>");
                parameters.Add("product_id", productID);
            }
            

            if (!string.IsNullOrWhiteSpace(openid))
            {
                builder.Append("<openid>"+openid+"</openid>");
                parameters.Add("openid", openid);
            }
            

            builder.Append("<out_trade_no>"+tradeNo+"</out_trade_no>");
            parameters.Add("out_trade_no", tradeNo);

            builder.Append("<spbill_create_ip>"+clientIP+"</spbill_create_ip>");
            parameters.Add("spbill_create_ip", clientIP);

            builder.Append("<total_fee>"+totalFee+"</total_fee>");
            parameters.Add("total_fee", totalFee.ToString());

            string tradeTypeString = "JSAPI";
            switch (tradeType)
            {
                case TradeType.NATIVE:
                    tradeTypeString = "NATIVE ";
                    break;

                case TradeType.APP:
                    tradeTypeString = "APP";
                    break;
            }

            builder.Append("<trade_type>"+ tradeTypeString + "</trade_type>");
            parameters.Add("trade_type", tradeTypeString);

            string sign = CalcSign(parameters);

            builder.Append("<sign>"+sign+"</sign>");
            builder.Append("</xml>");

            string xml = string.Empty;
            try
            {
                xml = ZTImage.HttpEx.SyncPost(Unified_Order_Url,builder.ToString(),Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("请求统一下单时出现错误", ex);
            }

            return ParseUnifiedOrderResult(xml);
        }

        /// <summary>
        /// 解析微信统一下单接口返回的数据
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private PrepayResult ParseUnifiedOrderResult(string xml)
        {
            PrepayResult result = new PrepayResult();
            result.Ok = false;
            result.Message = "微信服务错误";
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(xml);
                string val = GetNodeInnerText(document, "return_code");
                if (val != "SUCCESS")
                {
                    result.Message = GetNodeInnerText(document, "return_msg");
                    return result;
                }

                if (!CheckSign(document))
                {
                    result.Message = "微信端签名不正确";
                    return result;
                }

                //result.AppID = GetNodeInnerText(document, "appid");
                //result.MchID = GetNodeInnerText(document, "mch_id");
                //result.DeviceInfo = GetNodeInnerText(document, "device_info");

                val = GetNodeInnerText(document, "result_code");
                if (val != "SUCCESS")
                {
                    result.Message = GetNodeInnerText(document, "err_code_des");
                    return result;
                }


                

                TradeType tt = TradeType.JSAPI;
                string trade_type = GetNodeInnerText(document, "trade_type");
                if (!Enum.TryParse<TradeType>(trade_type, out tt))
                {
                    tt = TradeType.JSAPI;
                }
                result.TradeType = tt;
                result.PrepayID = GetNodeInnerText(document, "prepay_id");
                result.CodeUrl = GetNodeInnerText(document, "code_url");
                result.Ok = true;
                result.Message = "success";
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("解析统一下单接口时出错,xml:" + xml, ex);
                result.Ok = false;
            }
            return result;
        }
        
        /// <summary>
        /// 查询订单通过微信订单号
        /// </summary>
        public PayQueryResult OrderQueryByTransactionID(string transactionID)
        {
            return OrderQuery(true, transactionID);
        }

        /// <summary>
        /// 查询订单通过商户订单号
        /// </summary>
        public PayQueryResult OrderQueryByTradeNo(string outTradeNo)
        {
            return OrderQuery(false, outTradeNo);
        }
        
        /// <summary>
        /// 解析支付通知
        /// </summary>
        /// <param name="xml"></param>
        public PayResult ParsePayNotify(string xml)
        {
            PayResult result = new PayResult();
            result.Ok = true;
            result.Message = "微信服务错误";
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(xml);
                string val = GetNodeInnerText(document, "return_code");
                if (val != "SUCCESS")
                {
                    result.Ok = false;
                    result.Message = GetNodeInnerText(document, "return_msg");
                    return result;
                }

                result.AppID = GetNodeInnerText(document, "appid");
                result.MchID = GetNodeInnerText(document, "mch_id");
                result.DeviceInfo = GetNodeInnerText(document, "device_info");

                if (!CheckSign(document))
                {
                    result.Message = "微信端签名不正确";
                    result.Ok = false;
                    return result;
                }

                val = GetNodeInnerText(document, "result_code");
                if (val != "SUCCESS")
                {
                    result.Ok = false;
                    result.Message = GetNodeInnerText(document, "err_code_des");
                }
                
                result.OpenID = GetNodeInnerText(document, "openid");
                result.IsSubscribe = GetNodeInnerText(document, "is_subscribe");


                TradeType tt = TradeType.JSAPI;
                string trade_type = GetNodeInnerText(document, "trade_type");
                if (!Enum.TryParse<TradeType>(trade_type, out tt))
                {
                    tt = TradeType.JSAPI;
                }
                result.TradeType = tt;

                result.BankType = GetNodeInnerText(document, "bank_type");
                string totalFee = GetNodeInnerText(document, "total_fee");
                result.TotalFee = TypeConverter.StringToInt(totalFee, 0);

                string settlementTotalFee = GetNodeInnerText(document, "settlement_total_fee");
                result.SettlementTotalFee = TypeConverter.StringToInt(settlementTotalFee, 0);

                result.FeeType = GetNodeInnerText(document, "fee_type");
                string cashFee = GetNodeInnerText(document, "cash_fee");
                result.CashFee = TypeConverter.StringToInt(cashFee, 0);

                result.CashFeeType = GetNodeInnerText(document, "cash_fee_type");
                string couponFee = GetNodeInnerText(document, "coupon_fee");
                result.CouponFee = TypeConverter.StringToInt(couponFee, 0);

                string couponCount = GetNodeInnerText(document, "coupon_count");
                result.CouponCount = TypeConverter.StringToInt(couponCount, 0);


                result.TransactionID = GetNodeInnerText(document, "transaction_id");
                result.OutTradeNo = GetNodeInnerText(document, "out_trade_no");
                result.Attach = GetNodeInnerText(document, "attach");
                result.TimeEnd = GetNodeInnerText(document, "time_end");
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("解析支付结果通知时出错,xml:" + xml, ex);
                result.Ok = false;
            }
            return result;
        }
        
        /// <summary>
        /// 支付通知返回数据
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GenericPayNotifyReturnString(bool success, string msg)
        {
            string xml = @"
            <xml>
              <return_code><![CDATA["+(success? "SUCCESS" : "FAIL") +@"]]></return_code>
              <return_msg><![CDATA["+msg+@"]]></return_msg>
            </xml>";

            return xml;
        }
        
        /// <summary>
        /// 生成成签名字符串
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string CalcSign(SortedDictionary<string, string> parameters)
        {
            string signTemp = string.Empty;
            foreach (var item in parameters)
            {

                signTemp += item.Key + "=" + item.Value + "&";
            }

            signTemp += "key=" + this.KEY;

            return ZTImage.Security.Cryptography.MD5.Encrypt(signTemp).ToUpper();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private bool CheckSign(XmlDocument document)
        {
            string sign = GetNodeInnerText(document, "sign");
            if (string.IsNullOrWhiteSpace(sign))
            {
                return false;
            }
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            XmlElement element = document.DocumentElement;
            for (int i = 0; i < element.ChildNodes.Count; i++)
            {
                XmlNode node = element.ChildNodes[i];
                string name = node.Name;
                string val = node.InnerText;
                if (name == "sign")
                {
                    continue;
                }
                parameters.Add(name, val);
            }

            string msign = CalcSign(parameters);
            if (msign == sign)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 得到微信xml节点内容
        /// </summary>
        /// <param name="document"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetNodeInnerText(XmlDocument document, string key)
        {
            XmlNode node = document.SelectSingleNode("/xml/"+key);
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="byTransactionID"></param>
        /// <param name="identify"></param>
        /// <returns></returns>
        private PayQueryResult OrderQuery(bool byTransactionID, string identify)
        {
            ParamCheckHelper.WhiteSpaceThrow(identify, "identify");
            ParamCheckHelper.LimitLengthThrow(identify, 32, "identify");

            string nonce_str = Guid.NewGuid().ToString().Replace("-", "");

            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml>");
            builder.Append("<appid>" + this.AppID + "</appid>");
            parameters.Add("appid", this.AppID);

            builder.Append("<mch_id>" + this.MchID + "</mch_id>");
            parameters.Add("mch_id", this.MchID);

            if (byTransactionID)
            {
                builder.Append("<transaction_id>" + identify + "</transaction_id>");
                parameters.Add("transaction_id", identify);
            }
            else
            {
                builder.Append("<out_trade_no>" + identify + "</out_trade_no>");
                parameters.Add("out_trade_no", identify);
            }

            builder.Append("<nonce_str>" + nonce_str + "</nonce_str>");
            parameters.Add("nonce_str", nonce_str);

            string sign = CalcSign(parameters);

            builder.Append("<sign>" + sign + "</sign>");
            builder.Append("</xml>");

            string xml = string.Empty;
            try
            {
                xml = ZTImage.HttpEx.SyncPost(Order_QUERY_Url, builder.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("订单查询时出现错误", ex);
            }

            return ParseOrderQuery(xml);
        }

        /// <summary>
        /// 解析订单查询结果
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private PayQueryResult ParseOrderQuery(string xml)
        {
            PayQueryResult result = new PayQueryResult();
            result.Ok = false;
            result.Message = "微信服务错误";
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(xml);
                string val = GetNodeInnerText(document, "return_code");
                if (val != "SUCCESS")
                {
                    result.Ok = false;
                    result.Message = GetNodeInnerText(document, "return_msg");
                    return result;
                }

                if (!CheckSign(document))
                {
                    result.Message = "微信端签名不正确";
                    result.Ok = false;
                    return result;
                }

                result.AppID = GetNodeInnerText(document, "appid");
                result.MchID = GetNodeInnerText(document, "mch_id");


                val = GetNodeInnerText(document, "result_code");
                if (val != "SUCCESS")
                {
                    result.Ok = false;
                    result.Message = GetNodeInnerText(document, "err_code_des");
                    return result;
                }


                TradeState ts = TradeState.NOTPAY;
                string tradeState = GetNodeInnerText(document, "trade_state");
                if (!Enum.TryParse<TradeState>(tradeState, out ts))
                {
                    ts = TradeState.NOTPAY;
                }
                result.TradeState = ts;

                if (result.TradeState != TradeState.SUCCESS)
                {
                    result.TradeStateDesc = GetNodeInnerText(document, "trade_state_desc");
                    result.Ok = false;
                    result.Message = "订单未完成";
                    return result;
                }

                result.DeviceInfo = GetNodeInnerText(document, "device_info");
                result.OpenID = GetNodeInnerText(document, "openid");
                result.IsSubscribe = GetNodeInnerText(document, "is_subscribe");


                TradeType tt = TradeType.JSAPI;
                string trade_type = GetNodeInnerText(document, "trade_type");
                if (!Enum.TryParse<TradeType>(trade_type, out tt))
                {
                    tt = TradeType.JSAPI;
                }
                result.TradeType = tt;

                result.BankType = GetNodeInnerText(document, "bank_type");
                string totalFee = GetNodeInnerText(document, "total_fee");
                result.TotalFee = TypeConverter.StringToInt(totalFee, 0);

                string settlementTotalFee = GetNodeInnerText(document, "settlement_total_fee");
                result.SettlementTotalFee = TypeConverter.StringToInt(settlementTotalFee, 0);

                result.FeeType = GetNodeInnerText(document, "fee_type");
                string cashFee = GetNodeInnerText(document, "cash_fee");
                result.CashFee = TypeConverter.StringToInt(cashFee, 0);

                result.CashFeeType = GetNodeInnerText(document, "cash_fee_type");
                string couponFee = GetNodeInnerText(document, "coupon_fee");
                result.CouponFee = TypeConverter.StringToInt(couponFee, 0);

                string couponCount = GetNodeInnerText(document, "coupon_count");
                result.CouponCount = TypeConverter.StringToInt(couponCount, 0);


                result.TransactionID = GetNodeInnerText(document, "transaction_id");
                result.OutTradeNo = GetNodeInnerText(document, "out_trade_no");
                result.Attach = GetNodeInnerText(document, "attach");
                result.TimeEnd = GetNodeInnerText(document, "time_end");
                result.Ok = true;
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("解析支付结果通知时出错,xml:" + xml, ex);
                result.Ok = false;
            }
            return result;
        }
    }
}
