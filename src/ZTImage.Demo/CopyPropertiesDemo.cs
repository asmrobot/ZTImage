﻿using System;
using System.Collections.Generic;
using System.Text;
using ZTImage.Reflection.Reflector;

namespace Demo
{
    public class CopyPropertiesDemo : DemoUnit
    {
        public override void Do()
        {

            //person p = new person();
            //p.age = 12;
            //p.name = "xylee";
            //p.Sex = 1;

            //ani a = new ani();
            
            //ZTImage.Reflection.AutomiticVariable.CopyValue(p, a);

            //Console.WriteLine(a);



            double age = 12.3;

            //object toAge = Convert.ChangeType(age, typeof(Decimal));

            //Console.WriteLine($"value:{toAge},value type:{toAge.GetType().Name}");

            ZTReflector refl = ZTReflector.Cache(typeof(person), false);
            object p = refl.NewObject();
            if (refl.Properties["age"].TrySetValue(p, age))
            {
                Console.WriteLine($"set success,{((person)p).age}");
            }
            else
            {
                Console.WriteLine("set failed");
            }
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
        public int aaaaaaa4 { get; set; }
        public int aaaaaaa15 { get; set; }
        public int aaaaaaa16 { get; set; }
        public int aaaaaaa17 { get; set; }
        public int aaaaaaa18 { get; set; }
        public int aaaaaaa19 { get; set; }
        public int aaaaaaa111 { get; set; }
        public int aaaaaaa121 { get; set; }
        public int aaaaaaa122 { get; set; }
        public int aaaaaaa123 { get; set; }
        public int aaaaaaa124 { get; set; }
        public int aaaaaaa125 { get; set; }
        public int aaaaaaa126 { get; set; }
        public int aaaaaaa127 { get; set; }
        public int aaaaaaa128 { get; set; }
        public int aaaaaaa129 { get; set; }

    }




    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute()
        {

        }
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
}
