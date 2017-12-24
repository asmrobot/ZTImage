using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Reflection
{
    /// <summary>
    /// 元素顺序
    /// </summary>
    [AttributeUsage(AttributeTargets.Field |AttributeTargets.Property )]
    public class PropertyOrderAttribute:Attribute
    {
        public int Order
        {
            get;
            set;
        }
        public PropertyOrderAttribute(int order)
        {
            this.Order = order;
        }
    }
}
