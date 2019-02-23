using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{

    public struct DPoint
    {
        public DPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 经度
        /// </summary>
        public double x;

        /// <summary>
        /// 纬度
        /// </summary>
        public double y;


        public FPoint ToFPoint()
        {
            return new FPoint((float)this.x, (float)this.y);
        }

        public override string ToString()
        {
            return "x:" + x.ToString() + ",y:" + this.y.ToString();
        }
        
        
    }
}
