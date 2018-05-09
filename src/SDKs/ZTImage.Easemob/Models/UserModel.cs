using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Easemob.Models
{
    public class UserModel
    {
        public string uuid { get; set; }

        public string type { get; set; }

        public Int64 created { get; set; }

        public Int64 modified { get; set; }

        public string username { get; set; }

        public Boolean activated { get; set; }

    }
}
