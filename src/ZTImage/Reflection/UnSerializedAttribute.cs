using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Reflection
{
    /// <summary>
    /// 不序列化属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class UnSerializedAttribute : Attribute
    {
    }
}
