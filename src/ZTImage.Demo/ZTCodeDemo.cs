using System;
using System.Collections.Generic;
using ZTImage.Reflection.Reflector;
using System.Linq;
using ZTImage.DbLite;
using System.Data;
using ZTImage;

namespace Demo
{
    public class ZTCodeDemo : DemoUnit
    {
        public override void Do()
        {
            UInt64 input = 113150;
            string no = input.ToUnorderZTCode();
            Console.WriteLine($"no is :{no}");



            no = (input + 1).ToUnorderZTCode();

            Console.WriteLine($"no is :{no}");

            no = (input + 2).ToUnorderZTCode();

            Console.WriteLine($"no is :{no}");


            no = (input + 3).ToUnorderZTCode();

            Console.WriteLine($"no is :{no}");

            UInt64 target = no.UnorderZTCodeToUInt64();
            Console.WriteLine($"target is:{target}");
        }
    }
}
