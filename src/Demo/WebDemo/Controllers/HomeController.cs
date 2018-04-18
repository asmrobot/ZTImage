using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ZTImage.WeChat;
using ZTImage.WeChat.Events;
using ZTImage.WeChat.Messages;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            AccessTokenProvider atProvider = new AccessTokenProvider("wxf72a46e6e8cc09bc", "1837debfde1a3b7680390c7a84292432");
            
            return Content("AccessToken:"+ atProvider.GetAccessToken(), "text/html");
        }

        private MessageManager Manager;
        public IActionResult Message()
        {
            if (Manager == null)
            {
                Manager = new MessageManager("NkRWYqr3KUR1UfO7");
            }
            if (Request.Method.Equals("GET"))
            {
                //验证
                string signature = _Get("signature");
                string timestamp= _Get("timestamp");
                string nonce = _Get("nonce");
                string echostr = _Get("echostr");
                
                if (Manager.VaildIsOK(timestamp,nonce,signature))
                {
                    return Content(echostr, "text/html");
                }
                return Content("error,not wechat!~", "text/html");
            }
            else
            {
                //消息
                string xml=Request.GetPostData();
                
                PushBase mb= Manager.XmlToMessage(xml);
                ZTImage.Log.Trace.Debug("receive:" + xml);
                if (mb.GetMsgType() == MsgType.text)
                {
                    TextMessage message = mb as TextMessage;
                    if (message.Content.Equals("time", StringComparison.OrdinalIgnoreCase))
                    {
                        //返回时间
                        ZTImage.WeChat.ReplyMessages.ReplyTextMessage reply = new ZTImage.WeChat.ReplyMessages.ReplyTextMessage(mb.ToUserName, mb.FromUserName);
                        reply.Content = "the current time:" + DateTime.Now.ToString();
                        return Content(Manager.ReplyMessageToXml(reply), "text/html");
                    }
                }
                else if (mb.GetMsgType() == MsgType.@event)
                {
                    EventBase eb = mb as EventBase;
                    if (eb.GetEventType() == EventType.subscribe)
                    {
                        //订阅
                        ZTImage.WeChat.ReplyMessages.ReplyTextMessage reply = new ZTImage.WeChat.ReplyMessages.ReplyTextMessage(mb.ToUserName, mb.FromUserName);
                        reply.Content = "抱歉，让您久等了！~";
                        return Content(Manager.ReplyMessageToXml(reply), "text/html");
                    }
                }
                else
                {

                }
                return Content("", "text/html");
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateMenus()
        {

            return Content("ok", "text/html");
        }


        /// <summary>
        /// 得到Query中指定KEY参数的值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string _Get(string key)
        {
            
            StringValues _val = string.Empty;
            if (Request.Query.TryGetValue(key, out _val))
            {
                return String.Join(",", _val);
            }
            return string.Empty;
        }
    }
}
