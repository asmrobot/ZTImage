using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Easemob
{
    public class EasemobManager
    {
        public readonly string OrgName=string.Empty;
        public readonly string AppName = string.Empty;
        public readonly string ClientID = string.Empty;
        public readonly string ClientSecret = string.Empty;

        private TokenProvider mTokenProvider = null;
        public readonly string UrlBase = string.Empty;


        public EasemobManager(string orgName,string appName,string clientID,string clientSecret)
        {
            this.OrgName = orgName;
            this.AppName = appName;
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            this.UrlBase = string.Format("https://a1.easemob.com/{0}/{1}/", this.OrgName, this.AppName);
            init();
        }


        private void init()
        {
            mTokenProvider = new TokenProvider(this);
        }

        /// <summary>
        /// 得到请求token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            return this.mTokenProvider.GetToken();
        }

        #region 用户相关
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool RegisterUser(string userName,string password)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer "+this.mTokenProvider.GetToken());

            string requestBody = "{\"username\":\""+userName+"\",\"password\":\""+password+"\"}";
            string json=HttpEx.SyncPost(UrlBase + "users", requestBody, Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 批量注册用户
        /// </summary>
        /// <param name="users">username-password list</param>
        /// <returns></returns>
        public bool RegisterUsers(List<Tuple<string, string>> users)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string requestBody ="[";
            foreach (var item in users)
            {
                if (requestBody.Length > 1)
                {
                    requestBody += ",";
                }
                requestBody += "{\"username\":\"" + item.Item1 + "\",\"password\":\"" + item.Item2 + "\"}";
            }
            requestBody += "]";
            string json = HttpEx.SyncPost(UrlBase + "users", requestBody, Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 得到用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetUser(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());
            string json = HttpEx.SyncGet(UrlBase + "users/"+userName, headers);
            return json;
        }

        /// <summary>
        /// 批量得到用户
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public string GetUsers(Int32 limit, string cursor)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());
            string requestPar = "?limit=" + limit.ToString();
            if (!string.IsNullOrEmpty(cursor))
            {
                requestPar += "&cursor="+cursor;
            }
            string json = HttpEx.SyncGet(UrlBase + "users"+requestPar, headers);
            return json;
        }

        /// <summary>
        /// 删除单个用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool DeleteUser(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());
            string json = HttpEx.SyncDelete(UrlBase + "users/" + userName, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除指定数量的用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string DeleteUsers(int count)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());
            string json = HttpEx.SyncDelete(UrlBase + "users?limit="+count.ToString());
            return json;
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ResetUserPassword(string userName, string newPassword)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string requestBody = "{\"newpassword\":\"" + newPassword + "\"}";
            string json = HttpEx.SyncPut(UrlBase + "users/"+userName+"/password", requestBody, Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置用户昵称
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="nick"></param>
        /// <returns></returns>
        public bool SetUserNick(string userName, string nick)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string requestBody = "{\"nickname\":\"" + nick + "\"}";
            string json = HttpEx.SyncPut(UrlBase + "users/" + userName, requestBody, Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 为用户添加好友
        /// </summary>
        /// <param name="ownerUserName"></param>
        /// <param name="friendUserName"></param>
        /// <returns></returns>
        public bool AddContacts(string ownerUserName, string friendUserName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            
            string json = HttpEx.SyncPost(UrlBase + "users/" + ownerUserName+ "/contacts/users/"+friendUserName, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移除好友关系
        /// </summary>
        /// <param name="ownerUserName"></param>
        /// <param name="friendUserName"></param>
        /// <returns></returns>
        public bool RemoveContacts(string ownerUserName, string friendUserName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncDelete(UrlBase + "users/" + ownerUserName + "/contacts/users/" + friendUserName, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="ownerUserName"></param>
        /// <returns></returns>
        public List<string> GetContacts(string ownerUserName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncGet(UrlBase + "users/" + ownerUserName + "/contacts/users", headers);
            if (string.IsNullOrEmpty(json))
            {
                return new List<string>();
            }


            Dictionary<string, object> entities = null;
            try
            {
                entities = ZTImage.Json.JsonParser.ToDictionary(json);
            }
            catch (Exception ex)
            {
                return new List<string>();
            }

            if (!entities.ContainsKey("data"))
            {
                return new List<string>();
            }

            if (entities["data"] == null)
            {
                return new List<string>();
            }

            object[] names = entities["data"] as object[];
            if (names == null)
            {
                return new List<string>();
            }
            List<string> _names = new List<string>();
            foreach (var item in names)
            {
                _names.Add(item.ToString());
            }
            return _names;
        }

        /// <summary>
        /// 添加用户到黑名单
        /// </summary>
        /// <param name="ownerUserName"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public bool AddToBlacklist(string ownerUserName, List<string> users)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string requestBody = "{\"usernames\":[";

            foreach (var item in users)
            {
                requestBody += "\""+item+"\"";
            }
            requestBody += "]}";


            string json = HttpEx.SyncPost(UrlBase + "users/" + ownerUserName + "/blocks/users", headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从黑名单里删人
        /// </summary>
        /// <param name="ownerUserName"></param>
        /// <param name="blackedUserName"></param>
        /// <returns></returns>
        public bool RemoveFromBlacklist(string ownerUserName, string blackedUserName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncDelete(UrlBase + "users/" + ownerUserName + "/blocks/users/" + blackedUserName, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 得到黑名单
        /// </summary>
        /// <param name="ownerUserName"></param>
        /// <returns></returns>
        public List<string> GetBlacklist(string ownerUserName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncGet(UrlBase + "users/" + ownerUserName + "/blocks/users", headers);
            if (string.IsNullOrEmpty(json))
            {
                return new List<string>();
            }


            Dictionary<string, object> entities = null;
            try
            {
                entities = ZTImage.Json.JsonParser.ToDictionary(json);
            }
            catch (Exception ex)
            {
                return new List<string>();
            }

            if (!entities.ContainsKey("data"))
            {
                return new List<string>();
            }

            if (entities["data"] == null)
            {
                return new List<string>();
            }

            object[] names = entities["data"] as object[];
            if (names == null)
            {
                return new List<string>();
            }
            List<string> _names = new List<string>();
            foreach (var item in names)
            {
                _names.Add(item.ToString());
            }
            return _names;
        }

        /// <summary>
        /// 查询用户是否在线
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsOnline(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());
            
            string json = HttpEx.SyncGet(UrlBase + "users/" + userName + "/status", headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }
            
            Dictionary<string, object> entities = null;
            try
            {
                entities = ZTImage.Json.JsonParser.ToDictionary(json);
            }
            catch (Exception ex)
            {
                return false;
            }

            if (!entities.ContainsKey("data"))
            {
                return false;
            }

            if (entities["data"] == null)
            {
                return false;
            }

            Dictionary<string, object> names = entities["data"] as Dictionary<string,object>;
            if (names == null)
            {
                return false;
            }

            if (!names.ContainsKey(userName))
            {
                return false;
            }

            if (names[userName] == null)
            {
                return false;
            }

            string val = names[userName].ToString();
            if (val.Equals("online", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 得到离线消息数目
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Int32 GetOfflineMessageCount(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string json = HttpEx.SyncGet(UrlBase + "users/" + userName + "/offline_msg_count", headers);
            if (string.IsNullOrEmpty(json))
            {
                return 0;
            }

            Dictionary<string, object> entities = null;
            try
            {
                entities = ZTImage.Json.JsonParser.ToDictionary(json);
            }
            catch (Exception ex)
            {
                return 0;
            }

            if (!entities.ContainsKey("data"))
            {
                return 0;
            }

            if (entities["data"] == null)
            {
                return 0;
            }

            Dictionary<string, object> names = entities["data"] as Dictionary<string, object>;
            if (names == null)
            {
                return 0;
            }

            if (!names.ContainsKey(userName))
            {
                return 0;
            }

            if (names[userName] == null)
            {
                return 0;
            }

            return ZTImage.TypeConverter.ObjectToInt(names[userName], 0);
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool DisableUser(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncPost(UrlBase + "users/" + userName + "/deactivate", headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool EnableUser(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncPost(UrlBase + "users/" + userName + "/activate", headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ForceOffline(string userName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());


            string json = HttpEx.SyncGet(UrlBase + "users/" + userName + "/disconnect", headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 聊天记录相关

        #endregion

        #region 文件相关
        #endregion

        #region 消息相关
        /// <summary>
        /// 得到发送目标字符串表示
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private string GetTargetType(MessageTargetType t)
        {
            switch (t)
            {
                case MessageTargetType.chatgroups:
                    return "chatgroups";
                case MessageTargetType.chatrooms:
                    return "chatrooms";
                case MessageTargetType.users:
                    return "users";
                default:
                    return "users";
            }
        }

        /// <summary>
        /// 连接字符串数组为json数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ConcatStringToJsonArray(List<string> array)
        {
            string json = "[";
            for (int i = 0; i < array.Count; i++)
            {
                if (i != 0)
                {
                    json += ",";
                }
                json += "\""+array[i]+"\"";
            }

            json += "]";
            return json;
        }

        /// <summary>
        /// 包裹消息
        /// </summary>
        /// <param name="targetType">目标类型</param>
        /// <param name="targets">接收用户列表</param>
        /// <param name="from">发送者，可为空</param>
        /// <param name="messageJson">消息</param>
        /// <param name="ex">附加消息,可为空</param>
        /// <returns></returns>
        private string WarpMessageWarp(MessageTargetType targetType, List<string> targets,string from,string messageJson, object ex = null)
        {
            string requestBody = "{";
            requestBody += "\"target_type\" : \"" + GetTargetType(targetType) + "\",";
            requestBody += "\"target\" : " + ConcatStringToJsonArray(targets) + ",";
            requestBody += "\"msg\":"+messageJson;
            if (string.IsNullOrEmpty(from))
            {
                requestBody += ",\"from\":\""+from+"\"";
            }
            if (ex != null)
            {
                requestBody += ",\"ext\":"+Json.JsonBuilder.ToJsonString(ex);
            }
            requestBody += "}";

            return requestBody;
        }

        /// <summary>
        /// 发送文件消息
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="targets"></param>
        /// <param name="message"></param>
        /// <param name="from"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool SendTextMessage(MessageTargetType targetType, List<string> targets, string message, string from, object ex = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string messageJson = "{\"type\":\"txt\",\"msg\":\""+message+"\"}";
            
            string json = HttpEx.SyncPost(UrlBase + "messages", WarpMessageWarp(targetType,targets,from,messageJson,ex), Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="targets"></param>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <param name="secret"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="from"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool SendImageMessage(MessageTargetType targetType, List<string> targets,string url,string filename,string secret,Int32 width,Int32 height, string from, object ex = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string messageJson = "{\"type\":\"img\",\"url\":\"" + url + "\",\"filename\":\""+filename+ "\",\"secret\":\""+secret+ "\"\"size\":{\"width\":"+width.ToString()+",\"height\":"+height+"}}";
            string json = HttpEx.SyncPost(UrlBase + "messages", WarpMessageWarp(targetType, targets, from, messageJson, ex), Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="targets"></param>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <param name="secret"></param>
        /// <param name="length"></param>
        /// <param name="thumb"></param>
        /// <param name="fileLength"></param>
        /// <param name="thumbSecret"></param>
        /// <param name="from"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool SendVideoMessage(MessageTargetType targetType, List<string> targets, string url, string filename, string secret, Int32 length,string thumb,Int32 fileLength,string thumbSecret, string from, object ex = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string messageJson = "{\"type\":\"video\",\"url\":\"" + url + "\",\"filename\":\"" + filename + "\",\"secret\":\"" + secret + "\"\"length\":"+length.ToString()+",\"thumb\":\""+thumb+ "\",\"file_length\":"+fileLength.ToString()+ ",,\"thumb_secret\":\""+thumbSecret+"\"}";
            string json = HttpEx.SyncPost(UrlBase + "messages", WarpMessageWarp(targetType, targets, from, messageJson, ex), Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 发送音频消息
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="targets"></param>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <param name="secret"></param>
        /// <param name="length"></param>
        /// <param name="from"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool SendAudioMessage(MessageTargetType targetType, List<string> targets, string url, string filename, string secret, Int32 length, string from, object ex = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string messageJson = "{\"type\":\"audio\",\"url\":\"" + url + "\",\"filename\":\"" + filename + "\",\"secret\":\"" + secret + "\"\"length\":" + length.ToString() + "}";
            string json = HttpEx.SyncPost(UrlBase + "messages", WarpMessageWarp(targetType, targets, from, messageJson, ex), Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }



        /// <summary>
        /// 发送透视频消息
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="targets"></param>
        /// <param name="action"></param>
        /// <param name="from"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool SendCommandMessage(MessageTargetType targetType, List<string> targets, string action,string from, object ex = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", "Bearer " + this.mTokenProvider.GetToken());

            string messageJson = "{\"type\":\"cmd\",\"action\":\"" + action + "\"}";
            string json = HttpEx.SyncPost(UrlBase + "messages", WarpMessageWarp(targetType, targets, from, messageJson, ex), Encoding.UTF8, headers);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 群组管理
        #endregion
    }
}
