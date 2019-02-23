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
    }
}
