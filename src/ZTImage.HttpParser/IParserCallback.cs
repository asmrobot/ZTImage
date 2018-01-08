using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public unsafe interface IParserCallback
    {
        Int32 on_message_begin(HttpFrame frame);

        Int32 on_url(HttpFrame frame,byte* data);

        Int32 on_status(HttpFrame frame,byte* data);

        Int32 on_header_field(HttpFrame frame,byte* data);



        Int32 on_header_value(HttpFrame frame,byte* data);

        Int32 on_headers_complete(HttpFrame frame);

        Int32 on_body(HttpFrame frame,byte* data);


        Int32 on_message_complete(HttpFrame frame);

        /* When on_chunk_header is called, the current chunk length is stored
         * in this.content_length.
         */
        Int32 on_chunk_header(HttpFrame frame);

        Int32 on_chunk_complete(HttpFrame frame);
    }
}
