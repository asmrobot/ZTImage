using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class DefaultHttpFrame:HttpFrame
    {
        public HttpStatus Status
        {
            get;
            set;
        }

        public string GetValue()
        {
            return string.Empty;
        }

        public void GetHeaders()
        {

        }
    }
}
