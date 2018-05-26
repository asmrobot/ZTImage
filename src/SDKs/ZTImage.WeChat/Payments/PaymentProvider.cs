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

        private string KEY { get; set; }

        private const string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        
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
        public PrepayResult UnifiedOrder(TradeType tradeType,string body,string attach, string tradeNo, int totalFee, string clientIP,string productID,string openid)
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

            builder.Append("<notify_url>"+this.NotifyURL+"</notify_url>");
            parameters.Add("notify_url", this.NotifyURL);

            if (!string.IsNullOrEmpty(productID))
            {
                builder.Append("<product_id>"+productID+"</product_id>");
                parameters.Add("product_id", productID);
            }
            

            if (!string.IsNullOrWhiteSpace(openid))
            {
                builder.Append("<openid>{6}</openid>");
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

            string sign = GenericSignString(parameters);

            builder.Append("<sign>"+sign+"</sign>");
            builder.Append("</xml>");

            string xml = string.Empty;
            try
            {
                xml = ZTImage.HttpEx.SyncPost(url,builder.ToString(),Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("请求统一下单时出现错误", ex);
            }

            return ParseUnifiedOrderResult(xml);
        }
       

        /// <summary>
        /// 通过成签名字符串
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GenericSignString(SortedDictionary<string,string> parameters)
        {
            string signTemp = string.Empty;
            foreach (var item in parameters)
            {
               
                signTemp += item.Key + "=" + item.Value+"&";
            }

            signTemp += "key="+this.KEY;

            return ZTImage.Security.Cryptography.MD5.Encrypt(signTemp).ToUpper();
        }

        /// <summary>
        /// 解析统一下单接口返回
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


                val = GetNodeInnerText(document, "result_code");
                if (val != "SUCCESS")
                {
                    result.Message = GetNodeInnerText(document, "err_code_des");
                    return result;
                }
                

                if (!CheckResultSign(document))
                {
                    result.Message = "签名不正确";
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
            catch(Exception ex)
            {
                ZTImage.Log.Trace.Error("解析统一下单接口时出错,xml:"+xml, ex);
                result.Ok = false;
            }
            return result;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private bool CheckResultSign(XmlDocument document)
        {
            string sign = GetNodeInnerText(document, "sign");
            if (string.IsNullOrWhiteSpace(sign))
            {
                return false;
            }
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            parameters.Add("return_code", GetNodeInnerText(document, "return_code"));
            parameters.Add("return_msg", GetNodeInnerText(document, "return_msg"));
            parameters.Add("appid", GetNodeInnerText(document, "appid"));
            parameters.Add("mch_id", GetNodeInnerText(document, "mch_id"));
            parameters.Add("device_info", GetNodeInnerText(document, "device_info"));
            parameters.Add("nonce_str", GetNodeInnerText(document, "nonce_str"));
            parameters.Add("result_code", GetNodeInnerText(document, "result_code"));
            parameters.Add("err_code", GetNodeInnerText(document, "err_code"));
            parameters.Add("err_code_des", GetNodeInnerText(document, "err_code_des"));
            parameters.Add("trade_type", GetNodeInnerText(document, "trade_type"));
            parameters.Add("prepay_id", GetNodeInnerText(document, "prepay_id"));
            parameters.Add("code_url", GetNodeInnerText(document, "code_url"));

            string msign = GenericSignString(parameters);
            if (msign == sign)
            {
                return true;
            }

            return false;
        }

        private string GetNodeInnerText(XmlDocument document, string key)
        {
            XmlNode node = document.SelectSingleNode("/xml/key");
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }


        /// <summary>
        /// 查询订单
        /// </summary>
        public void OrderQuery()
        {

        }

        /// <summary>
        /// 解析支付通知
        /// </summary>
        /// <param name="xml"></param>
        public void ParsePayNotify(string xml)
        {

        }

    }
}
