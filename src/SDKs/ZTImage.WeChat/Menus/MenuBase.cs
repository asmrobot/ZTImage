using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Menus
{
    public abstract class MenuBase
    {
        public MenuBase(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// 菜单名称 
        /// </summary>
        public string name { get; set; }
        
    }
}
