using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using System.Collections;

namespace ZTImage.Reflection.Reflector
{
    /// <summary>
    /// 生成集合帮助类
    /// </summary>
    public class GenericCollection
    {
        static Dictionary<Type, GenericCollection> Cache = new Dictionary<Type, GenericCollection>();
        public LiteracyNewObject Init;
        public Type ElementType;
        public Type KeyType;

        public static GenericCollection GetList(Type type)
        {
            GenericCollection list;
            if (Cache.TryGetValue(type, out list))
            {
                return list;
            }
            if (type.GetInterface("System.Collections.IList") != typeof(IList))
            {
                return null;
            }
            lock (Cache)
            {
                if (Cache.TryGetValue(type, out list))
                {
                    return list;
                }
                list = new GenericCollection();
                list.Init = ZTReflector.CreateNewObject(type);
                if (type.IsGenericType)
                {
                    list.ElementType = type.GetGenericArguments()[0];
                    list.ElementType = Nullable.GetUnderlyingType(list.ElementType) ?? list.ElementType;
                }
                else if (type.IsArray)
                {
                    list.ElementType = type.GetElementType();
                }
                else
                {
                    list.ElementType = typeof(object);
                }
                Cache.Add(type, list);
                return list;
            }
        }

        public static GenericCollection GetDict(Type type)
        {
            GenericCollection dict;
            if (Cache.TryGetValue(type, out dict))
            {
                return dict;
            }
            lock (Cache)
            {
                if (Cache.TryGetValue(type, out dict))
                {
                    return dict;
                }
                dict = new GenericCollection();
                dict.Init = ZTReflector.CreateNewObject(type);
                if (type.IsGenericType)
                {
                    var ga = type.GetGenericArguments();
                    if (ga.Length > 1)
                    {
                        dict.KeyType = type.GetGenericArguments()[0];
                        dict.ElementType = type.GetGenericArguments()[1];
                        dict.ElementType = Nullable.GetUnderlyingType(dict.ElementType) ?? dict.ElementType;
                    }
                    else
                    {
                        dict.ElementType = typeof(object);
                    }
                }
                else
                {
                    dict.ElementType = typeof(object);
                }
                Cache.Add(type, dict);
                return dict;
            }
        }
    }
}
