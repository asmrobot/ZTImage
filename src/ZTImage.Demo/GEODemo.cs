using System;
using System.Collections.Generic;
using System.Text;
using ZTImage;

namespace Demo
{
    public class GEODemo : DemoUnit
    {
        public override void Do()
        {


            ZTShape geo = new ZTShape();
            Console.WriteLine(geo.DistanceToLine(new DPoint(3, 1), new DPoint(0, 2), new DPoint(2, 2)));


            List<DPoint> points = new List<DPoint>() {
                new DPoint(1,3),
                new DPoint(2,2),
                new DPoint(3,1),
                new DPoint(4,2),
                new DPoint(3,3),
                new DPoint(5,3),
                new DPoint(5,4),
                new DPoint(3,6),
                new DPoint(2,5),
                new DPoint(2,4),
            };


            DPoint point = geo.CalcCenterOfGravityPoint(points);
            Console.WriteLine("the center:" + point);




            points = new List<DPoint>() {
                new DPoint (1,1),
                new DPoint (3,1),
                new DPoint (3,3),
                new DPoint (1,3)
            };

            point = geo.CalcCenterOfGravityPoint(points);
            Console.WriteLine("the center:" + point);


            points = new List<DPoint>() {
                new DPoint (1,3),
                new DPoint (2,2),
                new DPoint (3,3),
                new DPoint (2,4)
            };

            point = geo.CalcCenterOfGravityPoint(points);
            Console.WriteLine("the center:" + point);

            //DPoint point = new DPoint(3, 5);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(1, 5);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));



            //point = new DPoint(3, 4);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));



            //point = new DPoint(1, 1);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));



            //point = new DPoint(4, 4);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(3, 0.5d);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(2, 3);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(3.5d, 2.5d);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));



            //point = new DPoint(1, 3);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));

            //point = new DPoint(2, 2);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(3, 1);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(4, 2);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(3, 3);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(5, 3);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(5, 4);
            //Console.WriteLine("point:" + point.ToString() + "$in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


            //point = new DPoint(3, 6);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));



            //point = new DPoint(2, 5);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));


        }
    }
}
