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
            

            string request = @"GET /profile?jsoncallback=jQuery1112040917624099260497_1515290208116&ct=1001&iid=744592160&aid=300578&cid=96&lid=0&ouid=0&_=1515290208117 HTTP/1.1
Host: service.danmu.youku.com
Connection: keep-alive
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36
Accept: */*
Referer: http://v.youku.com/v_show/id_XMjk3ODM2ODY0MA==.html?spm=a2h0j.8191423.vpofficiallistv5_wrap.5~5~5~5!19~5~5~A
Accept-Encoding: gzip, deflate
Accept-Language: zh-CN,zh;q=0.8
Cookie: juid=01blkjgbr22eeu; __ysuid=1500708973168Q5m; __aryft=1503920419; __artft=1503920419; __yscnt=1; yseid=15152868118346wDnRy; yseidcount=196; seid=01c3723l5t10j2; P_ck_ctl=DCB293C466F7B8891CF00B6D9A85E862; referhost=http%3A%2F%2Fwww.youku.com; _m_h5_tk=e6e22658baa5ea2a9dcd4496bdc32d98_1515291584756; _m_h5_tk_enc=01d270674502530c967b2bf2b693ff88; isg=AldXeturwf-L_0ZRmbrtAVyt5sthNCj1yXaDTKmFOSal2HYasW8FTBQ4Tk-8; ypvid=1515290208173Ok1TJC; ysestep=5; yseidtimeout=1515297408174; ycid=0; ystep=2160; seidtimeout=1515292008178; cna=ckT2EQoKCGkCAS1MM8O6lqgp; __ayft=1515286811270; __aysid=15145074670677Ot; __arpvid=151529020833032W53y-1515290208336; __arycid=dc-3-235-300578-744592160; __ayscnt=1; __arcms=dc-3-235-300578-744592160; __aypstp=5; __ayspstp=127; __ayvstp=66; __aysvstp=1727
";

            string response = @"HTTP/1.1 200 OK
Date: Sun, 07 Jan 2018 01:57:21 GMT
Content-Type: application/json;charset=utf8
Content-Length: 362
Connection: keep-alive
Service-Host: youku-danmu-service011139056192.na61
Server: Tengine/Aserver
Timing-Allow-Origin: *

fdsafdsafdsafdsafdsafdsafdsa=fdsafdsafdsa";

            byte[] ResponseDatas = System.Text.Encoding.UTF8.GetBytes(response);

            byte[] RequestData = System.Text.Encoding.UTF8.GetBytes(request);

            fixed (byte* data = &ResponseDatas[0])
            {
                Int32 count = engine.Execute(frame,data, ResponseDatas.Length);
            }
            

            Console.WriteLine("complete");
            Console.ReadKey();

        }
    }
}
