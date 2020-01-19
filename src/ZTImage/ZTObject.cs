using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    /// <summary>
    /// 动态对象
    /// </summary>
    public class ZTObject : DynamicObject
    {
        readonly Dictionary<string, object> dic = new Dictionary<string, object>();

        public ZTObject()
        {

        }


        #region DynamicObject成员
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return dic.TryGetValue(binder.Name.ToUpper(), out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dic[binder.Name.ToUpper()] = value;
            return true;
        }
        #endregion



        public Object this[string key]
        {
            get
            {
                return Get<Object>(key, null);
            }
        }

        public Object Get(string key)
        {
            return Get<Object>(key, null);
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public T Get<T>(string key, T defaultVal)
        {
            if (dic.ContainsKey(key.ToUpper()))
            {
                try
                {
                    return (T)dic[key.ToUpper()];
                }
                catch
                {
                    return defaultVal;
                }
            }
            return defaultVal;

        }

        public void Add(string key, object value)
        {
            dic[key.ToUpper()] = value;
        }

        public void Remove(string key)
        {
            if (dic.ContainsKey(key.ToUpper()))
            {
                dic.Remove(key.ToUpper());
            }
        }
    }
}
