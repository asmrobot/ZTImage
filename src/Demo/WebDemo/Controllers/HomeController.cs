using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using WebDemo.ViewModels.Home;
using ZTImage.WeChat;
using ZTImage.WeChat.Events;
using ZTImage.WeChat.Menus;
using ZTImage.WeChat.Messages;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private static WeChatManager Manager;

        static HomeController()
        {
            //Manager = new WeChatManager("NkRWYqr3KUR1UfO7", "wx56bb5bd00d687d3d", "02ac6f9db3166a7fd2e0098d1fe4f2ee");
        }

        public IActionResult Index()
        {
            return Content("AccessToken", "text/html");
        }

        
        public IActionResult Message()
        {
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
            
            List<MenuBase> menus = new List<MenuBase>();
            menus.Add( new ClickMenu("咨询o医生","v100_001"));
            MenuGroup group = new MenuGroup("我");
            group.AddMenu(new ClickMenu("设置o密码", "v200_001"));
            group.AddMenu(new ViewMenu("查看o信息", "http://wechat.ztimage.com/Home/Login"));
            menus.Add(group);


            menus.Add(new ViewMenu("查看o须知", "http://wechat.ztimage.com/Home/Login"));

            bool ret = Manager.CreateMenus(menus);
            if (ret)
            {
                return Content("ok", "text/html");
            }
            else
            {
                return Content("failed", "text/html");
            }
        }

        public IActionResult WechatAuth()
        {
            string code = _Get("code");
            string state = _Get("state");
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            {
                ZTImage.Log.Trace.Warn( Request.QueryString.ToString());
                return Content("authentication error");
            }
            WeChatUserInfo user=Manager.GetUserInfoByCode(code, AuthenticationScope.snsapi_userinfo);
            if (user == null)
            {
                ZTImage.Log.Trace.Warn("user is null" + Request.QueryString.ToString());
                return Content("authentication error");
            }
            
            string rurl = ZTImage.Text.Coding.Unescape(state);
            return Redirect(rurl + "?openid="+user.openid);
        }
        /// <summary>
        /// 登陆获取个人信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            Login model = new Login();
            model.OpenID = _Get("openid");
            if (string.IsNullOrEmpty(model.OpenID))
            {
                string redUrl = Manager.GetAuthUrl("http://wechat.ztimage.com/home/WechatAuth", AuthenticationScope.snsapi_userinfo, ZTImage.Text.Coding.Escape("/home/login"));
                return Redirect(redUrl);
            }
            
            return View(model);
        }


        public IActionResult QR()
        {
            var model=Manager.GetQRString(Guid.NewGuid().ToString());

            return Content("ticket:"+model.ticket+",url"+model.url, "text/html");
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
