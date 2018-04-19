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
        static void Main(string[] args)
        {
            ZTImage.Log.Trace.EnableListener(ZTImage.Log.NLog.Instance);
            WeChatManager Manager = new WeChatManager("NkRWYqr3KUR1UfO7", "wx56bb5bd00d687d3d", "02ac6f9db3166a7fd2e0098d1fe4f2ee");

            SugarTemplateNotificationMessage msg = new SugarTemplateNotificationMessage("12", "98", 178);
            if (Manager.SendTemplateMessage("oVn8nv2moexMjC7K7CkakRtYRsIA", string.Empty, msg))
            {
                Console.WriteLine("ok");
            }
            else
            {
                Console.WriteLine("failed");
            }


            //string xml = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1351776360</CreateTime><MsgType><![CDATA[link]]></MsgType><Title><![CDATA[公众平台官网链接]]></Title><Description><![CDATA[公众平台官网链接]]></Description><Url><![CDATA[url]]></Url><MsgId>1234567890123456</MsgId></xml>";
            //MessageManager manager = new MessageManager("abc");
            //MessageBase messageb=manager.ParseToMessage(xml);
            //if (messageb.GetMsgType() == MsgType.link)
            //{
            //    LinkMessage message = messageb as LinkMessage;
            //    Console.WriteLine(message.ToUserName+"\r\n");
            //    Console.WriteLine(message.FromUserName + "\r\n");
            //    Console.WriteLine(message.CreateTime + "\r\n");
            //    Console.WriteLine(message.MsgType + "\r\n");
            //    Console.WriteLine(message.Title+ "\r\n");
            //    Console.WriteLine(message.Description + "\r\n");
            //    Console.WriteLine(message.Url + "\r\n");
            //}
            //else
            //{
            //    Console.WriteLine("error");
            //}

            //AccessTokenProvider atProvider = new AccessTokenProvider("wx56bb5bd00d687d3d", "02ac6f9db3166a7fd2e0098d1fe4f2ee");
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("AccessToken:" + atProvider.GetAccessToken());
            //    System.Threading.Thread.Sleep(1000);
            //}


            Console.ReadKey();
        }
    }
}
