using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class ZTRequest:ZTHttpFrame
    {
        public ContentEncoding[] AcceptEncoding
        {
            get;
            private set;
        }

        public override void AddHeader(string headerKey, string headerValue)
        {
            base.AddHeader(headerKey, headerValue);
        }
    }
}
