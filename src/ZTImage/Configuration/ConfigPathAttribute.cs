using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Configuration
{
    /// <summary>
    /// 配置保存路径，开头不能以‘/’开始，
    /// </summary>
    [AttributeUsage (AttributeTargets.Class )]
    public class ConfigPathAttribute:Attribute 
    {
        public string Path
        {
            get;
            private set;
        }
        
        public ConfigPathAttribute(params string[] path)
        {
            if (path == null || path.Length <= 0)
            {
                throw new ArgumentNullException("Path变量不能为空");
            }
            for (int i = 0; i < path.Length; i++)
            {
                if (string.IsNullOrEmpty(path[i]))
                {
                    throw new ArgumentOutOfRangeException("第"+i.ToString()+"位路径不能为空");
                }
            }
            
            this.Path = System.IO.Path.Combine(path);
        }
    }
}
