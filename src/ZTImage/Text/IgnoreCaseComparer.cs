using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Text
{
    /// <summary>
    /// 忽略大小写的比较器
    /// </summary>
    public class IgnoreCaseComparer : IEqualityComparer<string>
    {
        public static readonly IgnoreCaseComparer Default = new IgnoreCaseComparer();

        public bool Equals(string x, string y)
        {
            return x.Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }
}
