using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public class ZTShape
    {
        /// <summary>
        /// 占位于图片中的位置区域
        /// </summary>
        public enum LocationArea : byte
        {
            /// <summary>
            /// 内部
            /// </summary>
            Inside = 0,

            /// <summary>
            /// 外部
            /// </summary>
            Outside = 1,//

            /// <summary>
            /// 重合在边上
            /// </summary>
            Line = 2
        }




        /// <summary>
        /// 计算两个坐标点之间的距离单位：公里/千米
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>单位：公里/千米</returns>
        public virtual double DistanceToPoint(DPoint from, DPoint to)
        {
            return Math.Sqrt(Math.Pow(from.x - to.x, 2) + Math.Pow(from.y - to.y, 2));
        }

        /// <summary>
        /// 计算点到线段的最短距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public double DistanceToLine(DPoint point, DPoint lineStart, DPoint lineEnd)
        {
            //多边形中当前点
            double a = DistanceToPoint(lineStart, lineEnd);
            double b = DistanceToPoint(lineEnd, point);
            double c = DistanceToPoint(lineStart, point);
            if (b * b >= c * c + a * a)
            {
                return c;
            }
            if (c * c >= b * b + a * a)
            {
                return b;
            }

            double l = (a + b + c) / 2;     //周长的一半
            double s = Math.Sqrt(l * (l - a) * (l - b) * (l - c));  //海伦公式求面积 
            return 2 * s / a;
        }


        /// <summary>
        /// 计算点到多边形的最短距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public double DistanceToPolygon(DPoint point, List<DPoint> points)
        {
            double minDistance = double.MaxValue;
            if (points.Count < 2)
            {
                throw new ArgumentException("parameter 太少");
            }
            double distance = 0;
            for (int i = 0; i < points.Count; i++)
            {
                //多边形中当前点
                var currentPoint = points[i];
                var nextPoint = (points.Count - 1) == i ? points[0] : points[i + 1];

                distance = DistanceToLine(point, currentPoint, nextPoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            return minDistance;
        }


        /// <summary>
        /// 判断一个点是否在圆形区域内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="centreOfCircle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public LocationArea InCircle(DPoint point, DPoint centreOfCircle, double radius)
        {
            double distance = DistanceToPoint(point, centreOfCircle);
            if (distance > radius)
            {
                return LocationArea.Outside;
            }
            else if (distance == radius)
            {
                return LocationArea.Line;
            }
            else
            {
                return LocationArea.Inside;
            }
        }

        /// <summary> 
        /// 判断点是否在多边形内. 
        /// ----------原理---------- 
        /// 注意到如果从P作水平向左的射线的话，如果P在多边形内部，那么这条射线与多边形的交点必为奇数， 
        /// 如果P在多边形外部，则交点个数必为偶数(0也在内)。 
        /// </summary> 
        /// <param name="point">要判断的点</param> 
        /// <returns></returns> 
        public LocationArea InPolygon(DPoint point, List<DPoint> points)
        {
            bool inside = false;
            int pointCount = points.Count;
            DPoint p1, p2;
            for (int i = 0, j = pointCount - 1; i < pointCount; j = i, i++)
            {
                //第一个点和最后一个点作为第一条线，之后是第一个点和第二个点作为第二条线，之后是第二个点与第三个点，第三个点与第四个点... 
                p1 = points[i];
                p2 = points[j];
                if (point.x == p2.x && point.y == p2.y)
                {
                    return LocationArea.Line;
                }

                if (point.y < p2.y)
                {
                    //p2在射线之上 
                    if (p1.y <= point.y)
                    {
                        //p1正好在射线中或者射线下方 
                        if ((point.y - p1.y) * (p2.x - p1.x) > (point.x - p1.x) * (p2.y - p1.y))
                        {
                            //斜率判断,在P1和P2之间且在P1P2右侧 
                            //射线与多边形交点为奇数时则在多边形之内，若为偶数个交点时则在多边形之外。 
                            //由于inside初始值为false，即交点数为零。所以当有第一个交点时，则必为奇数，则在内部，此时为inside=(!inside) 
                            //所以当有第二个交点时，则必为偶数，则在外部，此时为inside=(!inside) 
                            inside = (!inside);
                        }
                    }
                }
                else if (point.y < p1.y)
                {
                    //p2正好在射线中或者在射线下方，p1在射线上 
                    if ((point.y - p1.y) * (p2.x - p1.x) < (point.x - p1.x) * (p2.y - p1.y))
                    {
                        //斜率判断,在P1和P2之间且在P1P2右侧 
                        inside = (!inside);
                    }
                }
            }

            if (inside)
            {
                return LocationArea.Inside;
            }
            else
            {
                return LocationArea.Outside;
            }
        }

        ///// <summary>
        ///// 计算中心点
        ///// </summary>
        ///// <param name="points"></param>
        ///// <returns></returns>
        //public DPoint CalcCenterPoint(List<DPoint> points)
        //{
        //    List<double> areas = new List<double>();//面积
        //    double sall = 0;
        //    DPoint p = new DPoint(0, 0);
        //    for (int i = 1; i < points.Count - 1; i++)
        //    {
        //        //每个三角形由v[0],v[i],v[i+1]三个顶点组成
        //        //面积
        //        areas[i - 1] = (points[i].x - points[0].x) * (points[i + 1].y - points[0].y) - (points[i].y - points[0].y) * (points[i + 1].x - points[0].x);
        //        sall += areas[i - 1];
        //        //重心
        //        p.x += areas[i - 1] * (points[i].x + points[i + 1].x + points[0].x) * 1.0 / 3;
        //        p.y += areas[i - 1] * (points[i].y + points[i + 1].y + points[0].y) * 1.0 / 3;
        //    }
        //    p.x /= sall * 1.0;
        //    p.y /= sall * 1.0;
        //    return p;
        //}

        /// <summary>
        /// 计算重心点
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public DPoint CalcCenterOfGravityPoint(List<DPoint> points)
        {
            double area = 0.0f;//多边形面积  
            double Gx = 0.0f, Gy = 0.0f;// 重心的x、y  
            for (int i = 1; i <= points.Count; i++)
            {
                double iLat = points[(i % points.Count())].x;
                double iLng = points[(i % points.Count())].y;
                double preLat = points[(i - 1)].x;
                double preLng = points[(i - 1)].y;
                double temp = (iLat * preLng - iLng * preLat) / 2.0f;
                area += temp;
                Gx += temp * (iLat + preLat) / 3.0f;
                Gy += temp * (iLng + preLng) / 3.0f;
            }
            Gx = Gx / area;
            Gy = Gy / area;
            return new DPoint(Gx, Gy);
        }
    }
}
