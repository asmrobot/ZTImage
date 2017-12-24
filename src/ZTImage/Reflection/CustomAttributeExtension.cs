using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Reflection
{
    public static class CustomAttributeExtension
    {

        /// <summary>
        /// 得到第一个指定自定义属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metaType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider metaType, bool inherit = false) where T:Attribute
        {
            return metaType.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// 得到所有指定自定义属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metaType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static List<T> GetAttributes<T>(this ICustomAttributeProvider metaType, bool inherit = false) where T : Attribute
        {
            return metaType.GetCustomAttributes(inherit).OfType<T>().ToList();
        }
    }
}
