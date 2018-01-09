using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class ErrStr
    {
        public string name;

        public string description;



        /// <summary>
        /// 错误字符串表示
        /// </summary>
        public readonly static ErrStr[] Errors = new HttpParser.ErrStr[] {
            //new HttpParser.http_strerror_tab(){ name="",description=""}
            /* No error */
            new ErrStr(){ name="HPE_OK", description="success" },

            /*Callback-related errors */
            new ErrStr(){ name="CB_message_begin", description="the on_message_begin callback failed"       },
            new ErrStr(){ name="CB_url", description="the on_url callback failed"                           },
            new ErrStr(){ name="CB_header_field", description="the on_header_field callback failed"         },
            new ErrStr(){ name="CB_header_value", description="the on_header_value callback failed"         },
            new ErrStr(){ name="CB_headers_complete", description="the on_headers_complete callback failed" },
            new ErrStr(){ name="CB_body", description="the on_body callback failed"                         },
            new ErrStr(){ name="CB_message_complete", description="the on_message_complete callback failed" },
            new ErrStr(){ name="CB_status", description="the on_status callback failed"                     },
            new ErrStr(){ name="CB_chunk_header", description="the on_chunk_header callback failed"         },
            new ErrStr(){ name="CB_chunk_complete", description="the on_chunk_complete callback failed"     },

            /*Parsing-related errors */
            new ErrStr(){ name="HPE_INVALID_EOF_STATE ", description= "stream ended at an unexpected time" },
            new ErrStr(){ name="HPE_HEADER_OVERFLOW ", description= "too many header bytes seen}, overflow detected"},
            new ErrStr(){ name="HPE_CLOSED_CONNECTION ", description= "data received after completed connection: close message"},
            new ErrStr(){ name="HPE_INVALID_VERSION ", description= "invalid HTTP version"},
            new ErrStr(){ name="HPE_INVALID_STATUS ", description= "invalid HTTP status code"},
            new ErrStr(){ name="HPE_INVALID_METHOD ", description= "invalid HTTP method"},
            new ErrStr(){ name="HPE_INVALID_URL ", description= "invalid URL"},
            new ErrStr(){ name="HPE_INVALID_HOST ", description= "invalid host"},
            new ErrStr(){ name="HPE_INVALID_PORT ", description= "invalid port"},
            new ErrStr(){ name="HPE_INVALID_PATH ", description= "invalid path"},
            new ErrStr(){ name="HPE_INVALID_QUERY_STRING ", description= "invalid query string"},
            new ErrStr(){ name="HPE_INVALID_FRAGMENT ", description= "invalid fragment"},
            new ErrStr(){ name="HPE_LF_EXPECTED ", description= "LF character expected"},
            new ErrStr(){ name="HPE_INVALID_HEADER_TOKEN ", description= "invalid character in header"},
            new ErrStr(){ name="HPE_INVALID_CONTENT_LENGTH ", description= "invalid character in content-length header"},
            new ErrStr(){ name="HPE_UNEXPECTED_CONTENT_LENGTH ", description= "unexpected content-length header"},
            new ErrStr(){ name="HPE_INVALID_CHUNK_SIZE ", description= "invalid character in chunk size header"},
            new ErrStr(){ name="HPE_INVALID_CONSTANT ", description= "invalid constant string"},
            new ErrStr(){ name="HPE_INVALID_INTERNAL_STATE ", description= "encountered unexpected internal state"},
            new ErrStr(){ name="HPE_STRICT ", description= "strict mode assertion failed"},
            new ErrStr(){ name="HPE_PAUSED ", description= "parser is paused"},
            new ErrStr(){ name="HPE_UNKNOWN ", description= "an unknown error occurred"},

        };


        #region const
        /* No error */
        private const string HPE_OK = "success";

        /*Callback-related errors */
        private const string HPE_CB_message_begin = "the on_message_begin callback failed";
        private const string HPE_CB_url = "the on_url callback failed";
        private const string HPE_CB_header_field = "the on_header_field callback failed";
        private const string HPE_CB_header_value = "the on_header_value callback failed";
        private const string HPE_CB_headers_complete = "the on_headers_complete callback failed";
        private const string HPE_CB_body = "the on_body callback failed";
        private const string HPE_CB_message_complete = "the on_message_complete callback failed";
        private const string HPE_CB_status = "the on_status callback failed";
        private const string HPE_CB_chunk_header = "the on_chunk_header callback failed";
        private const string HPE_CB_chunk_complete = "the on_chunk_complete callback failed";

        /*Parsing-related errors */
        private const string HPE_INVALID_EOF_STATE = "stream ended at an unexpected time";
        private const string HPE_HEADER_OVERFLOW = "too many header bytes seen; overflow detected";
        private const string HPE_CLOSED_CONNECTION = "data received after completed connection: close message";
        private const string HPE_INVALID_VERSION = "invalid HTTP version";
        private const string HPE_INVALID_STATUS = "invalid HTTP status code";
        private const string HPE_INVALID_METHOD = "invalid HTTP method";
        private const string HPE_INVALID_URL = "invalid URL";
        private const string HPE_INVALID_HOST = "invalid host";
        private const string HPE_INVALID_PORT = "invalid port";
        private const string HPE_INVALID_PATH = "invalid path";
        private const string HPE_INVALID_QUERY_STRING = "invalid query string";
        private const string HPE_INVALID_FRAGMENT = "invalid fragment";
        private const string HPE_LF_EXPECTED = "LF character expected";
        private const string HPE_INVALID_HEADER_TOKEN = "invalid character in header";
        private const string HPE_INVALID_CONTENT_LENGTH = "invalid character in content-length header";
        private const string HPE_UNEXPECTED_CONTENT_LENGTH = "unexpected content-length header";
        private const string HPE_INVALID_CHUNK_SIZE = "invalid character in chunk size header";
        private const string HPE_INVALID_CONSTANT = "invalid constant string";
        private const string HPE_INVALID_INTERNAL_STATE = "encountered unexpected internal state";
        private const string HPE_STRICT = "strict mode assertion failed";
        private const string HPE_PAUSED = "parser is paused";
        private const string HPE_UNKNOWN = "an unknown error occurred";
        #endregion
    }
}
