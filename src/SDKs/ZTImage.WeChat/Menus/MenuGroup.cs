using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Menus
{
    /// <summary>
    /// 菜单组菜单
    /// 仅一级菜单包含多个菜单时使用
    /// </summary>
    public class MenuGroup:MenuBase
    {
        public MenuGroup(string name):base(name)
        {
            this.sub_button = new List<MenuBase>();
        }
        public List<MenuBase> sub_button { get; set; }

        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="menu"></param>
        public void AddMenu(MenuBase menu)
        {
            this.sub_button.Add(menu);
        }
    }
}
