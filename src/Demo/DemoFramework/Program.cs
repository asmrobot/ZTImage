using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage;
using ZTImage.WeChat;
using ZTImage.WeChat.Messages;
using ZTImage.WeChat.ReplyMessages;

namespace DemoFramework
{
    class Program
    {

        public enum EventType : int
        {
            unknow = 0,//未知
            subscribe = 1,//订阅
            unsubscribe = 2,//取消订阅
            scan = 3,//扫描带参数二维码
            location = 4,//上报地理位置
            click = 5,//点击自定义菜单
            view = 6,//点击菜单跳转链接
        }

        static void Main(string[] args)
        {
            ZTImage.Log.Trace.EnableListener(ZTImage.Log.NLog.Instance);
            //WeChatManager Manager = new WeChatManager("NkRWYqr3KUR1UfO7", "wx56bb5bd00d687d3d", "02ac6f9db3166a7fd2e0098d1fe4f2ee");

            //SugarTemplateNotificationMessage msg = new SugarTemplateNotificationMessage("12", "98", 178);
            //if (Manager.SendTemplateMessage("oVn8nvzApTSC_K-SNnm3dbxU0-ls", string.Empty, msg))
            //{
            //    Console.WriteLine("ok");
            //}
            //else
            //{
            //    Console.WriteLine("failed");
            //}


            EventType t = EventType.unknow;
            if (Enum.TryParse<EventType>("scan",true, out t))
            {
                Console.WriteLine(t);
            }
            else
            {
                Console.WriteLine("no");
            }
            




            Console.ReadKey();
        }
    }
}
