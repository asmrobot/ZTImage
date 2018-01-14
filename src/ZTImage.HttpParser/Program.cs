using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO.Compression;
using System.Threading;

namespace ZTImage.HttpParser
{

    class Program
    {

        private static void ReceiveCallback(IAsyncResult result)
        {
            int count=socket.EndReceive(result);
            if (count <= 0)
            {
                Console.WriteLine("ok");
                return;
            }


            int len = engine.Execute(mFrame, new ArraySegment<byte>(datas,0,count));
            
            

            Console.WriteLine("complete,return:{2},total:{0},read:{1},content-length:{3}", len, count, mFrame.http_errno, mFrame.content_length);

            socket.BeginReceive(datas, 0, datas.Length, SocketFlags.None, ReceiveCallback, null);
        }

        private static ZTResponse mFrame;
        private static Socket socket;
        private static byte[] datas = new byte[1024000];
        private static ParserEngine engine = new ParserEngine(new ZTParserCallback<ZTResponse>());
        static unsafe  void Main(string[] args)
        {
            mFrame = new ZTResponse();

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new DnsEndPoint("www.cnblogs.com", 80));

            socket.BeginReceive(datas, 0, datas.Length, SocketFlags.None, ReceiveCallback, null);
            string HttpContent = @"GET http://www.cnblogs.com/xing901022/p/8260362.html HTTP/1.1
Host: www.cnblogs.com
Connection: keep-alive
Cache-Control: max-age=0
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36
Upgrade-Insecure-Requests: 1
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Accept-Encoding: gzip, deflate
Accept-Language: zh-CN,zh;q=0.8
Cookie: pgv_pvi=7221736448; __gads=ID=ae773916ffa37073:T=1500729087:S=ALNI_Mbsqu692v_kvy9ln25GdvQlkTH2iA; UM_distinctid=15d6d1d0f004b-09a9beecd1bdcc-5393662-1fa400-15d6d1d0f01413; CNZZDATA1260386081=1253174744-1500774304-https%253A%252F%252Fwww.cnblogs.com%252F%7C1500774304; CNZZDATA1000228000=1465240195-1501159745-null%7C1501159745; CNZZDATA1261256959=2020531323-1503209328-null%7C1503209328; _pk_id.4.1edd=f457f856751b67b3.1503800628.3.1504180258.1504180258.; CNZZDATA1257151657=1408806613-1504396861-null%7C1504396861; CNZZDATA1259029673=769241473-1504925483-null%7C1504925483; CNZZDATA1254128672=1866741790-1505285260-null%7C1505285260; sc_is_visitor_unique=rx11247317.1505736177.C5704C7A44094FC476A904249117F42F.2.2.2.2.2.2.2.2.2; CNZZDATA1262435696=1103225973-1504185963-https%253A%252F%252Fwww.cnblogs.com%252F%7C1506348086; CNZZDATA1261058399=54743942-1506853614-null%7C1506853614; __utmz=226521935.1509448046.5.4.utmcsr=zzk.cnblogs.com|utmccn=(referral)|utmcmd=referral|utmcct=/s; CNZZDATA1264405927=1458231262-1508151623-null%7C1510668699; CNZZDATA3685059=cnzz_eid%3D1570514254-1511181180-%26ntime%3D1511186620; AJSTAT_ok_times=8; CNZZDATA1259286380=834457718-1503917865-null%7C1512948783; CNZZDATA2364173=cnzz_eid%3D88710487-1502547872-http%253A%252F%252Fwww.cnblogs.com%252F%26ntime%3D1513937829; __utma=226521935.1075941213.1500707916.1509448046.1514093578.6; CNZZDATA1000228226=564337070-1514616297-https%253A%252F%252Fwww.cnblogs.com%252F%7C1514616297; CNZZDATA1271555009=554802623-1514890362-https%253A%252F%252Fwww.cnblogs.com%252F%7C1514890362; CNZZDATA1260206164=498119767-1515108018-https%253A%252F%252Fwww.cnblogs.com%252F%7C1515108018; CNZZDATA1000342940=172427378-1515238053-https%253A%252F%252Fwww.baidu.com%252F%7C1515238053; _gat=1; _ga=GA1.2.564075727.1500687415; _gid=GA1.2.1329213995.1515502250
If-Modified-Since: Wed, 10 Jan 2018 12:03:41 GMT

";
            byte[] HttpData = System.Text.Encoding.UTF8.GetBytes(HttpContent);

            for (int i = 0; i < 1; i++)
            {
                socket.Send(HttpData);
            }


            Thread.Sleep(3000);

            
                Console.WriteLine(mFrame.GetContent());
            



            //            string request = @"POST http://admin.xzhealth.cn/Frame/Login HTTP/1.1
            //Host: admin.xzhealth.cn
            //Connection: close
            //Content-Length: 46
            //Cache-Control: max-age=0
            //Origin: http://admin.xzhealth.cn
            //Upgrade: website
            //Upgrade-Insecure-Requests: 1
            //Content-Type: application/x-www-form-urlencoded
            //User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.108 Safari/537.36
            //Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
            //Referer: http://admin.xzhealth.cn/
            //Accept-Encoding: gzip, deflate
            //Accept-Language: zh-CN,zh;q=0.9

            //ispostback=1&username=admin&password=283965069";
            //            byte[] RequestData = Encoding.UTF8.GetBytes(request);

            //            string response = "HTTP/1.1 200 OK\r\nDate: Sun, 07 Jan 2018 01:57:21 GMT\r\nContent-Type: application/json;charset=utf8\r\nContent-Length: 3\r\nConnection: keep-alive\r\nService-Host: youku-danmu-service011139056192.na61\r\nServer: Tengine/Aserver\r\nTiming-Allow-Origin: *\r\n\r\nfdsafdsafdsafdsafdsafdsafdsa=fdsafdsafdsafdsajfkdls;ajfkdls;ajklf;dsjakljfdsl";
            //            byte[] ResponseDatas = Encoding.UTF8.GetBytes(response);


            //            HttpFrame frame = new HttpFrame();

            //            Int32 len = ResponseDatas.Length;
            //            Int32 count = 0;
            //            //Stopwatch watch = new Stopwatch();
            //            //watch.Start();
            //            //for (int i = 0; i < 1000000; i++)
            //            //{
            //                count = engine.Execute(frame, ResponseDatas);
            //            //    frame.Reset();
            //            //}
            //            //watch.Stop();
            //            //Console.WriteLine("time:" + watch.ElapsedMilliseconds);


            //            Console.WriteLine("complete,return:{2},total:{0},read:{1},content-length:{3}", len, count, frame.http_errno,frame.content_length);
            Console.ReadKey();

        }
    }
}
