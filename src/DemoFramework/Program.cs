using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Log;

namespace DemoFramework
{
    public class InoneagentRequestModel
    {
        ///<summary>
        ///创建时间开始日期，非必要可为空
        /// </summary>
        public Int64 createbegin { get; set; }
        ///<summary>
        ///创建时间结束日期，非必要可为空
        /// </summary>
        public Int64? createend { get; set; }
        ///<summary>
        ///页码，非必要可为空（为空时默认1）
        /// </summary>
        public int pageindex { get; set; }
        ///<summary>
        ///页大小，非必要可为空(为空时默认20)(20-500)
        /// </summary>
        public int pagesize { get; set; }
        /// <summary>
        /// 提交请求的时间，为linux时间戳，UTC时间，必要
        /// </summary>
        public long ts { get; set; }
        /// <summary>
        /// 接口提供方提供的AP值，必要
        /// </summary>
        public string ap { get; set; }
        /// <summary>
        /// 加密值（加密算法见最后），必要
        /// </summary>
        public string sig { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string json = "{\"ap\":\"09de6c06-d81e-4027-a176-e379c0e5b66b\",\"createbegin\":\"1515046734\"}";
            var model = ZTImage.Json.JsonParser.ToObject<InoneagentRequestModel>(json);

            Console.WriteLine(model.ap);

            Console.ReadKey();
        }
    }
}
