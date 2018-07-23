﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
{
    /// <summary>
    /// 授权类型
    /// </summary>
    public enum AuthenticationScope
    {
        /// <summary>
        /// 只得到openid
        /// 以snsapi_base为scope发起的网页授权，是用来获取进入页面的用户的openid的，
        /// 并且是静默授权并自动跳转到回调页的。用户感知的就是直接进入了回调页（往往是业务页面）
        /// </summary>
        snsapi_base = 0,

        /// <summary>
        /// 以snsapi_userinfo为scope发起的网页授权，是用来获取用户的基本信息的。
        /// 但这种授权需要用户手动同意，并且由于用户同意过，所以无须关注，就可在授权后获取该用户的基本信息。
        /// </summary>
        snsapi_userinfo = 1//得到用户基本信息
    }
}
