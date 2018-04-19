using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Menus
{
    public class SimpleMenuBase:MenuBase
    {
        public SimpleMenuBase(string name,string type):base(name)
        {
            this.type = type;
        }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public string type { get; set; }
    }
}
