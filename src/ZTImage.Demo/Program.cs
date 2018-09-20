using System;
using System.Collections.Generic;

namespace ZTImage.Demo
{



    public class InoneUploadModel
    {
        public string MachineNo { get; set; }

        public List<ElderlyAppraiseModel> Datas { get; set; }
    }
    public class ElderlyAppraiseModel
    {
        public string ID { get; set; }
        public string IDNumber { get; set; }
        public float Score { get; set; }
        public string Doctor { get; set; }
        public Int64 AppraiseTime { get; set; }
        public List<appraiseitem> AppraiseItems { get; set; }
    }
    public class appraiseitem
    {
        public string AppraiseItem { get; set; }
        public string Content { get; set; }
        public Int32 SelectItem { get; set; }
        public float Points { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string json = "{\n  " +
                "  \"Datas\": [\n     " +
                "   {\n  " +
                "         \"AppraiseTime\": \"1536768000\"\n,\"ss\":     \"fdsfds\"  " +

                " }\n   " +
                " ]\n   " +
                "}\n";
            InoneUploadModel p = ZTImage.Json.JsonParser.ToObject<InoneUploadModel>(json);
            Console.WriteLine(p.Datas.Count);


            new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
