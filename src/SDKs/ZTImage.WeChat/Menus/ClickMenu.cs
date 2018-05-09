using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Menus
{
    /// <summary>
    /// 点击事件
    /// </summary>
    public class ClickMenu:SimpleMenuBase
    {
        
        public ClickMenu(string name,string key):base(name,"click")
        {
            this.key = key;
        }

        public string key { get; set; }

    }
}
