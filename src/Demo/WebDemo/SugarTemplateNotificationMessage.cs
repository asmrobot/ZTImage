using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDemo
{
    public class SugarTemplateNotificationMessage : ZTImage.WeChat.TemplateMessageBase
    {
        public SugarTemplateNotificationMessage(string sugar,string hr,Int32 height)
        {
            this.AddDataItem("sugar", sugar, "#cccccc");
            this.AddDataItem("hr", hr, "#aaaaaa");
            this.AddDataItem("height", height.ToString(), "#bbbbbb");
        }

        public override string template_id
        {
            get
            {
                return "SxFFMbKscXlfXTcZSzLwa-FyY6ljWfQumko_L6_yRv0";
            }
        }


    }
}
