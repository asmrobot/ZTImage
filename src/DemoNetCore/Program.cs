using System;

namespace DemoNetCore
{
    class Program
    {
        static void Main(string[] args)
        {

            ZTImage.Database.HelperBase.SQLDBHelper helper = null;
            if (helper != null)
            {
                helper.ExecuteNonQuery("ok");
            }


            object obj = 1;
            Console.WriteLine("ok");
            Console.WriteLine(ZTImage.TypeConverter.ObjectToInt(obj, 0));
            Console.WriteLine(ZTImage.ZTID.Instance.NextId());
            Console.ReadKey();
        }
    }
}
