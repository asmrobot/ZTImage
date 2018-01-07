using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public enum http_errno:byte
    {
        /* No error */
        HPE_OK,

        /*Callback-related errors */
        HPE_CB_message_begin,
        HPE_CB_url,
        HPE_CB_header_field,
        HPE_CB_header_value,
        HPE_CB_headers_complete,
        HPE_CB_body ,
        HPE_CB_message_complete,
        HPE_CB_status,
        HPE_CB_chunk_header,
        HPE_CB_chunk_complete,

        /*Parsing-related errors */
        HPE_INVALID_EOF_STATE,
        HPE_HEADER_OVERFLOW ,
        HPE_CLOSED_CONNECTION,
        HPE_INVALID_VERSION,
        HPE_INVALID_STATUS,
        HPE_INVALID_METHOD,
        HPE_INVALID_URL ,
        HPE_INVALID_HOST ,
        HPE_INVALID_PORT,
        HPE_INVALID_PATH ,
        HPE_INVALID_QUERY_STRING,
        HPE_INVALID_FRAGMENT ,
        HPE_LF_EXPECTED,
        HPE_INVALID_HEADER_TOKEN,
        HPE_INVALID_CONTENT_LENGTH,
        HPE_UNEXPECTED_CONTENT_LENGTH,
        HPE_INVALID_CHUNK_SIZE,
        HPE_INVALID_CONSTANT ,
        HPE_INVALID_INTERNAL_STATE,
        HPE_STRICT,
        HPE_PAUSED ,
        HPE_UNKNOWN 
    }
}
