using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
{
    /// <summary>
    /// AccessToken提供器
    /// </summary>
    public class AccessTokenProvider
    {
        public AccessTokenProvider(string appid,string appsecurity)
        {
            this.mAppID = appid;
            this.mAppSecurity = appsecurity;
            this.mExpiresTime = DateTime.MinValue;
        }
        private string mAccessToken;
        private string mAppID;
        private string mAppSecurity;
        private DateTime mExpiresTime;//AccessToken过期时间
        private string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        private object mGetLocker = new object();

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {

            if (DateTime.Now >= mExpiresTime)
            {
                lock (this.mGetLocker)
                {
                    if (DateTime.Now >= mExpiresTime)
                    {
                        string requestUrl = string.Format(url, this.mAppID, this.mAppSecurity);
                        string json = ZTImage.HttpEx.SyncGet(requestUrl);
                        Dictionary<string, object> data = ZTImage.Json.JsonParser.ToDictionary(json);
                        if (!data.ContainsKey("access_token") || !data.ContainsKey("expires_in"))
                        {
                            ZTImage.Log.Trace.Error("get access token error");
                            throw new Exception("access token get error");
                        }

                        this.mAccessToken = data["access_token"].ToString();
                        Int32 expires_in = ZTImage.TypeConverter.ObjectToInt(data["expires_in"], 7200);
                        this.mExpiresTime = DateTime.Now.AddSeconds(expires_in);
                    }
                }
            }
            return mAccessToken;
        }
    }
}
