using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.WeChat.Events;
using ZTImage.WeChat.Menus;
using ZTImage.WeChat.Messages;
using ZTImage.WeChat.Models;
using ZTImage.WeChat.Payments;
using ZTImage.WeChat.Utility;

namespace ZTImage.WeChat
{
    public class WeChatManager
    {
        private string mToken;
        private string mAppID;
        private string mAppSecurity;

        private static object mTokenProviderLocker = new object();
        private AccessTokenProvider mAccessTokenProvider;

        private PaymentProvider mPaymentProvider;

        private static object mJSAPITicketProviderLocker = new object();
        private JSAPITicketProvider mJSAPITicketProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token">消息接收的token</param>
        /// <param name="appID"></param>
        /// <param name="appSecurity"></param>
        public WeChatManager(string token,string appID,string appSecurity)
        {
            this.mToken = token;
            this.mAppID = appID;
            this.mAppSecurity = appSecurity;
           
        }

        /// <summary>
        /// 初始化支付
        /// </summary>
        /// <param name="mchID"></param>
        /// <param name="notifyURL"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool InitializePayment(string mchID, string notifyURL, string key)
        {
            if (this.mPaymentProvider != null)
            {
                return false;
            }

            this.mPaymentProvider = new PaymentProvider(this.mAppID, mchID, notifyURL, key);
            return true;
        }

        #region 公众号基础

        /// <summary>
        /// 确保access token provider已初始化
        /// </summary>
        /// <returns></returns>
        private AccessTokenProvider EnsureAccessTokenProvider()
        {
            if (mAccessTokenProvider == null)
            {
                lock (mTokenProviderLocker)
                {
                    if (mAccessTokenProvider == null)
                    {
                        mAccessTokenProvider = new AccessTokenProvider(this.mAppID, this.mAppSecurity);
                    }
                }
            }
            return mAccessTokenProvider;
        }

        /// <summary>
        /// 确保 jsapi ticket provider已初始化
        /// </summary>
        /// <returns></returns>
        private JSAPITicketProvider EnsureJSAPITicketProvider()
        {
            if (mJSAPITicketProvider == null)
            {
                EnsureAccessTokenProvider();
                lock (mJSAPITicketProviderLocker)
                {
                    if (mJSAPITicketProvider == null)
                    {
                        mJSAPITicketProvider = new JSAPITicketProvider(this.mAccessTokenProvider);
                    }
                }
            }
            return mJSAPITicketProvider;
        }




        /// <summary>
        /// 得到access token
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            EnsureAccessTokenProvider();
            return this.mAccessTokenProvider.GetAccessToken();
        }

        /// <summary>
        /// 设置access token过期
        /// </summary>
        public void SetAssessTokenExpire()
        {
            if (mAccessTokenProvider == null)
            {
                return;
            }

            mAccessTokenProvider.SetExpire();
        }

        /// <summary>
        /// 得到jsapi ticket
        /// </summary>
        /// <returns></returns>
        public string GetJSAPITicket()
        {
            EnsureJSAPITicketProvider();
            return this.mJSAPITicketProvider.GetTicket();
        }

        public void SetJSAPITicketExpire()
        {
            if (this.mJSAPITicketProvider == null)
            {
                return;
            }
            this.mJSAPITicketProvider.SetExpire();
        }
        

        /// <summary>
        /// 设置url时，验证
        /// </summary>
        /// <returns></returns>
        public bool VaildIsOK(string timestamp, string nonce, string signature)
        {
            List<string> list = new List<string>();
            list.Add(this.mToken);
            list.Add(timestamp);
            list.Add(nonce);
            list.Sort(new StringComparer());

            string raw = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                raw += list[i];
            }
            
            string hash = ZTImage.Security.Cryptography.SHA1.Encrypt(raw);
            return hash.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// xml字符串解析为消息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public PushBase XmlToMessage(string xml)
        {
            XmlUtils deserialize = new XmlUtils(xml);
            string msgtype = deserialize.GetValue("/xml/MsgType").ToLower();
            PushBase message = null;
            switch (msgtype)
            {
                //普通消息
                case "text":
                    message = new TextMessage();
                    break;
                case "image":
                    message = new ImageMessage();
                    break;
                case "voice":
                    message = new VoiceMessage();
                    break;
                case "video":
                    message = new VideoMessage();
                    break;
                case "shortvideo":
                    message = new ShortVideoMessage();
                    break;
                case "location":
                    message = new LocationMessage();
                    break;
                case "link":
                    message = new LinkMessage();
                    break;
                //事件推送
                case "event":
                    message = GetEventModel(deserialize);
                    break;
                default:
                    return null;
            }

            deserialize.FillModel(message);
            return message;
        }
        
        /// <summary>
        /// 事件模型
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private EventBase GetEventModel(XmlUtils xml)
        {
            string eventType = xml.GetValue("/xml/Event").ToLower();
            EventBase message = null;
            switch (eventType)
            {
                case "unsubscribe":
                    message = new UnsubscribeEvent();
                    break;
                case "subscribe":
                    message = new SubscribeEvent();
                    break;
                case "scan":
                    message = new ScanEvent();
                    break;
                case "location":
                    message = new LocationEvent();
                    break;
                case "click":
                    message = new ClickEvent();
                    break;
                case "view":
                    message = new ViewEvent();
                    break;
                default:
                    return null;
            }

            return message;
        }

        /// <summary>
        /// 回复消息转化为xml字符串
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ReplyMessageToXml(ReplyMessages.ReplyMessageBase message)
        {
            return XmlUtils.ToXmlString(message);
        }

        /// <summary>
        /// 公众号创建菜单
        ///  https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN
        /// </summary>
        /// <param name="menus"></param>
        /// <returns>调用是否成功</returns>
        public bool CreateMenus(List<MenuBase> menus)
        {
            if (menus.Count > 3)
            {
                throw new Exception("菜单超限");
            }


            MenuContainerModel container = new MenuContainerModel();
            container.button = menus;

            string postData = ZTImage.Json.JsonBuilder.ToJsonString(container);
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + this.GetAccessToken();
            string returnData = string.Empty;
            try
            {
                returnData = ZTImage.HttpEx.SyncPost(url, postData, Encoding.UTF8);
            }
            catch(Exception ex)
            {
                ZTImage.Log.Trace.Error("请求微信接口错误",ex);
                return false;
            }
            
            WeChatReturnModel ret= ZTImage.Json.JsonParser.ToObject<WeChatReturnModel>(returnData);
            if (ret == null)
            {
                ZTImage.Log.Trace.Error("返回消息没有成功转化为json,data:"+returnData);
                return false;
            }

            if (ret.errcode == 0)
            {
                return true;
            }

            ZTImage.Log.Trace.Warn("设置菜单返回值为："+returnData);
            return false;
        }

        /// <summary>
        /// 发送模板消息
        /// https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=ACCESS_TOKEN
        /// </summary>
        /// <returns></returns>
        public bool SendTemplateMessage(string touser,string template_id,string url,string dataJson)
        {
            string messageJson = "{\"touser\":\""+touser+"\",\"template_id\":\""+template_id+"\",";
            if (!string.IsNullOrEmpty(url))
            {
                messageJson += "\"url\":\""+url+"\",";
            }
            messageJson += "\"data\":"+dataJson+"}";

            string posturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.GetAccessToken();
            string returnData = string.Empty;
            try
            {
                returnData = ZTImage.HttpEx.SyncPost(posturl, messageJson, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("请求微信发送模板消息接口错误", ex);
                return false;
            }

            WeChatReturnModel ret = ZTImage.Json.JsonParser.ToObject<WeChatReturnModel>(returnData);
            if (ret == null)
            {
                ZTImage.Log.Trace.Error("返回消息没有成功转化为json,data:" + returnData);
                return false;
            }

            if (ret.errcode == 0)
            {
                return true;
            }

            ZTImage.Log.Trace.Warn("发送模板消息返回值为：" + returnData);
            return false;

        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="touser"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public bool SendTemplateMessage(string touser ,string url, TemplateMessageBase template)
        {
            return SendTemplateMessage(touser, template.template_id, url, template.GetDataJson());
        }


        /// <summary>
        /// 得到授权URL
        /// 将用户导向此url,如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。
        /// </summary>
        /// <param name="redirectURL">授权后回调url</param>
        /// <param name="scope">要求的授权类型</param>
        /// <param name="tag">附带信息，长度不可大于128，可空</param>
        /// <returns></returns>
        public string GetAuthUrl(string redirectURL, AuthenticationScope scope,string tag)
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+this.mAppID+"&redirect_uri="+ZTImage.Text.Coding.EncodeURI(redirectURL)+"&response_type=code&scope="+(scope==AuthenticationScope.snsapi_base? "snsapi_base" : "snsapi_userinfo") +"&state="+tag+"#wechat_redirect";
            return url;
        }


        /// <summary>
        /// 得到授权AccessToken,OpenID
        /// </summary>
        /// <param name="code">授权URL得到的code</param>
        /// <param name="authAccessToken">授权Access Token</param>
        /// <param name="openID"></param>
        /// <param name="expiresIn">AccessToken过期时间，单位(秒)</param>
        /// <param name="refreshToken">用户刷新access_token,有效期30天</param>
        /// <returns></returns>
        public bool GetAuthenticationAccessToken(string code,out string authAccessToken,out string openID,out Int32 expiresIn,out string refreshToken)
        {
            authAccessToken = string.Empty;
            openID = string.Empty;
            expiresIn = 7200;
            refreshToken = string.Empty;

            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid="+this.mAppID+"&secret="+this.mAppSecurity+"&code="+code+"&grant_type=authorization_code";
            string json = string.Empty;

            try
            {
                json = ZTImage.HttpEx.SyncGet(url);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("请求授权AccessToken时出现错误",ex);
                return false;
            }

            Dictionary<string, object> dic = null;
            try
            {
                dic = ZTImage.Json.JsonParser.ToDictionary(json);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("解析json时出现错误,json:"+json, ex);
            }

            if (dic.ContainsKey("access_token") && dic.ContainsKey("openid")
                &&dic.ContainsKey("expires_in")&&dic.ContainsKey("refresh_token"))
            {
                authAccessToken = dic["access_token"].ToString();
                openID = dic["openid"].ToString();
                expiresIn = ZTImage.TypeConverter.ObjectToInt(dic["expires_in"], 7200);
                refreshToken = dic["refresh_token"].ToString();
                return true;
            }
            
            ZTImage.Log.Trace.Warn("请求授权AccessToken时出现错误,json:"+json);
            return false;
        }

        /// <summary>
        /// 刷新授权AccessToken
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="authAccessToken"></param>
        /// <param name="openID"></param>
        /// <param name="expiresIn"></param>
        /// <param name="newRefreshToken"></param>
        /// <returns></returns>
        public bool RefreshAuthenticationAccessToken(string refreshToken,out string authAccessToken, out string openID, out Int32 expiresIn, out string newRefreshToken)
        {
            authAccessToken = string.Empty;
            openID = string.Empty;
            expiresIn = 7200;
            newRefreshToken = string.Empty;

            string url = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid="+this.mAppID+"&grant_type=refresh_token&refresh_token="+refreshToken;
            string json = string.Empty;

            try
            {
                json = ZTImage.HttpEx.SyncGet(url);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("请求授权AccessToken时出现错误", ex);
                return false;
            }

            Dictionary<string, object> dic = null;
            try
            {
                dic = ZTImage.Json.JsonParser.ToDictionary(json);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("解析json时出现错误,json:" + json, ex);
            }

            if (dic.ContainsKey("access_token") && dic.ContainsKey("openid")
                && dic.ContainsKey("expires_in") && dic.ContainsKey("refresh_token"))
            {
                authAccessToken = dic["access_token"].ToString();
                openID = dic["openid"].ToString();
                expiresIn = ZTImage.TypeConverter.ObjectToInt(dic["expires_in"], 7200);
                newRefreshToken = dic["refresh_token"].ToString();
                return true;
            }

            ZTImage.Log.Trace.Warn("刷新授权AccessToken时出现错误,json:" + json);
            return false;
        }
        
        /// <summary>
        /// 拉取用户信息,通过AccessToken
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="authAccessToken"></param>
        public WeChatUserInfo GetUserInfoByAuthAccessToken(string openid,string authAccessToken)
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + authAccessToken + "&openid=" + openid + "&lang=zh_CN";
            string json = string.Empty;

            try
            {
                json = ZTImage.HttpEx.SyncGet(url);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("请求授权AccessToken时出现错误", ex);
            }

            WeChatUserInfo userInfo = null;
            try
            {
                userInfo = ZTImage.Json.JsonParser.ToObject<WeChatUserInfo>(json);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("解析json时出现错误,json:"+json, ex);
                return null;
            }

            if (userInfo == null)
            {
                ZTImage.Log.Trace.Warn("解析json时出现错误,json:" + json);
            }
            return userInfo;
        }


        /// <summary>
        /// 拉取用户信息，通过授权URL中的code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public WeChatUserInfo GetUserInfoByCode(string code,AuthenticationScope scope)
        {
            string authAccessToken;
            string openID;
            Int32 expiresIn;
            string refreshToken;

            if (!GetAuthenticationAccessToken(code, out authAccessToken, out openID, out expiresIn, out refreshToken))
            {
                return null;
            }

            if (scope == AuthenticationScope.snsapi_base)
            {
                return new WeChatUserInfo() { openid = openID };
            }

            return GetUserInfoByAuthAccessToken(openID, authAccessToken);
        }


        /// <summary>
        /// 得到微信临时二维码字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public QRReturnModel GetTempQRString(string content,Int64 expireTime= 2592000)
        {
            return GetQRString(true, content, expireTime);
        }

        /// <summary>
        /// 得到微信二维码字符串
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public QRReturnModel GetQRString(string content)
        {
            return GetQRString(false, content, 0);
        }

        /// <summary>
        /// 生成二维码字符串
        /// https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=TOKEN
        /// </summary>
        /// <param name="isTemp"></param>
        /// <param name="content"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        private QRReturnModel GetQRString(bool isTemp,string content,long expireTime= 2592000)
        {
            string messageJson = "{";
            if (isTemp)
            {
                messageJson += "\"expire_seconds\":" + expireTime.ToString()+",";
            }
            messageJson+="\"action_name\":\"";
            if (isTemp)
            {
                messageJson += "QR_STR_SCENE\"";
            }
            else
            {
                messageJson += "QR_LIMIT_STR_SCENE\"";
            }

            messageJson +=",\"action_info\":{\"scene\":{\"scene_str\": \""+content+"\"}}}";

            string posturl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + this.GetAccessToken();
            string returnData = string.Empty;
            try
            {
                returnData = ZTImage.HttpEx.SyncPost(posturl, messageJson, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error("调用二维码生成失败", ex);
                return null;
            }

            QRReturnModel retModel = ZTImage.Json.JsonParser.ToObject< QRReturnModel>(returnData);
            if (retModel == null)
            {
                ZTImage.Log.Trace.Error("调用二维码生成失败返回消息没有成功转化为json,data:" + returnData);
                return null;
            }
            return retModel;
        }

        #endregion

        #region 支付
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
        public PrepayResult UnifiedOrder(TradeType tradeType, string body, string attach, string tradeNo, int totalFee, string clientIP, string productID, string openid)
        {
            if (this.mPaymentProvider == null)
            {
                throw new ArgumentNullException("PaymentProvider未初始化");
            }
            return this.mPaymentProvider.UnifiedOrder(tradeType, body, attach, tradeNo, totalFee, clientIP, productID, openid);
        }

        /// <summary>
        /// 查询订单通过微信订单号
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        public PayQueryResult OrderQueryByTransactionID(string transactionID)
        {
            if (this.mPaymentProvider == null)
            {
                throw new ArgumentNullException("PaymentProvider未初始化");
            }
            return this.mPaymentProvider.OrderQueryByTransactionID(transactionID);
        }

        /// <summary>
        /// 查询订单通过商户订单号
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <returns></returns>
        public PayQueryResult OrderQueryByTradeNo(string outTradeNo)
        {
            if (this.mPaymentProvider == null)
            {
                throw new ArgumentNullException("PaymentProvider未初始化");
            }
            return this.mPaymentProvider.OrderQueryByTradeNo(outTradeNo);
        }

        /// <summary>
        /// 解析支付通知
        /// </summary>
        /// <param name="xml"></param>
        public PayResult ParsePayNotify(string xml)
        {
            if (this.mPaymentProvider == null)
            {
                throw new ArgumentNullException("PaymentProvider未初始化");
            }
            return this.mPaymentProvider.ParsePayNotify(xml);
        }


        /// <summary>
        /// 支付通知返回数据
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GenericPayNotifyReturnString(bool success, string msg)
        {
            if (this.mPaymentProvider == null)
            {
                throw new ArgumentNullException("PaymentProvider未初始化");
            }

            return this.mPaymentProvider.GenericPayNotifyReturnString(success, msg);
        }


        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string CalcSign(SortedDictionary<string, string> parameters)
        {
            return this.mPaymentProvider.CalcSign(parameters);
        }
        #endregion

        #region JSAPI
        /// <summary>
        /// js api调用时，向config里传递的签名
        /// </summary>
        /// <param name="noncestr">随机字符串，与wx.config里的相同</param>
        /// <param name="timestamp">时间戳，与wx.config里的相同</param>
        /// <param name="url">完整url,不包含#及其后的部分</param>
        /// <returns></returns>
        public string JSSignature(string noncestr,Int64 timestamp,string url)
        {

            string query = string.Format("noncestr={0}&jsapi_ticket={1}&timestamp={2}&url={3}",
                noncestr,
                GetJSAPITicket(),
                timestamp,
                url);

            return ZTImage.Security.Cryptography.SHA1.Encrypt(query);
        }
        #endregion

    }
}
