using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public struct FPoint
    {
        public FPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 经度
        /// </summary>
        public float x;

        /// <summary>
        /// 纬度
        /// </summary>
        public float y;

        public DPoint ToDPoint()
        {
            return new DPoint(this.x, this.y);
        }


        public static FPoint Empty = new FPoint(0, 0);
    }
}
