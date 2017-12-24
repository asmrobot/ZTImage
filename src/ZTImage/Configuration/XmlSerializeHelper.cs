using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;


namespace ZTImage.Configuration
{
    /// <summary>
    /// 序列化工具类
    /// </summary>
    internal class XmlSerializeHelper
    {
        /// <summary>
        /// 将对象序列化成文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static bool Save(string path, object obj)
        {
            FileStream fs = null;
            XmlTextWriter xtw = null;
            try
            {
                File.Delete(path);
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                xtw = new XmlTextWriter(fs, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;

                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(xtw, obj);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }


        /// <summary>
        /// 从文件中序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path)
        {

            FileStream fs = null;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}

