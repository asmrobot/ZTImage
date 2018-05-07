using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat.Models
{
    /// <summary>
    /// 生成二维码返回模型
    /// </summary>
    public class QRReturnModel
    {
        public string ticket { get; set; }


        public Int64 expire_seconds { get; set; }

        public string url { get; set; }
    }
}
