using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.WeChat.Events;
using ZTImage.WeChat.Menus;
using ZTImage.WeChat.Messages;
using ZTImage.WeChat.Utility;

namespace ZTImage.WeChat
{
    public class WeChatManager
    {
        private string mToken;
        private string mAppID;
        private string mAppSecurity;
        private AccessTokenProvider mTokenProvider;

        public WeChatManager(string token,string appID,string appSecurity)
        {
            this.mToken = token;
            this.mAppID = appID;
            this.mAppSecurity = appSecurity;
            mTokenProvider = new AccessTokenProvider(this.mAppID, this.mAppSecurity);
        }

        /// <summary>
        /// 得到access token
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            return mTokenProvider.GetAccessToken();
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
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + this.mTokenProvider.GetAccessToken();
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

            string posturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.mTokenProvider.GetAccessToken();
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
        
    }
}
