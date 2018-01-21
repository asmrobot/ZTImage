using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Log;

namespace DemoFramework
{
    public class App_Upload_sec_user
    {
        public Guid userid { get; set; }

        public Int64 birthday { get; set; }
        public string nick { get; set; }
        public string idnumber { get; set; }
        public string sex { get; set; }
        public decimal height { get; set; }
        public decimal weight { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }

        public int departmantid { get; set; }
        public string doctorintroduct { get; set; }

        public string headportrait { get; set; }



        //public string oldpsw { get; set; }
        //public string newpsw { get; set; }


        //public string picname { get; set; }
        //public string image { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string json = "{\"phonenumber\":\"\"}";

            var model = ZTImage.Json.JsonParser.ToObject<App_Upload_sec_user>(json);

            Console.WriteLine(model.phonenumber);


            Console.ReadKey();
        }
    }
}
