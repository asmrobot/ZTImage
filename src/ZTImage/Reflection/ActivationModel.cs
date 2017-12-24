using ZTImage.Reflection.Reflector;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Reflection
{
    public class ActivationModel<T> where T :class,new()
    {

        /// <summary>
        /// 用NameValue填充对象
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T FillModel(NameValueCollection collection)
        {
            KubiuReflector reflector = KubiuReflector.Cache(typeof(T), false);
            T model = reflector.NewObject() as T;
            return FillModel(model, collection);
        }

        /// <summary>
        /// 用namevalue填充对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T FillModel(T model, NameValueCollection collection)
        {
            if (model == null)
            {
                throw new ArgumentNullException("传递过来的填充对象为空");
            }

            KubiuReflector reflector = KubiuReflector.Cache(typeof(T), false);
            foreach (var property in reflector.Properties)
            {
                if (collection.AllKeys.Contains(property.Name))
                {
                    SetValue(model, property, collection[property.Name]);
                }
            }

            return model;
        }

        private static void SetValue(T model, ObjectProperty property, string value)
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

        private static void ToDefault(T model, ObjectProperty property, string val)
        {
            property.TrySetValue(model, val);
        }

        private static void ToBoolean(T model, ObjectProperty property, string val)
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

        private static void ToGuid(T model, ObjectProperty property, string val)
        {
            Guid setter;
            if (Guid.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }

        }

        private static void ToChar(T model, ObjectProperty property, string val)
        {
            Char setter;
            if (Char.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToSByte(T model, ObjectProperty property, string val)
        {
            SByte setter;
            if (SByte.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToByte(T model, ObjectProperty property, string val)
        {
            Byte setter;
            if (Byte.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt16(T model, ObjectProperty property, string val)
        {
            Int16 setter;
            if (Int16.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt16(T model, ObjectProperty property, string val)
        {
            UInt32 setter;
            if (UInt32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt32(T model, ObjectProperty property, string val)
        {
            Int32 setter;
            if (Int32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt32(T model, ObjectProperty property, string val)
        {
            UInt32 setter;
            if (UInt32.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToInt64(T model, ObjectProperty property, string val)
        {
            Int64 setter;
            if (Int64.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToUInt64(T model, ObjectProperty property, string val)
        {
            UInt64 setter;
            if (UInt64.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToSingle(T model, ObjectProperty property, string val)
        {
            Single setter;
            if (Single.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDouble(T model, ObjectProperty property, string val)
        {
            Double setter;
            if (Double.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDecimal(T model, ObjectProperty property, string val)
        {
            Decimal setter;
            if (Decimal.TryParse(val, out setter))
            {
                property.TrySetValue(model, setter);
            }
        }

        private static void ToDateTime(T model, ObjectProperty property, string val)
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
