using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// 编码为base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeBase64(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }


        /// <summary>
        /// 编码为URL安全的base64
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string EncodeUrlSafeBase64(this byte[] content)
        {
            string base64 = Convert.ToBase64String(content);
            return base64.CommonBase64ToUrlSafeBase64();
        }

    }
}
