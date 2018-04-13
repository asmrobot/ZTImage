﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ZTImage.WeChat;
using ZTImage.WeChat.Messages;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            AccessTokenProvider atProvider = new AccessTokenProvider("wx56bb5bd00d687d3d", "02ac6f9db3166a7fd2e0098d1fe4f2ee");
            
            return Content("AccessToken:"+ atProvider.GetAccessToken(), "text/html");
        }

        private MessageManager Manager;
        public IActionResult Message()
        {
            if (Manager == null)
            {
                Manager = new MessageManager("NkRWYqr3KUR1UfO7cKJgxBkNicHD8GrEG3Q6TkvVEyu");
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
            }

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
