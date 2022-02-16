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
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public ZTObject()
        {

        }


        #region DynamicObject成员
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name.ToUpper(), out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name.ToUpper()] = value;
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
            if (_dictionary.ContainsKey(key.ToUpper()))
            {
                try
                {
                    return (T)_dictionary[key.ToUpper()];
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
            _dictionary[key.ToUpper()] = value;
        }

        public void Remove(string key)
        {
            if (_dictionary.ContainsKey(key.ToUpper()))
            {
                _dictionary.Remove(key.ToUpper());
            }
        }
    }
}
