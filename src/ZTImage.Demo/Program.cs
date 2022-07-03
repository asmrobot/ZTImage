using System;
using System.Collections.Generic;
using ZTImage.Reflection.Reflector;
using System.Linq;
using ZTImage.DbLite;
using System.Data;

namespace ZTImage.Demo
{
    class Program
    {

        [AttributeUsage(AttributeTargets.Property)]
        public class KeyAttribute : Attribute
        {
            public KeyAttribute()
            {

            }
        }

        public class person
        {
            public decimal age { get; set; }

            [Key]
            public string name { get; set; }

            public byte Sex { get; set; }

            public int MyProperty { get; set; }

            public int MyProperty1 { get; set; }

            public int MyProperty2 { get; set; }

            public int MyProperty3 { get; set; }





            public int aaaaaaa1 { get; set; }
            public int aaaaaaa2 { get; set; }
            public int aaaaaaa3 { get; set; }
            public int aaaaaaa4   {get;set;}
            public int aaaaaaa15  {get;set;}
            public int aaaaaaa16  {get;set;}
            public int aaaaaaa17  {get;set;}
            public int aaaaaaa18  {get;set;}
            public int aaaaaaa19  {get;set;}
            public int aaaaaaa111 {get;set;}
            public int aaaaaaa121 {get;set;}
            public int aaaaaaa122 {get;set;}
            public int aaaaaaa123 {get;set;}
            public int aaaaaaa124 {get;set;}
            public int aaaaaaa125 {get;set;}
            public int aaaaaaa126 {get;set;}
            public int aaaaaaa127 {get;set;}
            public int aaaaaaa128 {get;set;}
            public int aaaaaaa129 { get; set; }

        }

        public class ani
        { 
            public Int32 Age { get; set; }

            public string Name { get; set; }

            public bool sex { get; set; }

            public override string ToString()
            {
                return $"age:{Age},name:{Name},sex:{sex}";
            }
        }

        public struct hii
        {
            public Int32 age;
        }

        static void Main(string[] args)
        {
            UInt64 input = 113150;
            string no = input.ToUnorderZTCode();
            Console.WriteLine($"no is :{no}");


            
            no = (input+1).ToUnorderZTCode();

            Console.WriteLine($"no is :{no}");

            no = (input + 2).ToUnorderZTCode();

            Console.WriteLine($"no is :{no}");


            no = (input + 3).ToUnorderZTCode();

            Console.WriteLine($"no is :{no}");

            UInt64 target = no.UnorderZTCodeToUInt64();
            Console.WriteLine($"target is:{target}"); 


            //DbConnectionFactoryBuilder builder = new DbConnectionFactoryBuilder();
            //var factory=builder.AddDbConnectionOption(null)
            //    .AddDbConnectionOption(null)
            //    .AddDbConnectionOption(null)
            //    .Build();

           

            //string name = "person_name_age";

            //Console.WriteLine(name.Replace("_", ""));



            new System.Threading.ManualResetEvent(false).WaitOne();





            //person p = new person();
            //p.age = 12;
            //p.name = "xylee";
            //p.Sex = 1;

            //ani a = new ani();

            //ZTImage.Reflection.AutomiticVariable.CopyValue(p, a);

            //Console.WriteLine(a);



            //double age = 12.3;

            //object toAge=Convert.ChangeType(age, typeof(Decimal));

            //Console.WriteLine($"value:{toAge},value type:{toAge.GetType().Name}");

            //ZTReflector refl = ZTReflector.Cache(typeof(person), false);
            //object p = refl.NewObject();
            //if (refl.Properties["age"].TrySetValue(p, age))
            //{
            //    Console.WriteLine($"set success,{((person)p).age}");
            //}
            //else
            //{
            //    Console.WriteLine("set failed");
            //}











            //ZTShape geo = new ZTShape();
            //Console.WriteLine(geo.DistanceToLine(new DPoint(3, 1), new DPoint(0, 2), new DPoint(2, 2)));


            //List<DPoint> points = new List<DPoint>() {
            //    new DPoint(1,3),
            //    new DPoint(2,2),
            //    new DPoint(3,1),
            //    new DPoint(4,2),
            //    new DPoint(3,3),
            //    new DPoint(5,3),
            //    new DPoint(5,4),
            //    new DPoint(3,6),
            //    new DPoint(2,5),
            //    new DPoint(2,4),
            //};


            //DPoint point=geo.CalcCenterOfGravityPoint(points);
            //Console.WriteLine("the center:" + point);




            //points = new List<DPoint>() {
            //    new DPoint (1,1),
            //    new DPoint (3,1),
            //    new DPoint (3,3),
            //    new DPoint (1,3)
            //};

            //point = geo.CalcCenterOfGravityPoint(points);
            //Console.WriteLine("the center:" + point);


            //points = new List<DPoint>() {
            //    new DPoint (1,3),
            //    new DPoint (2,2),
            //    new DPoint (3,3),
            //    new DPoint (2,4)
            //};

            //point = geo.CalcCenterOfGravityPoint(points);
            //Console.WriteLine("the center:" + point);

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
            //Console.WriteLine("point:" + point.ToString() + "$in polygon:" + geo.InPolygon(point, points)+"$distance:"+geo.DistanceToPolygon(point,points));


            //point = new DPoint(3, 6);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));



            //point = new DPoint(2, 5);
            //Console.WriteLine("point:" + point.ToString() + "in polygon:" + geo.InPolygon(point, points) + "$distance:" + geo.DistanceToPolygon(point, points));





                new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
