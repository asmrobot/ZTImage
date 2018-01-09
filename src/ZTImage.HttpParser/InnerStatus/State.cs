using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    internal enum State:byte
    {
        s_dead = 1 /* important that this is > 0 */
      , s_start_req_or_res
      , s_res_or_resp_H
      , s_start_res
      , s_res_H
      , s_res_HT
      , s_res_HTT
      , s_res_HTTP
      , s_res_http_major
      , s_res_http_dot
      , s_res_http_minor
      , s_res_http_end
      , s_res_first_status_code
      , s_res_status_code
      , s_res_status_start
      , s_res_status
      , s_res_line_almost_done

      , s_start_req

      , s_req_method
      , s_req_spaces_before_url
      , s_req_schema
      , s_req_schema_slash
      , s_req_schema_slash_slash
      , s_req_server_start
      , s_req_server
      , s_req_server_with_at
      , s_req_path
      , s_req_query_string_start
      , s_req_query_string
      , s_req_fragment_start
      , s_req_fragment
      , s_req_http_start
      , s_req_http_H
      , s_req_http_HT
      , s_req_http_HTT
      , s_req_http_HTTP
      , s_req_http_major
      , s_req_http_dot
      , s_req_http_minor
      , s_req_http_end
      , s_req_line_almost_done

      , s_header_field_start
      , s_header_field
      , s_header_value_discard_ws
      , s_header_value_discard_ws_almost_done
      , s_header_value_discard_lws
      , s_header_value_start
      , s_header_value
      , s_header_value_lws

      , s_header_almost_done

      , s_chunk_size_start
      , s_chunk_size
      , s_chunk_parameters
      , s_chunk_size_almost_done

      , s_headers_almost_done
      , s_headers_done

      /* Important: 's_headers_done' must be the last 'header' state. All
       * states beyond this must be 'body' states. It is used for overflow
       * checking. See the PARSING_HEADER() macro.
       */

      , s_chunk_data
      , s_chunk_data_almost_done
      , s_chunk_data_done

      , s_body_identity
      , s_body_identity_eof

      , s_message_done
    }
}
