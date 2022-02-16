using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public static class DecimalExtensions
    {
        public static double Ceiling(this decimal val, int bit)
        {
            return decimal.ToDouble(Math.Round(val, bit));
        }
    }
}
