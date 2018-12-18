using System;
using System.Collections.Generic;

namespace ZTImage.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = TypeConverter.LongToDate(1);
            Console.WriteLine(dt);
            
            
            //string content = System.Text.Encoding.UTF8.GetString(b);
            //Console.WriteLine(content);

            new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
