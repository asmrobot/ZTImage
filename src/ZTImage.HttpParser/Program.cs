using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{

    class Program
    {
        private static ParserEngine engine = new ParserEngine(new DefaultParserCallback());
        static unsafe  void Main(string[] args)
        {
            string request = @"POST http://admin.xzhealth.cn/Frame/Login HTTP/1.1
Host: admin.xzhealth.cn
Connection: close
Content-Length: 46
Cache-Control: max-age=0
Origin: http://admin.xzhealth.cn
Upgrade: website
Upgrade-Insecure-Requests: 1
Content-Type: application/x-www-form-urlencoded
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.108 Safari/537.36
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Referer: http://admin.xzhealth.cn/
Accept-Encoding: gzip, deflate
Accept-Language: zh-CN,zh;q=0.9

ispostback=1&username=admin&password=283965069";
            byte[] RequestData = Encoding.UTF8.GetBytes(request);

            string response = "HTTP/1.1 200 OK\r\nDate: Sun, 07 Jan 2018 01:57:21 GMT\r\nContent-Type: application/json;charset=utf8\r\nContent-Length: 3\r\nConnection: keep-alive\r\nService-Host: youku-danmu-service011139056192.na61\r\nServer: Tengine/Aserver\r\nTiming-Allow-Origin: *\r\n\r\nfdsafdsafdsafdsafdsafdsafdsa=fdsafdsafdsafdsajfkdls;ajfkdls;ajklf;dsjakljfdsl";
            byte[] ResponseDatas = Encoding.UTF8.GetBytes(response);


            HttpFrame frame = new HttpFrame();
            
            Int32 len = ResponseDatas.Length;
            Int32 count = 0;
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //for (int i = 0; i < 1000000; i++)
            //{
                count = engine.Execute(frame, ResponseDatas);
            //    frame.Reset();
            //}
            //watch.Stop();
            //Console.WriteLine("time:" + watch.ElapsedMilliseconds);
            
            
            Console.WriteLine("complete,return:{2},total:{0},read:{1},content-length:{3}", len, count, frame.http_errno,frame.content_length);
            Console.ReadKey();

        }
    }
}
