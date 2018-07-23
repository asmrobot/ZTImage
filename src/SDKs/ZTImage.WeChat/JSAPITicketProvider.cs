using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
{
    /// <summary>
    /// JSAPI Ticket 提供器
    /// </summary>
    public class JSAPITicketProvider
    {
        
        public JSAPITicketProvider(AccessTokenProvider provider)
        {
            this.mAccessTokenProvider = provider;
        }

        private AccessTokenProvider mAccessTokenProvider;
        private DateTime mExpiresTime;//jsapi ticket过期时间
        private string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
        private object mGetLocker = new object();
        private string mTicket = string.Empty;
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetTicket()
        {

            if (DateTime.Now >= mExpiresTime)
            {
                lock (this.mGetLocker)
                {
                    if (DateTime.Now >= mExpiresTime)
                    {
                        string requestUrl = string.Format(url, this.mAccessTokenProvider.GetAccessToken());
                        string json = ZTImage.HttpEx.SyncGet(requestUrl);
                        Dictionary<string, object> data = ZTImage.Json.JsonParser.ToDictionary(json);
                        if (!data.ContainsKey("ticket") || !data.ContainsKey("expires_in"))
                        {
                            ZTImage.Log.Trace.Error("get jsapi ticket error");
                            throw new Exception("jsapi ticket get error");
                        }

                        this.mTicket = data["ticket"].ToString();
                        Int32 expires_in = ZTImage.TypeConverter.ObjectToInt(data["expires_in"], 7200);
                        this.mExpiresTime = DateTime.Now.AddSeconds(expires_in);
                    }
                }
            }
            return this.mTicket;
        }


        /// <summary>
        /// 设置ticket过期
        /// </summary>
        /// <returns></returns>
        public void SetExpire()
        {
            mExpiresTime = DateTime.Now;
        }
    }
}
