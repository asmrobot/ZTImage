using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public static class DoubleExtensions
    {
        public static double Ceiling(this double val, int bit)
        {
            return Math.Round(val, bit);
        }

        
    }
}
