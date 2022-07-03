using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public static class UInt64Extensions
    {


        public static string ToZTCode(this UInt64 num)
        {
            string ret = "";
            UInt64 _n = num;
            do
            {
                UInt64 mod = _n % 36;
                ret = StringExtensions.ZTCodes[mod] + ret;
                _n = _n / 36;

            }
            while (_n != 0);
            return ret;
        }



        public static string ToUnorderZTCode(this UInt64 num)
        {
            string ret = "";
            UInt64 _n = num;
            do
            {
                UInt64 mod = _n % 36;
                ret = StringExtensions.Unorder_ZTCodes[mod] + ret;
                _n = _n / 36;

            }
            while (_n != 0);
            return ret;
        }
    }
}
