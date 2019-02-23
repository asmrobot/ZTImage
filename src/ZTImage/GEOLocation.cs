using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    /// <summary>
    /// 地理位置帮助类
    /// </summary>
    public class GEOLocation:ZTShape
    {

        
        /// <summary>
        /// 极半径
        /// 从地心到北极或南极的距离，大约3950英里(6356.9088千米)（两极的差极小，可以忽略）。
        /// </summary>
        public const double EarthJIRadius = 6356.9088;

        /// <summary>
        /// 赤道半径
        /// 是从地心到赤道的距离，大约3963英里(6377.830千米)。
        /// </summary>
        public const double EarthCHIDAORadius = 6377.830;

        /// <summary>
        /// 平均半径
        /// 大约3959英里(6371.393千米) 。这个数字是地心到地球表面所有各点距离的平均值
        /// </summary>
        public const double EarthRERadius = 6371.393;

        /// <summary>
        /// 地球半径,千米
        /// 默认
        /// </summary>
        public const double EarthRadius = 6378.137;

        /// <summary>
        /// 地球周长
        /// </summary>
        public const double EarthCircumference = EarthRadius * 2 * Math.PI;


        private const double COS38 = 0.95507364404729;

        /// <summary>
        /// 地球38度周长
        /// </summary>
        public const double Earth38Circumference = EarthRadius * 2 * Math.PI * COS38;

        /// <summary>
        /// 经度上一米的度数
        /// 北纬38度上
        /// </summary>
        public const double Earth38LongOneMeter = 360 / Earth38Circumference;

        /// <summary>
        /// 纬度上一米的度数
        /// </summary>
        public const double EarthLatOneMeter = 360 / EarthCircumference;

        // <summary>
        /// 经纬度转化成弧度
                /// </summary>
                /// <param name="d"></param>
                /// <returns></returns>
        public static double ToRadian(double angle)
        {
            return angle * Math.PI / 180d;
        }

        /// <summary>
        /// 计算两个坐标点之间的距离单位：公里/千米
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>单位：公里/千米</returns>
        public override double DistanceToPoint(DPoint from, DPoint to)
        {
            //double firstRadLat = ToRadian(from.y);
            //double firstRadLng = ToRadian(from.x);
            //double secondRadLat = ToRadian(to.y);
            //double secondRadLng = ToRadian(to.x);


            //var a = firstRadLat - secondRadLat;
            //var b = firstRadLng - secondRadLng;
            //var cal = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(firstRadLat) * Math.Cos(secondRadLat) * Math.Pow(Math.Sin(b / 2), 2))) * EarthRadius;
            //var result = Math.Round(cal * 10000) / 10000;
            //return result;



            double radLat1 = ToRadian(from.y);
            double radLat2 = ToRadian(to.y);

            double radLon1 = ToRadian(from.x);
            double radLon2 = ToRadian(to.x);

            if (radLat1 < 0)
                radLat1 = Math.PI / 2 + Math.Abs(radLat1);// south  
            if (radLat1 > 0)
                radLat1 = Math.PI / 2 - Math.Abs(radLat1);// north  
            if (radLon1 < 0)
                radLon1 = Math.PI * 2 - Math.Abs(radLon1);// west  
            if (radLat2 < 0)
                radLat2 = Math.PI / 2 + Math.Abs(radLat2);// south  
            if (radLat2 > 0)
                radLat2 = Math.PI / 2 - Math.Abs(radLat2);// north  
            if (radLon2 < 0)
                radLon2 = Math.PI * 2 - Math.Abs(radLon2);// west  
            double x1 = EarthRadius * Math.Cos(radLon1) * Math.Sin(radLat1);
            double y1 = EarthRadius * Math.Sin(radLon1) * Math.Sin(radLat1);
            double z1 = EarthRadius * Math.Cos(radLat1);

            double x2 = EarthRadius * Math.Cos(radLon2) * Math.Sin(radLat2);
            double y2 = EarthRadius * Math.Sin(radLon2) * Math.Sin(radLat2);
            double z2 = EarthRadius * Math.Cos(radLat2);

            double d = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2));
            //余弦定理求夹角  
            double theta = Math.Acos((EarthRadius * EarthRadius + EarthRadius * EarthRadius - d * d) / (2 * EarthRadius * EarthRadius));
            double dist = theta * EarthRadius;
            return dist;
        }





    }
}
