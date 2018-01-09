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
        private static ParserEngine engine = new ParserEngine();
        static unsafe  void Main(string[] args)
        {
            string request = "POST http://111.206.62.179/cloudquery.php HTTP/1.1\r\nUser-Agent: Post_Multipart\r\nHost: 111.206.62.179\r\nAccept: */*\r\nConnection: Keep-Alive\r\nPragma: no-cache\r\nX-360-Cloud-Security-Desc: Scan Suspicious File\r\nx-360-ver: 4\r\nContent-Length: 7\r\nContent-Type: multipart/form-data; boundary=----------------------------eca696d85637\r\n\r\nababcfdsafdsafdsa";
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
