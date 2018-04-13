using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public MessageBase ParseToMessage(string xml)
        {
            XmlDeserialize deserialize = new XmlDeserialize(xml);
            string msgtype = deserialize.GetValue("/xml/MsgType").ToLower();
            MessageBase message = null;
            switch (msgtype)
            {
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
                default:
                    return null;
            }

            deserialize.FillModel(message);
            return message;
        }

    }
}
