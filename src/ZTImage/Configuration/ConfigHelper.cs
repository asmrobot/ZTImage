using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZTImage.Reflection;


namespace ZTImage.Configuration
{
    /// <summary>
    /// 配置帮助类
    /// 要实现配置实体，必须标注ConfigPathAttribute属性
    /// </summary>
    public class ConfigHelper
    {
        static ConfigHelper()
        {
            m_PathCache = new Dictionary<Type, string>();
            m_ConfigCache = new Dictionary<Type, ConfigCacheHelper>();
        }


        #region Field Members
        
        private static object lockHelper = new object();
        private static Dictionary<Type, string> m_PathCache;//路径缓存
        private static Dictionary<Type, ConfigCacheHelper> m_ConfigCache;//配置缓存
        #endregion

        #region Instance Members
        /// <summary>
        /// 确定配置在缓存里,并返回配置路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string EnsurePathCache(Type type)
        {
            if (m_PathCache.ContainsKey(type))
            {
                return m_PathCache[type];
            }
            var pathAttribute = type.GetAttribute<ConfigPathAttribute>(true);
            if (pathAttribute == null)
            {
                throw new ArgumentOutOfRangeException("配置必须有一个路径属性(Kubiu.Configuration.ConfigPathAttribute)");
            }
            if (string.IsNullOrEmpty(pathAttribute.Path))
            {
                throw new ArgumentOutOfRangeException("配置的路径属性为空");
            }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathAttribute.Path.TrimStart("\\/".ToCharArray()));
            
            m_PathCache[type] =path;
            return path;
        }


        /// <summary>
        /// 设置配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public static void SetInstance<T>(T info) where T : new()
        {   
            lock (lockHelper)
            {
                XmlSerializeHelper.Save(EnsurePathCache(info.GetType()), info);
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class,new() 
        {
            var t = typeof(T);
            string path = EnsurePathCache(t);
            
            DateTime lastWriteTime = File.GetLastWriteTime(path);
            
            
            
            ConfigCacheHelper configCache = null;
            if (m_ConfigCache.ContainsKey(t))
            {
                configCache = m_ConfigCache[t];
            }


            if (configCache == null || configCache.LastCacheTime != lastWriteTime)
            {
                lock (lockHelper)
                {
                    T config = XmlSerializeHelper.Load<T>(path);
                    configCache= new ConfigCacheHelper(lastWriteTime, config);
                    m_ConfigCache[t] = configCache;

                }
            }

            return configCache.Config as T;
        }
        #endregion



    }
}
