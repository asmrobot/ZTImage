using System;
using System.Collections.Generic;

namespace ZTImage.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            byte b = 0x05;
            Console.WriteLine(b.ToString("x2"));


            new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
