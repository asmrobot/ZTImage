using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Easemob
{
    /// <summary>
    /// Token提供器
    /// </summary>
    internal class TokenProvider
    {
        public TokenProvider(EasemobManager easemobManager)
        {
            mEasemobManager = easemobManager;
            this.mExpiresTime = DateTime.MinValue;
        }

        private EasemobManager mEasemobManager=null;
        private string mAccessToken = string.Empty;
        private DateTime mExpiresTime;//AccessToken过期时间
        private object mGetLocker = new object();

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {

            if (DateTime.Now >= mExpiresTime)
            {
                lock (this.mGetLocker)
                {
                    if (DateTime.Now >= mExpiresTime)
                    {
                        string requestUrl = string.Format(this.mEasemobManager.UrlBase+"token", this.mEasemobManager.OrgName, this.mEasemobManager.AppName);
                        Dictionary<string, string> requestHeader = new Dictionary<string, string>();
                        requestHeader.Add("Content-Type", "application/json");
                        string requestBody = "{\"grant_type\":\"client_credentials\",\"client_id\":\""+this.mEasemobManager.ClientID+"\",\"client_secret\":\""+this.mEasemobManager.ClientSecret+"\"}";
                        string json = ZTImage.HttpEx.SyncPost(requestUrl,requestBody,Encoding.UTF8,requestHeader);
                        Dictionary<string, object> data = ZTImage.Json.JsonParser.ToDictionary(json);
                        if (!data.ContainsKey("access_token") || !data.ContainsKey("expires_in"))
                        {
                            ZTImage.Log.Trace.Error("easemob get access token error");
                            throw new Exception("easemob access token get error");
                        }

                        this.mAccessToken = data["access_token"].ToString();
                        Int32 expires_in = ZTImage.TypeConverter.ObjectToInt(data["expires_in"], 5184000);
                        this.mExpiresTime = DateTime.Now.AddSeconds(expires_in);
                    }
                }
            }
            return mAccessToken;
        }
    }
}
