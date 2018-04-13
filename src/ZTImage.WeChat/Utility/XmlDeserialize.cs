using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZTImage.Reflection.Reflector;

namespace ZTImage.WeChat.Utility
{
    internal class XmlDeserialize
    {
        private string mXml;
        private XmlDocument mDocument = new XmlDocument();

        public XmlDeserialize(string xml)
        {
            this.mXml = xml;
            InitXml();
        }
        /// <summary>
        /// xml填充对象
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public T FillModel<T>() where T:class,new()
        {
            KubiuReflector reflector = KubiuReflector.Cache(typeof(T), false);
            T model = reflector.NewObject() as T;
            FillModel(model);
            return model;
        }

        /// <summary>
        /// xml填充对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void FillModel(object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("要填充对象为空");
            }

            KubiuReflector reflector = KubiuReflector.Cache(model.GetType(), false);
            string key = string.Empty;
            foreach (var property in reflector.Properties)
            {
                key = "/xml/" + property.Name;
                if (IsExistNode(key))
                {
                    SetValue(model, property, GetValue(key));
                }
            }
        }

        private void SetValue(object model, ObjectProperty property, string value)
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
        
        #region xml
        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void InitXml()
        {
            try
            {
                mDocument.LoadXml(this.mXml);
            }
            catch (Exception ex)
            {
                ZTImage.Log.Trace.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 判断指定XPath表达式的节点对象的是否存在
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public bool IsExistNode(string xPath)
        {
            XmlNode node = GetNode(xPath);
            if (node != null)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //返回XPath节点
            
            return this.mDocument.SelectSingleNode(xPath);
        }


        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        public string GetValue(string xPath)
        {
            XmlNode node = this.mDocument.SelectSingleNode(xPath);
            if (node == null)
            {
                return string.Empty;
            }
            //返回XPath节点的值
            return node.InnerText;
        }


        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            XmlNode node = this.mDocument.SelectSingleNode(xPath);
            if (node == null)
            {
                return string.Empty;
            }

            //返回XPath节点的属性值
            return node.Attributes[attributeName].Value;
        }
        #endregion

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
