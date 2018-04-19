using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Menus
{
    /// <summary>
    /// 点击跳转到URL
    /// </summary>
    public class ViewMenu:SimpleMenuBase
    {
        public ViewMenu(string name,string url):base(name,"view")
        {
            this.url = url;
        }

        public string url { get; set; }
    }
}
