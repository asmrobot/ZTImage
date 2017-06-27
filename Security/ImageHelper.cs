using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Security
{
    /// <summary>
    /// logistic映射
    /// 可用于图像加密
    /// </summary>
    public class Logistic
    {
        private const double CONST_U = 3.62;
        private const double CONST_X0 = 0.314159;

        public static byte[] Encrypt(byte[] data)
        {
            return Encrypt(data, CONST_U, CONST_X0);
        }
        
        /// <summary>
        /// Logistic模型：X_n+1=u*Xn(1-Xn)
        /// 基于Logistic模型的混沌加解密
        /// </summary>
        /// <param name="data">要处理的数据</param>
        /// <param name="u">应属于[3.57,4]</param>
        /// <param name="x0">应属于(0,1)</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, double u, double x0)
        {
            byte[] res = new byte[data.Length];
            double x = logistic(u, x0, 2000);

            for (int i = 0; i < data.Length; i++)
            {
                x = logistic(u, x, 5);                
                res[i] = Convert.ToByte((Int32)Math.Floor(x * 1000) % 256 ^ data[i]);//取x小数点后3位来生成密钥
            }
            return res;
        }

        private static double logistic(double u, double x, int n)
        {
            for (int i = 0; i < n; i++)
            {
                x = u * x * (1 - x);
            }
            return x;
        }
    }
}
