using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class ZTHttpFrame:HttpFrame
    {
        protected static readonly Encoding DefaultEncoding = System.Text.Encoding.UTF8;

        //private Dictionary<string, string> mQueryString = new Dictionary<string, string>();

        //private Dictionary<string, string> mForm = new Dictionary<string, string>();

        private Dictionary<string, string> mHeader = new Dictionary<string, string>();

        public string GetHeader(string headerKey)
        {

            if (string.IsNullOrWhiteSpace(headerKey))
            {
                return string.Empty;
            }
            headerKey = headerKey.Trim().ToUpper();
            if (mHeader.ContainsKey(headerKey))
            {
                return mHeader[headerKey];
            }
            return string.Empty;
        }

        public virtual void AddHeader(string headerKey,string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerKey))
            {
                return;
            }

            headerKey = headerKey.Trim().ToUpper();
            if (!mHeader.ContainsKey(headerKey))
            {
                mHeader.Add(headerKey, headerValue);
            }
            return;
        }



        protected List<ArraySegment<byte>> mContent = new List<ArraySegment<byte>>();

        public virtual void AddContent(ArraySegment<byte> chunk)
        {
            if (chunk.Array == null || chunk.Count <= 0)
            {
                return;
            }

            //Content-Encoding

            mContent.Add(chunk);
        }

        protected override void Clear()
        {
            base.Clear();
            mHeader.Clear();
            mContent.Clear();
        }


        public virtual string GetContent()
        {

            Encoding encode = DefaultEncoding;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.mContent.Count; i++)
            {
                builder.Append(encode.GetString(this.mContent[i].Array, this.mContent[i].Offset, this.mContent[i].Count));
            }

            return builder.ToString();
        }



    }
}
