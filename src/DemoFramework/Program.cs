using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFramework
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

            Console.WriteLine(ZTImage.TypeConverter.ObjectToInt(obj, 0));
            Console.WriteLine(ZTImage.ZTID.Instance.NextId());

            Console.ReadKey();
        }
    }
}
