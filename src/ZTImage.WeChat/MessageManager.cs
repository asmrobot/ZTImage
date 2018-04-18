using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.WeChat.Events;
using ZTImage.WeChat.Messages;
using ZTImage.WeChat.Utility;

namespace ZTImage.WeChat
{
    public class MessageManager
    {
        private string mToken;
        public MessageManager(string token)
        {
            this.mToken = token;
        }
        

        /// <summary>
        /// 验证是否通过
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
        
    }
}
