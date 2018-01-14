using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class HttpFrame
    {
        public HttpFrame():this(HttpParserType.HTTP_BOTH)
        {}

        public HttpFrame(HttpParserType parserType)
        {
            this.mSourceType=this.type = parserType;
            Init();
        }

        private void Init()
        {
            this.type = this.mSourceType;
            flags = 0;
            state = (this.type == HttpParserType.HTTP_REQUEST ? State.s_start_req : (this.type == HttpParserType.HTTP_RESPONSE ? State.s_start_res : State.s_start_req_or_res));
            header_state = 0;
            lenient_http_headers = false;
            nread = 0;
            content_length = 0;
            http_major = 0;
            http_minor = 0;
            status_code = 0;
            method = 0;
            http_errno = HttpErrNO.HPE_OK;
            upgrade = false;
            data = default(ArraySegment<byte>);
        }

        /// <summary>
        /// 清理
        /// </summary>
        protected virtual void Clear() { }
        

        /// <summary>
        /// 重置复用
        /// </summary>
        public void Reset()
        {
            Init();
        }


        private HttpParserType mSourceType;
        /// <summary>
        /// enum http_parser_type : 2bits
        /// </summary>
        internal HttpParserType type
        {
            get;
            set;
        }

        internal Flags flags; // F_* values from 'flags' enum; semi-public :8bits


        internal State state; //enum state from http_parser.c :7 bits
        internal HeaderStates header_state; // enum header_state from http_parser.c :7bits
        internal byte index;//index into current matcher :7bits
        internal bool lenient_http_headers = false;//http header 宽容模式 1bits

        internal UInt32 nread;          /* # bytes read in various scenarios */
        public UInt64 content_length
        {
            get;

            set;
        }/* # bytes in body (0 if no Content-Length header) */

        /** READ-ONLY **/
        public byte http_major
        {
            get;
            internal set;
        }
        public byte http_minor
        {
            get;
            internal set;
        }

        /// <summary>
        /// responses only :2bytes
        /// </summary>
        public UInt16 status_code
        {
            get;
            internal set;
        }

        /// <summary>
        /// requests only  :8bits
        /// </summary>
        public HttpMethod method
        {
            get;
            internal set;
        }

        /// <summary>
        /// execute result :7 bits
        /// </summary>
        public HttpErrNO http_errno
        {
            get;
            internal set;
        }




        /* true = Upgrade header was present and the parser has exited because of that.
         * false = No upgrade header present.
         * Should be checked when http_parser_execute() returns in addition to
         * error checking.
         */
        /// <summary>
        /// tras to websocket:1bits
        /// </summary>
        public bool upgrade
        {
            get;
            internal set;
        }

        /// <summary>
        /// A pointer to get hook to the "connection" or "socket" object
        /// </summary>
        public ArraySegment<byte> data
        {
            get;
            set;
        }
    }
}
