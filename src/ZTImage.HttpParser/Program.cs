using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{

    class Program
    {
        static unsafe  void Main(string[] args)
        {
            ParserEngine engine = new ParserEngine();
            HttpFrame frame = new HttpFrame();
            

            string request = @"POST http://111.206.62.179/cloudquery.php HTTP/1.1
User-Agent: Post_Multipart
Host: 111.206.62.179
Accept: */*
Connection: Keep-Alive
Pragma: no-cache
X-360-Cloud-Security-Desc: Scan Suspicious File
x-360-ver: 4
Content-Length: 7
Content-Type: multipart/form-data; boundary=----------------------------eca696d85637

------------------------------eca696d85637
Content-Disposition: form-data; name=""m""


------------------------------eca696d85637--
";

            string response = @"HTTP/1.1 200 OK
Date: Sun, 07 Jan 2018 01:57:21 GMT
Content-Type: application/json;charset=utf8
Content-Length: 3
Connection: keep-alive
Service-Host: youku-danmu-service011139056192.na61
Server: Tengine/Aserver
Timing-Allow-Origin: *

fdsafdsafdsafdsafdsafdsafdsa=fdsafdsafdsafdsajfkdls;ajfkdls;ajklf;dsjakljfdsl
";

            byte[] ResponseDatas = System.Text.Encoding.UTF8.GetBytes(response);


            byte[] RequestData = System.Text.Encoding.UTF8.GetBytes(request);
            Int32 len = ResponseDatas.Length;
            Int32 count = 0;
            count = engine.Execute(frame, ResponseDatas);
            
            Console.WriteLine("complete,return:{2},total:{0},read:{1},content-length:{3}", len, count, frame.http_errno,frame.content_length);
            Console.ReadKey();

        }
    }
}
