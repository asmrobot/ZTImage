using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Reflection.Reflector;

namespace ZTImage.Reflection
{
    /// <summary>
    /// 自动变量类
    /// </summary>
    public class AutomiticVariable
    {

        /// <summary>
        /// 用NameValue填充对象
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T FillModel<T>(IDictionary<string,string> collection) where T:class
        {
            ZTReflector reflector = ZTReflector.Cache(typeof(T), true);
            object model = reflector.NewObject();
            FillModel(model, collection);
            return model as T;
        }

        /// <summary>
        /// 用namevalue填充对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object FillModel(object model, IDictionary<string,string> collection)
        {
            if (model == null)
            {
                throw new ArgumentNullException("传递过来的填充对象为空");
            }

            ZTReflector reflector = ZTReflector.Cache(model.GetType(), true);
            foreach (var item in collection)
            {
                if (reflector.Properties.ContainsKey(item.Key))
                {
                    SetValue(model, reflector.Properties[item.Key], item.Value);
                }
            }

            return model;
        }

        /// <summary>
        /// 复制两个对象的值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyValue(object from, object to)
        {
            ZTReflector fromReflector = ZTReflector.Cache(from.GetType(), true);
            ZTReflector toReflector = ZTReflector.Cache(to.GetType(), true);
            foreach (var fromProperty in fromReflector.Properties)
            {
                //目标不包含此属性
                if (!toReflector.Properties.ContainsKey(fromProperty.Name))
                {
                    continue;
                }

                var toProperty = toReflector.Properties[fromProperty.Name];

                object val = null;
                if (!fromProperty.TryGetValue(from, out val))
                {
                    //无法从源中得到值 
                    continue;
                }

                //源和目标的类型是否相同
                if (fromProperty.OriginalType == toProperty.OriginalType)
                {
                    toProperty.TrySetValue(to, val);
                }
                else
                {
                    //类型不同
                    if (val != null)
                    {
                        SetValue(to, toProperty, val.ToString());
                    }
                }
            }
        }

        

        private static void SetValue(object model, ObjectProperty property, string value)
        {
            switch (property.MemberType.Name)
            {
                case "Boolean":
                    ToBoolean(model, property, value);
                    break;
                case "Char":
                    ToChar(model, property, value);
                    break;
                case "SByte":
                    ToSByte(model, property, value);
                    break;
                case "Byte":
                    ToByte(model, property, value);
                    break;
                case "Int16":
                    ToInt16(model, property, value);
                    break;
                case "UInt16":
                    ToUInt16(model, property, value);
                    break;
                case "Int32":
                    ToInt32(model, property, value);
                    break;
                case "UInt32":
                    ToUInt32(model, property, value);
                    break;
                case "Int64":
                    ToInt64(model, property, value);
                    break;
                case "UInt64":
                    ToUInt64(model, property, value);
                    break;
                case "Single":
                    ToSingle(model, property, value);
                    break;
                case "Double":
                    ToDouble(model, property, value);
                    break;
                case "Decimal":
                    ToDecimal(model, property, value);
                    break;
                case "DateTime":
                    ToDateTime(model, property, value);
                    break;
                case "Guid":
                    ToGuid(model, property, value);
                    break;
                default:
                    ToDefault(model, property, value);
                    break;
            }
        }


        #region convert

        private static void ToDefault(object model, ObjectProperty property, string val)
        {
            property.TrySetValue(model, val);
        }

        private static void ToBoolean(object model, ObjectProperty property, string val)
        {
            Boolean setter = false;
            if (!Boolean.TryParse(val, out setter))
            {
                int i = 0;
                if (Int32.TryParse(val, out i))
                {
                    if (i > 0)
                    {
                        setter = true;
                    }
                }
            }
            property.TrySetValue(model, setter);
        }

        private static void ToGuid(object model, ObjectProperty property, string val)
        {
            Guid setter;
            if (Guid.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }

        }

        private static void ToChar(object model, ObjectProperty property, string val)
        {
            Char setter;
            if (Char.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToSByte(object model, ObjectProperty property, string val)
        {
            SByte setter;
            if (SByte.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToByte(object model, ObjectProperty property, string val)
        {
            Byte setter;
            if (Byte.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt16(object model, ObjectProperty property, string val)
        {
            Int16 setter;
            if (Int16.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt16(object model, ObjectProperty property, string val)
        {
            UInt32 setter;
            if (UInt32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt32(object model, ObjectProperty property, string val)
        {
            Int32 setter;
            if (Int32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt32(object model, ObjectProperty property, string val)
        {
            UInt32 setter;
            if (UInt32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt64(object model, ObjectProperty property, string val)
        {
            Int64 setter;
            if (Int64.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt64(object model, ObjectProperty property, string val)
        {
            UInt64 setter;
            if (UInt64.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToSingle(object model, ObjectProperty property, string val)
        {
            Single setter;
            if (Single.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDouble(object model, ObjectProperty property, string val)
        {
            Double setter;
            if (Double.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDecimal(object model, ObjectProperty property, string val)
        {
            Decimal setter;
            if (Decimal.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDateTime(object model, ObjectProperty property, string val)
        {
            DateTime setter;
            if (DateTime.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }
        #endregion
    }
}
