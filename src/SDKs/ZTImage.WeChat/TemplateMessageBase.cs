using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
{
    /// <summary>
    /// 模板消息基类
    /// 发送模板消息最好继承这个类
    /// 然后调用AddDataItem来添加模板项值
    /// </summary>
    public abstract class TemplateMessageBase
    {
        private List<Tuple<string, string, string>> items = new List<Tuple<string, string, string>>();

        /// <summary>
        /// 模板ID
        /// </summary>
        public abstract string template_id { get; }

        /// <summary>
        /// 添加模板值项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="color"></param>
        protected void AddDataItem(string key,string value,string color)
        {
            this.items.Add(new Tuple<string, string, string>(key, value, color));
        }


        

        /// <summary>
        /// 得到数据Json
        /// </summary>
        /// <returns></returns>
        public string GetDataJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            for (int i = 0; i < items.Count; i++)
            {
                if (i != 0)
                {
                    builder.Append(",");
                }

                var item = this.items[i];
                builder.Append("\""+item.Item1+"\":{\"value\":\""+item.Item2+"\"");
                if (!string.IsNullOrEmpty(item.Item3))
                {
                    builder.Append(",\"color\":\""+item.Item3+"\"");
                }
                builder.Append("}");

            }
            builder.Append("}");
            return builder.ToString();
        }




    }
}
