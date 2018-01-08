using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class HttpFrame
    {
        public HttpFrame():this(http_parser_type.HTTP_BOTH)
        {

        }

        public HttpFrame(http_parser_type parserType)
        {
            this.type = parserType;
            Init();
        }

        private void Init()
        {
            //this.state = (this.type == http_parser_type.HTTP_REQUEST ? state.s_start_req : (this.type == http_parser_type.HTTP_RESPONSE ? state.s_start_res : state.s_start_req_or_res));
            //this.http_errno = http_errno.HPE_OK;
        }

        public http_parser_type type;//enum http_parser_type : 2bits

        public flags flags; // F_* values from 'flags' enum; semi-public :8bits


        public state state; //enum state from http_parser.c :7 bits
        public header_states header_state; // enum header_state from http_parser.c :7bits
        public byte index;//index into current matcher :7bits
        public bool lenient_http_headers = false;//http header 宽容模式 1bits

        public UInt32 nread;          /* # bytes read in various scenarios */
        public UInt64 content_length; /* # bytes in body (0 if no Content-Length header) */

        /** READ-ONLY **/
        public byte http_major;
        public byte http_minor;
        public UInt16 status_code; /* responses only :2bytes*/
        public http_method method;       /* requests only  :8bits*/
        public http_errno http_errno; //7 bits




        /* 1 = Upgrade header was present and the parser has exited because of that.
         * 0 = No upgrade header present.
         * Should be checked when http_parser_execute() returns in addition to
         * error checking.
         */
        public bool upgrade; //1bits

        /** PUBLIC **/
        public ArraySegment<byte> data; /* A pointer to get hook to the "connection" or "socket" object */
    }
}
