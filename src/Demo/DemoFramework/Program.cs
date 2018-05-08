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
            string ok=ZTImage.HttpEx.SyncGet("http://www.cnblogs.com");
            Console.WriteLine(ok);
            




            Console.ReadKey();
        }
    }
}
