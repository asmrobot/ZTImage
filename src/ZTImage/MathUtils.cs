using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public class MathUtils
    {

        /// <summary>
        /// 舍弃小数位,i为保留的位数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double Ceiling(double d, uint i)
        {
            string str = d.ToString();

            int offset = str.IndexOf('.');
            if (offset <= -1)
            {
                return d;
            }

            if ((str.Length - offset - 1) <= i)
            {
                return d;
            }

            int length = offset + (int)i;
            if (i != 0)
            {
                length += 1;
            }
            str = str.Substring(0, length);
            return double.Parse(str);
        }

    }
}
