#define HTTP_PARSER_STRICT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public unsafe delegate Int32 http_data_cb(ParserEngine engine, byte* data);

    public delegate Int32 http_cb(ParserEngine engine);

    public struct http_strerror_tab
    {
        public string name;

        public string description;
    }

    public class ParserEngine
    {
        public ParserEngine() : this(http_parser_type.HTTP_BOTH)
        { }

        public ParserEngine(http_parser_type parserType)
        {
            this.type = parserType;
            Init();
        }

        private const Int32 HTTP_MAX_HEADER_SIZE = 80 * 1024;//头部最大长度
        private readonly string[] method_strings = {
            "DELETE",
            "GET",
            "HEAD",
            "POST",
            "PUT",

            "CONNECT",
            "OPTIONS",
            "TRACE",

            "COPY",
            "LOCK",
            "MKCOL",
            "MOVE",
            "PROPFIND",
            "PROPPATCH",
            "SEARCH",
            "UNLOCK",
            "BIND",
            "REBIND",
            "UNBIND",
            "ACL",

            "REPORT",
            "MKACTIVITY",
            "CHECKOUT",
            "MERGE",

            "M-SEARCH",
            "NOTIFY",
            "SUBSCRIBE",
            "UNSUBSCRIBE",

            "PATCH",
            "PURGE",

            "MKCALENDAR",

            "LINK",
            "UNLINK"
        };//方法字符串

        /// <summary>
        /// 错误字符串表示
        /// </summary>
        private readonly http_strerror_tab[] http_strerror_tab = new HttpParser.http_strerror_tab[] {
            //new HttpParser.http_strerror_tab(){ name="",description=""}
            /* No error */
            new http_strerror_tab(){ name="HPE_OK", description="success" },

            /*Callback-related errors */
            new http_strerror_tab(){ name="CB_message_begin", description="the on_message_begin callback failed"       },
            new http_strerror_tab(){ name="CB_url", description="the on_url callback failed"                           },
            new http_strerror_tab(){ name="CB_header_field", description="the on_header_field callback failed"         },
            new http_strerror_tab(){ name="CB_header_value", description="the on_header_value callback failed"         },
            new http_strerror_tab(){ name="CB_headers_complete", description="the on_headers_complete callback failed" },
            new http_strerror_tab(){ name="CB_body", description="the on_body callback failed"                         },
            new http_strerror_tab(){ name="CB_message_complete", description="the on_message_complete callback failed" },
            new http_strerror_tab(){ name="CB_status", description="the on_status callback failed"                     },
            new http_strerror_tab(){ name="CB_chunk_header", description="the on_chunk_header callback failed"         },
            new http_strerror_tab(){ name="CB_chunk_complete", description="the on_chunk_complete callback failed"     },

            /*Parsing-related errors */
            new http_strerror_tab(){ name="HPE_INVALID_EOF_STATE ", description= "stream ended at an unexpected time" },
            new http_strerror_tab(){ name="HPE_HEADER_OVERFLOW ", description= "too many header bytes seen}, overflow detected"},
            new http_strerror_tab(){ name="HPE_CLOSED_CONNECTION ", description= "data received after completed connection: close message"},
            new http_strerror_tab(){ name="HPE_INVALID_VERSION ", description= "invalid HTTP version"},
            new http_strerror_tab(){ name="HPE_INVALID_STATUS ", description= "invalid HTTP status code"},
            new http_strerror_tab(){ name="HPE_INVALID_METHOD ", description= "invalid HTTP method"},
            new http_strerror_tab(){ name="HPE_INVALID_URL ", description= "invalid URL"},
            new http_strerror_tab(){ name="HPE_INVALID_HOST ", description= "invalid host"},
            new http_strerror_tab(){ name="HPE_INVALID_PORT ", description= "invalid port"},
            new http_strerror_tab(){ name="HPE_INVALID_PATH ", description= "invalid path"},
            new http_strerror_tab(){ name="HPE_INVALID_QUERY_STRING ", description= "invalid query string"},
            new http_strerror_tab(){ name="HPE_INVALID_FRAGMENT ", description= "invalid fragment"},
            new http_strerror_tab(){ name="HPE_LF_EXPECTED ", description= "LF character expected"},
            new http_strerror_tab(){ name="HPE_INVALID_HEADER_TOKEN ", description= "invalid character in header"},
            new http_strerror_tab(){ name="HPE_INVALID_CONTENT_LENGTH ", description= "invalid character in content-length header"},
            new http_strerror_tab(){ name="HPE_UNEXPECTED_CONTENT_LENGTH ", description= "unexpected content-length header"},
            new http_strerror_tab(){ name="HPE_INVALID_CHUNK_SIZE ", description= "invalid character in chunk size header"},
            new http_strerror_tab(){ name="HPE_INVALID_CONSTANT ", description= "invalid constant string"},
            new http_strerror_tab(){ name="HPE_INVALID_INTERNAL_STATE ", description= "encountered unexpected internal state"},
            new http_strerror_tab(){ name="HPE_STRICT ", description= "strict mode assertion failed"},
            new http_strerror_tab(){ name="HPE_PAUSED ", description= "parser is paused"},
            new http_strerror_tab(){ name="HPE_UNKNOWN ", description= "an unknown error occurred"},

        };

        private http_parser_type type;//enum http_parser_type : 2bits

        private flags flags; // F_* values from 'flags' enum; semi-public :8bits
        private state state; //enum state from http_parser.c :7 bits
        private header_states header_state; // enum header_state from http_parser.c :7bits
        private byte index;//index into current matcher :7bits
        private bool lenient_http_headers;//http header 宽容模式 1bits

        private UInt32 nread;          /* # bytes read in various scenarios */
        private UInt64 content_length; /* # bytes in body (0 if no Content-Length header) */

        /** READ-ONLY **/
        private byte http_major;
        private byte http_minor;
        private UInt16 status_code; /* responses only :2bytes*/
        private http_method method;       /* requests only  :8bits*/
        private http_errno http_errno; //7 bits

        /* 1 = Upgrade header was present and the parser has exited because of that.
         * 0 = No upgrade header present.
         * Should be checked when http_parser_execute() returns in addition to
         * error checking.
         */
        private bool upgrade; //1bits

        /** PUBLIC **/
        public ArraySegment<byte> data; /* A pointer to get hook to the "connection" or "socket" object */

        private void Init()
        {
            this.state = (this.type == http_parser_type.HTTP_REQUEST ? state.s_start_req : (this.type == http_parser_type.HTTP_RESPONSE ? state.s_start_res : state.s_start_req_or_res));
            this.http_errno = http_errno.HPE_OK;
        }

        private void SET_ERRNO(http_errno e)
        {
            this.http_errno = e;
        }

        private bool COUNT_HEADER_SIZE(UInt32 V)
        {
            this.nread += (V);
            if (this.nread > HTTP_MAX_HEADER_SIZE)
            {
                SET_ERRNO(http_errno.HPE_HEADER_OVERFLOW);
                return false;
            }
            return true;
        }

        private unsafe Int32 RETURN(byte* currentPTR, byte* sourcePTR)
        {
            return (Int32)(currentPTR - sourcePTR);
        }

        private void UPDATE_STATE(state state)
        {
            this.state = state;
        }

#if HTTP_PARSER_STRICT
        private bool STRICT_CHECK(bool condition)
        {
            if (condition)
            {
                SET_ERRNO(http_errno.HPE_STRICT);
                return true;
            }
            return false;
        }
#else
        private bool STRICT_CHECK(bool condition)
        {
            return false;
        }
#endif
        private char LOWER(char c)
        {
            return (char)(c | 0x20);
        }

        private bool IS_NUM(char c)
        {
            return ((c) >= '0' && (c) <= '9');
        }

        private bool IS_ALPHA(char c)
        {
            return (LOWER(c) >= 'a' && LOWER(c) <= 'z');
        }

        private bool IS_HEX(char c)
        {
            return (IS_NUM(c) || (LOWER(c) >= 'a' && LOWER(c) <= 'f'));
        }

        private bool IS_MARK(char c)
        {
            return ((c) == '-' || (c) == '_' || (c) == '.' || (c) == '!' || (c) == '~' || (c) == '*' || (c) == '\'' || (c) == '(' || (c) == ')');
        }

        private bool IS_ALPHANUM(char c)
        {
            return (IS_ALPHA(c) || IS_NUM(c));
        }

        private bool IS_USERINFO_CHAR(char c)
        {
            return (IS_ALPHANUM(c) || IS_MARK(c) || (c) == '%' ||
                (c) == ';' || (c) == ':' || (c) == '&' || (c) == '=' || (c) == '+' ||
                (c) == '$' || (c) == ',');
        }

        /* Tokens as defined by rfc 2616. Also lowercases them.
         *        token       = 1*<any CHAR except CTLs or separators>
         *     separators     = "(" | ")" | "<" | ">" | "@"
         *                    | "," | ";" | ":" | "\" | <">
         *                    | "/" | "[" | "]" | "?" | "="
         *                    | "{" | "}" | SP | HT
         */
        private readonly char[] tokens = {
        /*   0 nul    1 soh    2 stx    3 etx    4 eot    5 enq    6 ack    7 bel  */
          (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
        /*   8 bs     9 ht    10 nl    11 vt    12 np    13 cr    14 so    15 si   */
          (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
        /*  16 dle   17 dc1   18 dc2   19 dc3   20 dc4   21 nak   22 syn   23 etb */
          (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
        /*  24 can   25 em    26 sub   27 esc   28 fs    29 gs    30 rs    31 us  */
          (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
        /*  32 sp    33  !    34  "    35  #    36  $    37  %    38  &    39  '  */
          (char)0,      '!',(char)0,      '#',     '$',     '%',     '&',    '\'',
        /*  40  (    41  )    42  *    43  +    44  ,    45  -    46  .    47  /  */
          (char)0, (char)0,      '*',     '+',(char)0,      '-',     '.',(char)0,
        /*  48  0    49  1    50  2    51  3    52  4    53  5    54  6    55  7  */
               '0',     '1',     '2',     '3',     '4',     '5',     '6',     '7',
        /*  56  8    57  9    58  :    59  ;    60  <    61  =    62  >    63  ?  */
               '8',     '9',(char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
        /*  64  @    65  A    66  B    67  C    68  D    69  E    70  F    71  G  */
          (char)0,      'a',     'b',     'c',     'd',     'e',     'f',     'g',
        /*  72  H    73  I    74  J    75  K    76  L    77  M    78  N    79  O  */
               'h',     'i',     'j',     'k',     'l',     'm',     'n',     'o',
        /*  80  P    81  Q    82  R    83  S    84  T    85  U    86  V    87  W  */
               'p',     'q',     'r',     's',     't',     'u',     'v',     'w',
        /*  88  X    89  Y    90  Z    91  [    92  \    93  ]    94  ^    95  _  */
               'x',     'y',     'z',(char)0, (char)0, (char)0,      '^',     '_',
        /*  96  `    97  a    98  b    99  c   100  d   101  e   102  f   103  g  */
               '`',     'a',     'b',     'c',     'd',     'e',     'f',     'g',
        /* 104  h   105  i   106  j   107  k   108  l   109  m   110  n   111  o  */
               'h',     'i',     'j',     'k',     'l',     'm',     'n',     'o',
        /* 112  p   113  q   114  r   115  s   116  t   117  u   118  v   119  w  */
               'p',     'q',     'r',     's',     't',     'u',     'v',     'w',
        /* 120  x   121  y   122  z   123  {   124  |   125  }   126  ~   127 del */
               'x',     'y',     'z',(char)0,      '|',(char)0,      '~', (char)0 };


        private readonly static sbyte[] unhex =
          {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
          ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
          ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
          , 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,-1,-1,-1,-1,-1,-1
          ,-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1
          ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
          ,-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1
          ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
          };




        private readonly byte[] normal_url_char = {
        /*   0 nul    1 soh    2 stx    3 etx    4 eot    5 enq    6 ack    7 bel  */
                0    |   0    |   0    |   0    |   0    |   0    |   0    |   0,
            /*   8 bs     9 ht    10 nl    11 vt    12 np    13 cr    14 so    15 si   */
#if HTTP_PARSER_STRICT
                0    |   0   |    0    |   0    |   0  |     0    |   0    |   0,
#else
                0    |   2    |   0    |   0    |   16   |   0    |   0    |   0,
#endif
        /*  16 dle   17 dc1   18 dc2   19 dc3   20 dc4   21 nak   22 syn   23 etb */
                0    |   0    |   0    |   0    |   0    |   0    |   0    |   0,
        /*  24 can   25 em    26 sub   27 esc   28 fs    29 gs    30 rs    31 us  */
                0    |   0    |   0    |   0    |   0    |   0    |   0    |   0,
        /*  32 sp    33  !    34  "    35  #    36  $    37  %    38  &    39  '  */
                0    |   2    |   4    |   0    |   16   |   32   |   64   |  128,
        /*  40  (    41  )    42  *    43  +    44  ,    45  -    46  .    47  /  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /*  48  0    49  1    50  2    51  3    52  4    53  5    54  6    55  7  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /*  56  8    57  9    58  :    59  ;    60  <    61  =    62  >    63  ?  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |   0,
        /*  64  @    65  A    66  B    67  C    68  D    69  E    70  F    71  G  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /*  72  H    73  I    74  J    75  K    76  L    77  M    78  N    79  O  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /*  80  P    81  Q    82  R    83  S    84  T    85  U    86  V    87  W  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /*  88  X    89  Y    90  Z    91  [    92  \    93  ]    94  ^    95  _  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /*  96  `    97  a    98  b    99  c   100  d   101  e   102  f   103  g  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /* 104  h   105  i   106  j   107  k   108  l   109  m   110  n   111  o  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /* 112  p   113  q   114  r   115  s   116  t   117  u   118  v   119  w  */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |  128,
        /* 120  x   121  y   122  z   123  {   124  |   125  }   126  ~   127 del */
                1    |   2    |   4    |   8    |   16   |   32   |   64   |   0, };

        private char STRICT_TOKEN(char c)
        {
            return tokens[(byte)(c)];

        }

        private bool BIT_AT(byte[] a, byte i)
        {
            //todo:logic is error
            return true;
            //return (!!((UInt32)(a)[(UInt32)(i) >> 3] & (1 << ((UInt32)(i) & 7))));
        }



#if HTTP_PARSER_STRICT
        private char TOKEN(char c)
        {
            return tokens[(byte)(c)];
        }

        private bool IS_URL_CHAR(char c)
        {
            return BIT_AT(normal_url_char, (byte)c);
        }

        private bool IS_HOST_CHAR(char c)
        {
            return (IS_ALPHANUM(c) || (c) == '.' || (c) == '-');
        }

#else
        private char TOKEN(char c)
        {
            return ((c == ' ') ? ' ' : tokens[(byte)c]);
        }

        private bool IS_URL_CHAR(char c)
        {
            return BIT_AT(normal_url_char, (byte)c) || ((((byte)c) & 0x80)==0x80);
        }

        private bool IS_HOST_CHAR(char c)
        {
            return (IS_ALPHANUM(c) || (c) == '.' || (c) == '-' || (c) == '_');
        }
#endif


        private state parse_url_char(state s, char ch)
        {
            if (ch == ' ' || ch == '\r' || ch == '\n')
            {
                return state.s_dead;
            }

#if HTTP_PARSER_STRICT
            if (ch == '\t' || ch == '\f')
            {
                return state.s_dead;
            }
#endif

            switch (s)
            {
                case state.s_req_spaces_before_url:
                    /* Proxied requests are followed by scheme of an absolute URI (alpha).
                     * All methods except CONNECT are followed by '/' or '*'.
                     */

                    if (ch == '/' || ch == '*')
                    {
                        return state.s_req_path;
                    }

                    if (IS_ALPHA(ch))
                    {
                        return state.s_req_schema;
                    }

                    break;

                case state.s_req_schema:
                    if (IS_ALPHA(ch))
                    {
                        return s;
                    }

                    if (ch == ':')
                    {
                        return state.s_req_schema_slash;
                    }

                    break;

                case state.s_req_schema_slash:
                    if (ch == '/')
                    {
                        return state.s_req_schema_slash_slash;
                    }

                    break;

                case state.s_req_schema_slash_slash:
                    if (ch == '/')
                    {
                        return state.s_req_server_start;
                    }

                    break;

                case state.s_req_server_with_at:

                /* FALLTHROUGH */
                case state.s_req_server_start:
                case state.s_req_server:

                    if (s == state.s_req_server_with_at && ch == '@')
                    {
                        return state.s_dead;
                    }

                    if (ch == '/')
                    {
                        return state.s_req_path;
                    }

                    if (ch == '?')
                    {
                        return state.s_req_query_string_start;
                    }

                    if (ch == '@')
                    {
                        return state.s_req_server_with_at;
                    }

                    if (IS_USERINFO_CHAR(ch) || ch == '[' || ch == ']')
                    {
                        return state.s_req_server;
                    }

                    break;

                case state.s_req_path:
                    if (IS_URL_CHAR(ch))
                    {
                        return s;
                    }

                    switch (ch)
                    {
                        case '?':
                            return state.s_req_query_string_start;

                        case '#':
                            return state.s_req_fragment_start;
                    }

                    break;

                case state.s_req_query_string_start:
                case state.s_req_query_string:
                    if (IS_URL_CHAR(ch))
                    {
                        return state.s_req_query_string;
                    }

                    switch (ch)
                    {
                        case '?':
                            /* allow extra '?' in query string */
                            return state.s_req_query_string;

                        case '#':
                            return state.s_req_fragment_start;
                    }

                    break;

                case state.s_req_fragment_start:
                    if (IS_URL_CHAR(ch))
                    {
                        return state.s_req_fragment;
                    }

                    switch (ch)
                    {
                        case '?':
                            return state.s_req_fragment;

                        case '#':
                            return s;
                    }

                    break;

                case state.s_req_fragment:
                    if (IS_URL_CHAR(ch))
                    {
                        return s;
                    }

                    switch (ch)
                    {
                        case '?':
                        case '#':
                            return s;
                    }

                    break;

                default:
                    break;
            }

            /* We should never fall out of the switch above unless there's an error */
            return state.s_dead;
        }

        public unsafe Int32 Execute(byte* data, Int32 len)
        {
            char ch;
            char c;
            byte unhex_val;
            byte* p = data;
            byte* header_field_mark = null;
            byte* header_value_mark = null;
            byte* url_mark = null;
            byte* body_mark = null;
            byte* status_mark = null;

            if (this.http_errno != http_errno.HPE_OK)
            {
                return 0;
            }
            if (len == 0)
            {
                switch (this.state)
                {
                    case state.s_body_identity_eof:
                        /* Use of CALLBACK_NOTIFY() here would erroneously return 1 byte read if
                        * we got paused.
                        */
                        if (!RaiseMessageComplete())
                        {
                            return RETURN(p, data);
                        }
                        return 0;
                    case state.s_dead:
                    case state.s_start_req_or_res:
                    case state.s_start_res:
                    case state.s_start_req:
                        return 0;
                    default:
                        SET_ERRNO(http_errno.HPE_INVALID_EOF_STATE);
                        return 1;
                }
            }

            if (this.state == state.s_header_field)
            {
                header_field_mark = data;
            }

            if (this.state == state.s_header_value)
            {
                header_value_mark = data;
            }

            switch (this.state)
            {
                case state.s_req_path:
                case state.s_req_schema:
                case state.s_req_schema_slash:
                case state.s_req_schema_slash_slash:
                case state.s_req_server_start:
                case state.s_req_server:
                case state.s_req_server_with_at:
                case state.s_req_query_string_start:
                case state.s_req_query_string:
                case state.s_req_fragment_start:
                case state.s_req_fragment:
                    url_mark = data;
                    break;
                case state.s_res_status:
                    status_mark = data;
                    break;
                default:
                    break;
            }

            for (p = data; p != data + len; p++)
            {
                ch = (char)*p;
                if (state <= state.s_headers_done)
                {
                    if (!COUNT_HEADER_SIZE(1))
                    {
                        goto error;
                    }
                }

                reexecute:

                switch (this.state)
                {
                    case state.s_dead:
                        if (ch == CR || ch == LF)
                        {
                            break;
                        }
                        SET_ERRNO(http_errno.HPE_CLOSED_CONNECTION);
                        goto error;

                    case state.s_start_req_or_res:
                        {
                            if (ch == CR || ch == LF)
                            {
                                break;
                            }
                            this.flags = 0;
                            this.content_length = UInt64.MaxValue;

                            if (ch == 'H')
                            {
                                UPDATE_STATE(state.s_res_or_resp_H);
                                if (!RaiseMessageBegin())
                                {
                                    return RETURN(p, data);
                                }
                            }
                            else
                            {
                                this.type = http_parser_type.HTTP_REQUEST;
                                UPDATE_STATE(state.s_start_req);
                                goto reexecute;
                            }
                            break;
                        }
                    case state.s_res_or_resp_H:
                        if (ch == 'T')
                        {
                            this.type = http_parser_type.HTTP_RESPONSE;
                            UPDATE_STATE(state.s_res_HT);
                        }
                        else
                        {
                            if (ch != 'E')
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_CONSTANT);
                                goto error;
                            }

                            this.type = http_parser_type.HTTP_REQUEST;
                            this.method = http_method.HEAD;
                            this.index = 2;
                            UPDATE_STATE(state.s_req_method);
                        }
                        break;
                    case state.s_start_res:
                        {
                            this.flags = 0;
                            this.content_length = UInt64.MaxValue;

                            switch (ch)
                            {
                                case 'H':
                                    UPDATE_STATE(state.s_res_H);
                                    break;

                                case CR:
                                case LF:
                                    break;

                                default:
                                    SET_ERRNO(http_errno.HPE_INVALID_CONSTANT);
                                    goto error;
                            }

                            if (!RaiseMessageBegin())
                            {
                                return RETURN(p, data);
                            }
                            break;
                        }
                    case state.s_res_H:
                        if (STRICT_CHECK(ch != 'T'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(state.s_res_HT);
                        break;

                    case state.s_res_HT:
                        if (STRICT_CHECK(ch != 'T'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(state.s_res_HTT);
                        break;

                    case state.s_res_HTT:
                        if (STRICT_CHECK(ch != 'P'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(state.s_res_HTTP);
                        break;

                    case state.s_res_HTTP:
                        if (STRICT_CHECK(ch != '/'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(state.s_res_http_major);
                        break;

                    case state.s_res_http_major:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                            goto error;
                        }

                        this.http_major = (byte)(ch - '0');
                        UPDATE_STATE(state.s_res_http_dot);
                        break;

                    case state.s_res_http_dot:
                        {
                            if (ch != '.')
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                                goto error;
                            }

                            UPDATE_STATE(state.s_res_http_minor);
                            break;
                        }

                    case state.s_res_http_minor:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                            goto error;
                        }

                        http_minor = (byte)(ch - '0');
                        UPDATE_STATE(state.s_res_http_end);
                        break;

                    case state.s_res_http_end:
                        {
                            if (ch != ' ')
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                                goto error;
                            }

                            UPDATE_STATE(state.s_res_first_status_code);
                            break;
                        }

                    case state.s_res_first_status_code:
                        {
                            if (!IS_NUM(ch))
                            {
                                if (ch == ' ')
                                {
                                    break;
                                }

                                SET_ERRNO(http_errno.HPE_INVALID_STATUS);
                                goto error;
                            }
                            status_code = (UInt16)(ch - '0');
                            UPDATE_STATE(state.s_res_status_code);
                            break;
                        }

                    case state.s_res_status_code:
                        {
                            if (!IS_NUM(ch))
                            {
                                switch (ch)
                                {
                                    case ' ':
                                        UPDATE_STATE(state.s_res_status_start);
                                        break;
                                    case CR:
                                    case LF:
                                        UPDATE_STATE(state.s_res_status_start);
                                        goto reexecute;
                                    default:
                                        SET_ERRNO(http_errno.HPE_INVALID_STATUS);
                                        goto error;
                                }
                                break;
                            }

                            this.status_code *= 10;
                            this.status_code += (UInt16)(ch - '0');

                            if (this.status_code > 999)
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_STATUS);
                                goto error;
                            }

                            break;
                        }

                    case state.s_res_status_start:
                        {
                            if (status_mark == null)
                            {
                                status_mark = p;
                            }
                            UPDATE_STATE(state.s_res_status);
                            this.index = 0;

                            if (ch == CR || ch == LF)
                                goto reexecute;

                            break;
                        }

                    case state.s_res_status:
                        if (ch == CR)
                        {
                            UPDATE_STATE(state.s_res_line_almost_done);
                            if (!RaiseStatus(p))
                            {
                                return RETURN(p, data);
                            }
                            //CALLBACK_DATA(tatus);
                            break;
                        }

                        if (ch == LF)
                        {
                            UPDATE_STATE(state.s_header_field_start);
                            if (!RaiseStatus(p))
                            {
                                return RETURN(p, data);
                            }
                            //CALLBACK_DATA(status);
                            break;
                        }

                        break;

                    case state.s_res_line_almost_done:
                        STRICT_CHECK(ch != LF);
                        UPDATE_STATE(state.s_header_field_start);
                        break;
                    case state.s_start_req:
                        {
                            if (ch == CR || ch == LF)
                                break;
                            this.flags = 0;
                            this.content_length = UInt64.MaxValue;

                            if (!IS_ALPHA(ch))
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_METHOD);
                                goto error;
                            }

                            this.method = (http_method)0;
                            this.index = 1;
                            switch (ch)
                            {
                                case 'A': this.method = http_method.ACL; break;
                                case 'B': this.method = http_method.BIND; break;
                                case 'C': this.method = http_method.CONNECT; /* or COPY, CHECKOUT */ break;
                                case 'D': this.method = http_method.DELETE; break;
                                case 'G': this.method = http_method.GET; break;
                                case 'H': this.method = http_method.HEAD; break;
                                case 'L': this.method = http_method.LOCK; /* or LINK */ break;
                                case 'M': this.method = http_method.MKCOL; /* or MOVE, MKACTIVITY, MERGE, M-SEARCH, MKCALENDAR */ break;
                                case 'N': this.method = http_method.NOTIFY; break;
                                case 'O': this.method = http_method.OPTIONS; break;
                                case 'P':
                                    this.method = http_method.POST;
                                    /* or PROPFIND|PROPPATCH|PUT|PATCH|PURGE */
                                    break;
                                case 'R': this.method = http_method.REPORT; /* or REBIND */ break;
                                case 'S': this.method = http_method.SUBSCRIBE; /* or SEARCH */ break;
                                case 'T': this.method = http_method.TRACE; break;
                                case 'U': this.method = http_method.UNLOCK; /* or UNSUBSCRIBE, UNBIND, UNLINK */ break;
                                default:
                                    SET_ERRNO(http_errno.HPE_INVALID_METHOD);
                                    goto error;
                            }
                            UPDATE_STATE(state.s_req_method);
                            if (!RaiseMessageBegin())
                            {
                                return RETURN(p, data);
                            }
                            //CALLBACK_NOTIFY(message_begin);

                            break;
                        }
                    case state.s_req_method:
                        {
                            if (ch == '\0')
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_METHOD);
                                goto error;
                            }

                            string matcher = method_strings[(byte)this.method];
                            if (ch == ' ' && this.index >= matcher.Length)
                            {
                                UPDATE_STATE(state.s_req_spaces_before_url);
                            }
                            else if (ch == matcher[this.index])
                            {
                                ; /* nada */
                            }
                            else if ((ch >= 'A' && ch <= 'Z') || ch == '-')
                            {
                                byte m = (byte)this.method;
                                switch (((byte)this.method) << 16 | this.index << 8 | ch)
                                {
                                    case (((byte)http_method.POST) << 16 | 1 << 8 | (byte)'U'):
                                        this.method = http_method.PUT;
                                        break;
                                    case (((byte)http_method.POST) << 16 | 1 << 8 | (byte)'A'):
                                        this.method = http_method.PATCH;
                                        break;
                                    case (((byte)http_method.POST) << 16 | 1 << 8 | (byte)'R'):
                                        this.method = http_method.PROPFIND;
                                        break;
                                    case (((byte)http_method.PUT) << 16 | 2 << 8 | (byte)'R'):
                                        this.method = http_method.PURGE;
                                        break;

                                    case (((byte)http_method.CONNECT) << 16 | 1 << 8 | (byte)'H'):
                                        this.method = http_method.CHECKOUT;
                                        break;
                                    case (((byte)http_method.CONNECT) << 16 | 2 << 8 | (byte)'P'):
                                        this.method = http_method.COPY;
                                        break;


                                    case (((byte)http_method.MKCOL) << 16 | 1 << 8 | (byte)'O'):
                                        this.method = http_method.MOVE;
                                        break;
                                    case (((byte)http_method.MKCOL) << 16 | 1 << 8 | (byte)'E'):
                                        this.method = http_method.MERGE;
                                        break;
                                    case (((byte)http_method.MKCOL) << 16 | 1 << 8 | (byte)'S'):
                                        this.method = http_method.MSEARCH;
                                        break;
                                    case (((byte)http_method.MKCOL) << 16 | 2 << 8 | (byte)'A'):
                                        this.method = http_method.MKACTIVITY;
                                        break;
                                    case (((byte)http_method.MKCOL) << 16 | 3 << 8 | (byte)'A'):
                                        this.method = http_method.MKCALENDAR;
                                        break;


                                    //XX(SUBSCRIBE, 1, 'E', SEARCH)
                                    case (((byte)http_method.SUBSCRIBE) << 16 | 1 << 8 | (byte)'E'):
                                        this.method = http_method.SEARCH;
                                        break;
                                    //XX(REPORT, 2, 'B', REBIND)
                                    case (((byte)http_method.REPORT) << 16 | 2 << 8 | (byte)'B'):
                                        this.method = http_method.REBIND;
                                        break;
                                    //XX(PROPFIND, 4, 'P', PROPPATCH)
                                    case (((byte)http_method.PROPFIND) << 16 | 4 << 8 | (byte)'P'):
                                        this.method = http_method.PROPPATCH;
                                        break;
                                    //XX(LOCK, 1, 'I', LINK)
                                    case (((byte)http_method.LOCK) << 16 | 1 << 8 | (byte)'I'):
                                        this.method = http_method.LINK;
                                        break;
                                    //XX(UNLOCK, 2, 'S', UNSUBSCRIBE)
                                    case (((byte)http_method.UNLOCK) << 16 | 2 << 8 | (byte)'S'):
                                        this.method = http_method.UNSUBSCRIBE;
                                        break;
                                    //XX(UNLOCK, 2, 'B', UNBIND)
                                    case (((byte)http_method.UNLOCK) << 16 | 2 << 8 | (byte)'B'):
                                        this.method = http_method.UNBIND;
                                        break;
                                    //XX(UNLOCK, 3, 'I', UNLINK)
                                    case (((byte)http_method.UNLOCK) << 16 | 3 << 8 | (byte)'I'):
                                        this.method = http_method.UNLINK;
                                        break;

                                    default:
                                        SET_ERRNO(http_errno.HPE_INVALID_METHOD);
                                        goto error;
                                }
                            }
                            else
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_METHOD);
                                goto error;
                            }

                            ++this.index;
                            break;
                        }

                    case state.s_req_spaces_before_url:
                        {
                            if (ch == ' ') break;
                            if (url_mark == null)
                            {
                                url_mark = p;
                            }

                            if (this.method == http_method.CONNECT)
                            {
                                UPDATE_STATE(state.s_req_server_start);
                            }

                            UPDATE_STATE(parse_url_char(this.state, ch));
                            if (this.state == state.s_dead)
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_URL);
                                goto error;
                            }

                            break;
                        }

                    case state.s_req_schema:
                    case state.s_req_schema_slash:
                    case state.s_req_schema_slash_slash:
                    case state.s_req_server_start:
                        {
                            switch (ch)
                            {
                                /* No whitespace allowed here */
                                case ' ':
                                case CR:
                                case LF:
                                    SET_ERRNO(http_errno.HPE_INVALID_URL);
                                    goto error;
                                default:
                                    UPDATE_STATE(parse_url_char(this.state, ch));
                                    if (this.state == state.s_dead)
                                    {
                                        SET_ERRNO(http_errno.HPE_INVALID_URL);
                                        goto error;
                                    }
                                    break;
                            }

                            break;
                        }

                    case state.s_req_server:
                    case state.s_req_server_with_at:
                    case state.s_req_path:
                    case state.s_req_query_string_start:
                    case state.s_req_query_string:
                    case state.s_req_fragment_start:
                    case state.s_req_fragment:
                        {
                            switch (ch)
                            {
                                case ' ':
                                    UPDATE_STATE(state.s_req_http_start);
                                    if (!RaiseUrl(url_mark))
                                    {
                                        return RETURN(p, data);
                                    }
                                    //CALLBACK_DATA(url);
                                    break;
                                case CR:
                                case LF:
                                    this.http_major = 0;
                                    this.http_minor = 9;
                                    UPDATE_STATE((ch == CR) ?
                                      state.s_req_line_almost_done :
                                      state.s_header_field_start);
                                    if (!RaiseUrl(url_mark))
                                    {
                                        return RETURN(p, data);
                                    }
                                    //CALLBACK_DATA(url);
                                    break;
                                default:
                                    UPDATE_STATE(parse_url_char(this.state, ch));
                                    if (this.state == state.s_dead)
                                    {
                                        SET_ERRNO(http_errno.HPE_INVALID_URL);
                                        goto error;
                                    }
                                    break;
                            }
                            break;
                        }

                    case state.s_req_http_start:
                        switch (ch)
                        {
                            case 'H':
                                UPDATE_STATE(state.s_req_http_H);
                                break;
                            case ' ':
                                break;
                            default:
                                SET_ERRNO(http_errno.HPE_INVALID_CONSTANT);
                                goto error;
                        }
                        break;

                    case state.s_req_http_H:
                        STRICT_CHECK(ch != 'T');
                        UPDATE_STATE(state.s_req_http_HT);
                        break;

                    case state.s_req_http_HT:
                        STRICT_CHECK(ch != 'T');
                        UPDATE_STATE(state.s_req_http_HTT);
                        break;

                    case state.s_req_http_HTT:
                        STRICT_CHECK(ch != 'P');
                        UPDATE_STATE(state.s_req_http_HTTP);
                        break;

                    case state.s_req_http_HTTP:
                        STRICT_CHECK(ch != '/');
                        UPDATE_STATE(state.s_req_http_major);
                        break;

                    case state.s_req_http_major:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                            goto error;
                        }

                        this.http_major = (byte)(ch - '0');
                        UPDATE_STATE(state.s_req_http_dot);
                        break;

                    case state.s_req_http_dot:
                        {
                            if (ch != '.')
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                                goto error;
                            }

                            UPDATE_STATE(state.s_req_http_minor);
                            break;
                        }

                    case state.s_req_http_minor:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                            goto error;
                        }

                        this.http_minor = (byte)(ch - '0');
                        UPDATE_STATE(state.s_req_http_end);
                        break;

                    case state.s_req_http_end:
                        {
                            if (ch == CR)
                            {
                                UPDATE_STATE(state.s_req_line_almost_done);
                                break;
                            }

                            if (ch == LF)
                            {
                                UPDATE_STATE(state.s_header_field_start);
                                break;
                            }

                            SET_ERRNO(http_errno.HPE_INVALID_VERSION);
                            goto error;
                            break;
                        }

                    /* end of request line */
                    case state.s_req_line_almost_done:
                        {
                            if (ch != LF)
                            {
                                SET_ERRNO(http_errno.HPE_LF_EXPECTED);
                                goto error;
                            }

                            UPDATE_STATE(state.s_header_field_start);
                            break;
                        }

                    case state.s_header_field_start:
                        {
                            if (ch == CR)
                            {
                                UPDATE_STATE(state.s_headers_almost_done);
                                break;
                            }

                            if (ch == LF)
                            {
                                /* they might be just sending \n instead of \r\n so this would be
                                 * the second \n to denote the end of headers*/
                                UPDATE_STATE(state.s_headers_almost_done);
                                goto reexecute;
                            }

                            c = TOKEN(ch);

                            //todo:condition
                            if (c==0)
                            {
                                SET_ERRNO(http_errno.HPE_INVALID_HEADER_TOKEN);
                                goto error;
                            }
                            if (header_field_mark == null)
                            {
                                header_field_mark = p;
                            }
                            //MARK(header_field);

                            this.index = 0;
                            UPDATE_STATE(state.s_header_field);

                            switch (c)
                            {
                                case 'c':
                                    this.header_state = header_states.h_C;
                                    break;

                                case 'p':
                                    this.header_state = header_states.h_matching_proxy_connection;
                                    break;

                                case 't':
                                    this.header_state = header_states.h_matching_transfer_encoding;
                                    break;

                                case 'u':
                                    this.header_state = header_states.h_matching_upgrade;
                                    break;

                                default:
                                    this.header_state = header_states.h_general;
                                    break;
                            }
                            break;
                        }

                    case state.s_header_field:
                        {
                            byte* start = p;
                            for (; p != data + len; p++)
                            {
                                ch = (char)*p;
                                c = TOKEN(ch);

                                if (c==0)
                                    break;

                                switch (this.header_state)
                                {
                                    case header_states.h_general:
                                        break;

                                    case header_states.h_C:
                                        this.index++;
                                        this.header_state = (c == 'o' ? header_states.h_CO : header_states.h_general);
                                        break;

                                    case header_states.h_CO:
                                        this.index++;
                                        this.header_state = (c == 'n' ? header_states.h_CON : header_states.h_general);
                                        break;

                                    case header_states.h_CON:
                                        this.index++;
                                        switch (c)
                                        {
                                            case 'n':
                                                this.header_state = header_states.h_matching_connection;
                                                break;
                                            case 't':
                                                this.header_state = header_states.h_matching_content_length;
                                                break;
                                            default:
                                                this.header_state = header_states.h_general;
                                                break;
                                        }
                                        break;

                                    /* connection */

                                    case header_states.h_matching_connection:
                                        this.index++;
                                        if (this.index > CONNECTION.Length - 1
                                            || c != CONNECTION[this.index])
                                        {
                                            this.header_state = header_states.h_general;
                                        }
                                        else if (this.index == CONNECTION.Length - 2)
                                        {
                                            this.header_state = header_states.h_connection;
                                        }
                                        break;

                                    /* proxy-connection */

                                    case header_states.h_matching_proxy_connection:
                                        this.index++;
                                        if (this.index > PROXY_CONNECTION.Length - 1
                                            || c != PROXY_CONNECTION[this.index])
                                        {
                                            this.header_state = header_states.h_general;
                                        }
                                        else if (this.index == PROXY_CONNECTION.Length - 2)
                                        {
                                            this.header_state = header_states.h_connection;
                                        }
                                        break;

                                    /* content-length */

                                    case header_states.h_matching_content_length:
                                        this.index++;
                                        if (this.index > CONTENT_LENGTH.Length - 1
                                            || c != CONTENT_LENGTH[this.index])
                                        {
                                            this.header_state = header_states.h_general;
                                        }
                                        else if (this.index == CONTENT_LENGTH.Length - 2)
                                        {
                                            this.header_state = header_states.h_content_length;
                                        }
                                        break;

                                    /* transfer-encoding */

                                    case header_states.h_matching_transfer_encoding:
                                        this.index++;
                                        if (this.index > TRANSFER_ENCODING.Length - 1
                                            || c != TRANSFER_ENCODING[this.index])
                                        {
                                            this.header_state = header_states.h_general;
                                        }
                                        else if (this.index == TRANSFER_ENCODING.Length - 2)
                                        {
                                            this.header_state = header_states.h_transfer_encoding;
                                        }
                                        break;

                                    /* upgrade */

                                    case header_states.h_matching_upgrade:
                                        this.index++;
                                        if (this.index > UPGRADE.Length - 1
                                            || c != UPGRADE[this.index])
                                        {
                                            this.header_state = header_states.h_general;
                                        }
                                        else if (this.index == UPGRADE.Length - 2)
                                        {
                                            this.header_state = header_states.h_upgrade;
                                        }
                                        break;

                                    case header_states.h_connection:
                                    case header_states.h_content_length:
                                    case header_states.h_transfer_encoding:
                                    case header_states.h_upgrade:
                                        if (ch != ' ') this.header_state = header_states.h_general;
                                        break;

                                    default:
                                        //assert(0 && "Unknown header_state");
                                        break;
                                }
                            }

                            COUNT_HEADER_SIZE((UInt32)(p - start));

                            if (p == data + len)
                            {
                                --p;
                                break;
                            }

                            if (ch == ':')
                            {
                                UPDATE_STATE(state.s_header_value_discard_ws);
                                if (!RaiseHeaderField(header_field_mark))
                                {
                                    return RETURN(p, data);
                                }
                                //CALLBACK_DATA(header_field);
                                break;
                            }

                            SET_ERRNO(http_errno.HPE_INVALID_HEADER_TOKEN);
                            goto error;
                        }

                    //case s_header_value_discard_ws:
                    //  if (ch == ' ' || ch == '\t') break;

                    //  if (ch == CR) {
                    //    UPDATE_STATE(s_header_value_discard_ws_almost_done);
                    //    break;
                    //  }

                    //  if (ch == LF) {
                    //    UPDATE_STATE(s_header_value_discard_lws);
                    //    break;
                    //  }

                    //  /* FALLTHROUGH */

                    //case s_header_value_start:
                    //{
                    //  MARK(header_value);

                    //  UPDATE_STATE(s_header_value);
                    //  this.index = 0;

                    //  c = LOWER(ch);

                    //  switch (this.header_state) {
                    //    case h_upgrade:
                    //      this.flags |= F_UPGRADE;
                    //      this.header_state = h_general;
                    //      break;

                    //    case h_transfer_encoding:
                    //      /* looking for 'Transfer-Encoding: chunked' */
                    //      if ('c' == c) {
                    //        this.header_state = h_matching_transfer_encoding_chunked;
                    //      } else {
                    //        this.header_state = h_general;
                    //      }
                    //      break;

                    //    case h_content_length:
                    //      if (UNLIKELY(!IS_NUM(ch))) {
                    //        SET_ERRNO(HPE_INVALID_CONTENT_LENGTH);
                    //        goto error;
                    //      }

                    //      if (this.flags & F_CONTENTLENGTH) {
                    //        SET_ERRNO(HPE_UNEXPECTED_CONTENT_LENGTH);
                    //        goto error;
                    //      }

                    //      this.flags |= F_CONTENTLENGTH;
                    //      this.content_length = ch - '0';
                    //      break;

                    //    case h_connection:
                    //      /* looking for 'Connection: keep-alive' */
                    //      if (c == 'k') {
                    //        this.header_state = h_matching_connection_keep_alive;
                    //      /* looking for 'Connection: close' */
                    //      } else if (c == 'c') {
                    //        this.header_state = h_matching_connection_close;
                    //      } else if (c == 'u') {
                    //        this.header_state = h_matching_connection_upgrade;
                    //      } else {
                    //        this.header_state = h_matching_connection_token;
                    //      }
                    //      break;

                    //    /* Multi-value `Connection` header */
                    //    case h_matching_connection_token_start:
                    //      break;

                    //    default:
                    //      this.header_state = h_general;
                    //      break;
                    //  }
                    //  break;
                    //}

                    //case s_header_value:
                    //{
                    //  const char* start = p;
                    //  enum header_states h_state = (enum header_states) this.header_state;
                    //  for (; p != data + len; p++) {
                    //    ch = *p;
                    //    if (ch == CR) {
                    //      UPDATE_STATE(s_header_almost_done);
                    //      this.header_state = h_state;
                    //      CALLBACK_DATA(header_value);
                    //      break;
                    //    }

                    //    if (ch == LF) {
                    //      UPDATE_STATE(s_header_almost_done);
                    //      COUNT_HEADER_SIZE(p - start);
                    //      this.header_state = h_state;
                    //      CALLBACK_DATA_NOADVANCE(header_value);
                    //      REEXECUTE();
                    //    }

                    //    if (!lenient && !IS_HEADER_CHAR(ch)) {
                    //      SET_ERRNO(HPE_INVALID_HEADER_TOKEN);
                    //      goto error;
                    //    }

                    //    c = LOWER(ch);

                    //    switch (h_state) {
                    //      case h_general:
                    //      {
                    //        const char* p_cr;
                    //        const char* p_lf;
                    //        size_t limit = data + len - p;

                    //        limit = MIN(limit, HTTP_MAX_HEADER_SIZE);

                    //        p_cr = (const char*) memchr(p, CR, limit);
                    //        p_lf = (const char*) memchr(p, LF, limit);
                    //        if (p_cr != NULL) {
                    //          if (p_lf != NULL && p_cr >= p_lf)
                    //            p = p_lf;
                    //          else
                    //            p = p_cr;
                    //        } else if (UNLIKELY(p_lf != NULL)) {
                    //          p = p_lf;
                    //        } else {
                    //          p = data + len;
                    //        }
                    //        --p;

                    //        break;
                    //      }

                    //      case h_connection:
                    //      case h_transfer_encoding:
                    //        assert(0 && "Shouldn't get here.");
                    //        break;

                    //      case h_content_length:
                    //      {
                    //        uint64_t t;

                    //        if (ch == ' ') break;

                    //        if (UNLIKELY(!IS_NUM(ch))) {
                    //          SET_ERRNO(HPE_INVALID_CONTENT_LENGTH);
                    //          this.header_state = h_state;
                    //          goto error;
                    //        }

                    //        t = this.content_length;
                    //        t *= 10;
                    //        t += ch - '0';

                    //        /* Overflow? Test against a conservative limit for simplicity. */
                    //        if (UNLIKELY((ULLONG_MAX - 10) / 10 < this.content_length)) {
                    //          SET_ERRNO(HPE_INVALID_CONTENT_LENGTH);
                    //          this.header_state = h_state;
                    //          goto error;
                    //        }

                    //        this.content_length = t;
                    //        break;
                    //      }

                    //      /* Transfer-Encoding: chunked */
                    //      case h_matching_transfer_encoding_chunked:
                    //        this.index++;
                    //        if (this.index > sizeof(CHUNKED)-1
                    //            || c != CHUNKED[this.index]) {
                    //          h_state = h_general;
                    //        } else if (this.index == sizeof(CHUNKED)-2) {
                    //          h_state = h_transfer_encoding_chunked;
                    //        }
                    //        break;

                    //      case h_matching_connection_token_start:
                    //        /* looking for 'Connection: keep-alive' */
                    //        if (c == 'k') {
                    //          h_state = h_matching_connection_keep_alive;
                    //        /* looking for 'Connection: close' */
                    //        } else if (c == 'c') {
                    //          h_state = h_matching_connection_close;
                    //        } else if (c == 'u') {
                    //          h_state = h_matching_connection_upgrade;
                    //        } else if (STRICT_TOKEN(c)) {
                    //          h_state = h_matching_connection_token;
                    //        } else if (c == ' ' || c == '\t') {
                    //          /* Skip lws */
                    //        } else {
                    //          h_state = h_general;
                    //        }
                    //        break;

                    //      /* looking for 'Connection: keep-alive' */
                    //      case h_matching_connection_keep_alive:
                    //        this.index++;
                    //        if (this.index > sizeof(KEEP_ALIVE)-1
                    //            || c != KEEP_ALIVE[this.index]) {
                    //          h_state = h_matching_connection_token;
                    //        } else if (this.index == sizeof(KEEP_ALIVE)-2) {
                    //          h_state = h_connection_keep_alive;
                    //        }
                    //        break;

                    //      /* looking for 'Connection: close' */
                    //      case h_matching_connection_close:
                    //        this.index++;
                    //        if (this.index > sizeof(CLOSE)-1 || c != CLOSE[this.index]) {
                    //          h_state = h_matching_connection_token;
                    //        } else if (this.index == sizeof(CLOSE)-2) {
                    //          h_state = h_connection_close;
                    //        }
                    //        break;

                    //      /* looking for 'Connection: upgrade' */
                    //      case h_matching_connection_upgrade:
                    //        this.index++;
                    //        if (this.index > sizeof(UPGRADE) - 1 ||
                    //            c != UPGRADE[this.index]) {
                    //          h_state = h_matching_connection_token;
                    //        } else if (this.index == sizeof(UPGRADE)-2) {
                    //          h_state = h_connection_upgrade;
                    //        }
                    //        break;

                    //      case h_matching_connection_token:
                    //        if (ch == ',') {
                    //          h_state = h_matching_connection_token_start;
                    //          this.index = 0;
                    //        }
                    //        break;

                    //      case h_transfer_encoding_chunked:
                    //        if (ch != ' ') h_state = h_general;
                    //        break;

                    //      case h_connection_keep_alive:
                    //      case h_connection_close:
                    //      case h_connection_upgrade:
                    //        if (ch == ',') {
                    //          if (h_state == h_connection_keep_alive) {
                    //            this.flags |= F_CONNECTION_KEEP_ALIVE;
                    //          } else if (h_state == h_connection_close) {
                    //            this.flags |= F_CONNECTION_CLOSE;
                    //          } else if (h_state == h_connection_upgrade) {
                    //            this.flags |= F_CONNECTION_UPGRADE;
                    //          }
                    //          h_state = h_matching_connection_token_start;
                    //          this.index = 0;
                    //        } else if (ch != ' ') {
                    //          h_state = h_matching_connection_token;
                    //        }
                    //        break;

                    //      default:
                    //        UPDATE_STATE(s_header_value);
                    //        h_state = h_general;
                    //        break;
                    //    }
                    //  }
                    //  this.header_state = h_state;

                    //  COUNT_HEADER_SIZE(p - start);

                    //  if (p == data + len)
                    //    --p;
                    //  break;
                    //}

                    //case s_header_almost_done:
                    //{
                    //  if (UNLIKELY(ch != LF)) {
                    //    SET_ERRNO(HPE_LF_EXPECTED);
                    //    goto error;
                    //  }

                    //  UPDATE_STATE(s_header_value_lws);
                    //  break;
                    //}

                    //case s_header_value_lws:
                    //{
                    //  if (ch == ' ' || ch == '\t') {
                    //    UPDATE_STATE(s_header_value_start);
                    //    REEXECUTE();
                    //  }

                    //  /* finished the header */
                    //  switch (this.header_state) {
                    //    case h_connection_keep_alive:
                    //      this.flags |= F_CONNECTION_KEEP_ALIVE;
                    //      break;
                    //    case h_connection_close:
                    //      this.flags |= F_CONNECTION_CLOSE;
                    //      break;
                    //    case h_transfer_encoding_chunked:
                    //      this.flags |= F_CHUNKED;
                    //      break;
                    //    case h_connection_upgrade:
                    //      this.flags |= F_CONNECTION_UPGRADE;
                    //      break;
                    //    default:
                    //      break;
                    //  }

                    //  UPDATE_STATE(s_header_field_start);
                    //  REEXECUTE();
                    //}

                    //case s_header_value_discard_ws_almost_done:
                    //{
                    //  STRICT_CHECK(ch != LF);
                    //  UPDATE_STATE(s_header_value_discard_lws);
                    //  break;
                    //}

                    //case s_header_value_discard_lws:
                    //{
                    //  if (ch == ' ' || ch == '\t') {
                    //    UPDATE_STATE(s_header_value_discard_ws);
                    //    break;
                    //  } else {
                    //    switch (this.header_state) {
                    //      case h_connection_keep_alive:
                    //        this.flags |= F_CONNECTION_KEEP_ALIVE;
                    //        break;
                    //      case h_connection_close:
                    //        this.flags |= F_CONNECTION_CLOSE;
                    //        break;
                    //      case h_connection_upgrade:
                    //        this.flags |= F_CONNECTION_UPGRADE;
                    //        break;
                    //      case h_transfer_encoding_chunked:
                    //        this.flags |= F_CHUNKED;
                    //        break;
                    //      default:
                    //        break;
                    //    }

                    //    /* header value was empty */
                    //    MARK(header_value);
                    //    UPDATE_STATE(s_header_field_start);
                    //    CALLBACK_DATA_NOADVANCE(header_value);
                    //    REEXECUTE();
                    //  }
                    //}

                    //case s_headers_almost_done:
                    //{
                    //  STRICT_CHECK(ch != LF);

                    //  if (this.flags & F_TRAILING) {
                    //    /* End of a chunked request */
                    //    UPDATE_STATE(s_message_done);
                    //    CALLBACK_NOTIFY_NOADVANCE(chunk_complete);
                    //    REEXECUTE();
                    //  }

                    //  /* Cannot use chunked encoding and a content-length header together
                    //     per the HTTP specification. */
                    //  if ((this.flags & F_CHUNKED) &&
                    //      (this.flags & F_CONTENTLENGTH)) {
                    //    SET_ERRNO(HPE_UNEXPECTED_CONTENT_LENGTH);
                    //    goto error;
                    //  }

                    //  UPDATE_STATE(s_headers_done);

                    //  /* Set this here so that on_headers_complete() callbacks can see it */
                    //  if ((this.flags & F_UPGRADE) &&
                    //      (this.flags & F_CONNECTION_UPGRADE)) {
                    //    /* For responses, "Upgrade: foo" and "Connection: upgrade" are
                    //     * mandatory only when it is a 101 Switching Protocols response,
                    //     * otherwise it is purely informational, to announce support.
                    //     */
                    //    this.upgrade =
                    //        (this.type == HTTP_REQUEST || this.status_code == 101);
                    //  } else {
                    //    this.upgrade = (this.method == HTTP_CONNECT);
                    //  }

                    //  /* Here we call the headers_complete callback. This is somewhat
                    //   * different than other callbacks because if the user returns 1, we
                    //   * will interpret that as saying that this message has no body. This
                    //   * is needed for the annoying case of recieving a response to a HEAD
                    //   * request.
                    //   *
                    //   * We'd like to use CALLBACK_NOTIFY_NOADVANCE() here but we cannot, so
                    //   * we have to simulate it by handling a change in errno below.
                    //   */
                    //  if (settings->on_headers_complete) {
                    //    switch (settings->on_headers_complete(parser)) {
                    //      case 0:
                    //        break;

                    //      case 2:
                    //        this.upgrade = 1;

                    //      /* FALLTHROUGH */
                    //      case 1:
                    //        this.flags |= F_SKIPBODY;
                    //        break;

                    //      default:
                    //        SET_ERRNO(HPE_CB_headers_complete);
                    //        RETURN(p - data); /* Error */
                    //    }
                    //  }

                    //  if (HTTP_PARSER_ERRNO(parser) != HPE_OK) {
                    //    RETURN(p - data);
                    //  }

                    //  REEXECUTE();
                    //}

                    //case s_headers_done:
                    //{
                    //  int hasBody;
                    //  STRICT_CHECK(ch != LF);

                    //  this.nread = 0;

                    //  hasBody = this.flags & F_CHUNKED ||
                    //    (this.content_length > 0 && this.content_length != ULLONG_MAX);
                    //  if (this.upgrade && (this.method == HTTP_CONNECT ||
                    //                          (this.flags & F_SKIPBODY) || !hasBody)) {
                    //    /* Exit, the rest of the message is in a different protocol. */
                    //    UPDATE_STATE(NEW_MESSAGE());
                    //    CALLBACK_NOTIFY(message_complete);
                    //    RETURN((p - data) + 1);
                    //  }

                    //  if (this.flags & F_SKIPBODY) {
                    //    UPDATE_STATE(NEW_MESSAGE());
                    //    CALLBACK_NOTIFY(message_complete);
                    //  } else if (this.flags & F_CHUNKED) {
                    //    /* chunked encoding - ignore Content-Length header */
                    //    UPDATE_STATE(s_chunk_size_start);
                    //  } else {
                    //    if (this.content_length == 0) {
                    //      /* Content-Length header given but zero: Content-Length: 0\r\n */
                    //      UPDATE_STATE(NEW_MESSAGE());
                    //      CALLBACK_NOTIFY(message_complete);
                    //    } else if (this.content_length != ULLONG_MAX) {
                    //      /* Content-Length header given and non-zero */
                    //      UPDATE_STATE(s_body_identity);
                    //    } else {
                    //      if (!http_message_needs_eof(parser)) {
                    //        /* Assume content-length 0 - read the next */
                    //        UPDATE_STATE(NEW_MESSAGE());
                    //        CALLBACK_NOTIFY(message_complete);
                    //      } else {
                    //        /* Read body until EOF */
                    //        UPDATE_STATE(s_body_identity_eof);
                    //      }
                    //    }
                    //  }

                    //  break;
                    //}

                    //case s_body_identity:
                    //{
                    //  uint64_t to_read = MIN(this.content_length,
                    //                         (uint64_t) ((data + len) - p));

                    //  assert(this.content_length != 0
                    //      && this.content_length != ULLONG_MAX);

                    //  /* The difference between advancing content_length and p is because
                    //   * the latter will automaticaly advance on the next loop iteration.
                    //   * Further, if content_length ends up at 0, we want to see the last
                    //   * byte again for our message complete callback.
                    //   */
                    //  MARK(body);
                    //  this.content_length -= to_read;
                    //  p += to_read - 1;

                    //  if (this.content_length == 0) {
                    //    UPDATE_STATE(s_message_done);

                    //    /* Mimic CALLBACK_DATA_NOADVANCE() but with one extra byte.
                    //     *
                    //     * The alternative to doing this is to wait for the next byte to
                    //     * trigger the data callback, just as in every other case. The
                    //     * problem with this is that this makes it difficult for the test
                    //     * harness to distinguish between complete-on-EOF and
                    //     * complete-on-length. It's not clear that this distinction is
                    //     * important for applications, but let's keep it for now.
                    //     */
                    //    CALLBACK_DATA_(body, p - body_mark + 1, p - data);
                    //    REEXECUTE();
                    //  }

                    //  break;
                    //}

                    /* read until EOF */
                    case state.s_body_identity_eof:
                        if (body_mark == null)
                        {
                            body_mark = p;
                        }
                        //MARK(body);
                        p = data + len - 1;

                        break;

                    case state.s_message_done:
                        UPDATE_STATE(NEW_MESSAGE());
                        CALLBACK_NOTIFY(message_complete);
                        if (this.upgrade)
                        {
                            /* todo:Exit, the rest of the message is in a different protocol. */
                            //return RETURN((p - data) + 1);
                            return RETURN(p, data);
                        }
                        break;

                    case state.s_chunk_size_start:
                        {
                            assert(this.nread == 1);
                            assert(this.flags & flags.F_CHUNKED);

                            unhex_val = unhex[(byte)ch];
                            if (UNLIKELY(unhex_val == -1))
                            {
                                SET_ERRNO(HPE_INVALID_CHUNK_SIZE);
                                goto error;
                            }

                            this.content_length = unhex_val;
                            UPDATE_STATE(s_chunk_size);
                            break;
                        }

                    case state.s_chunk_size:
                        {
                            UInt64 t;

                            //assert(this.flags & flags.F_CHUNKED);

                            if (ch == CR)
                            {
                                UPDATE_STATE(state.s_chunk_size_almost_done);
                                break;
                            }

                            unhex_val = unhex[(unsigned char)ch];

                            if (unhex_val == -1)
                            {
                                if (ch == ';' || ch == ' ')
                                {
                                    UPDATE_STATE(state.s_chunk_parameters);
                                    break;
                                }

                                SET_ERRNO(HPE_INVALID_CHUNK_SIZE);
                                goto error;
                            }

                            t = this.content_length;
                            t *= 16;
                            t += unhex_val;

                            /* Overflow? Test against a conservative limit for simplicity. */
                            if (UNLIKELY((ULLONG_MAX - 16) / 16 < this.content_length))
                            {
                                SET_ERRNO(HPE_INVALID_CONTENT_LENGTH);
                                goto error;
                            }

                            this.content_length = t;
                            break;
                        }

                    case state.s_chunk_parameters:
                        {
                            assert(this.flags & F_CHUNKED);
                            /* just ignore this shit. TODO check for overflow */
                            if (ch == CR)
                            {
                                UPDATE_STATE(s_chunk_size_almost_done);
                                break;
                            }
                            break;
                        }

                    case state.s_chunk_size_almost_done:
                        {
                            assert(this.flags & F_CHUNKED);
                            STRICT_CHECK(ch != LF);

                            this.nread = 0;

                            if (this.content_length == 0)
                            {
                                this.flags |= F_TRAILING;
                                UPDATE_STATE(s_header_field_start);
                            }
                            else
                            {
                                UPDATE_STATE(s_chunk_data);
                            }
                            CALLBACK_NOTIFY(chunk_header);
                            break;
                        }

                    case state.s_chunk_data:
                        {
                            UInt64 to_read = Math.Min(this.content_length,
                                                   (UInt64)((data + len) - p));

                            assert(this.flags & flags.F_CHUNKED);
                            assert(this.content_length != 0
                                && this.content_length != UInt64.MaxValue);

                            /* See the explanation in s_body_identity for why the content
                             * length and data pointers are managed this way.
                             */
                            MARK(body);
                            this.content_length -= to_read;
                            p += to_read - 1;

                            if (this.content_length == 0)
                            {
                                UPDATE_STATE(state.s_chunk_data_almost_done);
                            }

                            break;
                        }

                    case state.s_chunk_data_almost_done:
                        //assert(this.flags & F_CHUNKED);
                        //assert(this.content_length == 0);
                        STRICT_CHECK(ch != CR);
                        UPDATE_STATE(state.s_chunk_data_done);
                        CALLBACK_DATA(body);
                        break;

                    case state.s_chunk_data_done:
                        //assert(this.flags & F_CHUNKED);
                        STRICT_CHECK(ch != LF);
                        this.nread = 0;
                        UPDATE_STATE(state.s_chunk_size_start);
                        if (!RaiseChunkComplete())
                        {

                        }
                        //CALLBACK_NOTIFY(chunk_complete);
                        break;

                    default:
                        //assert(0 && "unhandled state");
                        SET_ERRNO(http_errno.HPE_INVALID_INTERNAL_STATE);
                        goto error;
























                }//switch end



            }//loop end



            System.Diagnostics.Debug.Assert(((header_field_mark != null ? 1 : 0) + (header_value_mark != null ? 1 : 0) + (url_mark != null ? 1 : 0) + (body_mark != null ? 1 : 0) + (status_mark != null ? 1 : 0)) <= 1);


            //CALLBACK_DATA_NOADVANCE(header_field);
            RaiseHeaderField(header_field_mark);
            //CALLBACK_DATA_NOADVANCE(header_value);
            RaiseHeaderValue(header_value_mark);
            //CALLBACK_DATA_NOADVANCE(url);
            RaiseUrl(url_mark);
            //CALLBACK_DATA_NOADVANCE(body);
            RaiseBody(body_mark);
            //CALLBACK_DATA_NOADVANCE(status);
            RaiseStatus(status_mark);

            return len;


            error:
            if (this.http_errno == http_errno.HPE_OK)
            {
                SET_ERRNO(http_errno.HPE_UNKNOWN);
            }
            return RETURN(p, data);
        }




        #region events
        public event http_cb on_message_begin;
        private bool RaiseMessageBegin()
        {
            if (on_message_begin != null)
            {
                if (on_message_begin(this) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_message_begin);
                    return false;
                }
            }

            return true;
        }

        public http_data_cb on_url;
        private unsafe bool RaiseUrl(byte* data)
        {
            if (on_url != null)
            {
                if (on_url(this, data) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_url);
                    return false;
                }
            }

            return true;
        }

        public http_data_cb on_status;
        private unsafe bool RaiseStatus(byte* data)
        {
            if (on_status != null)
            {
                if (on_status(this, data) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_status);
                    return false;
                }
            }

            return true;
        }

        public http_data_cb on_header_field;
        private unsafe bool RaiseHeaderField(byte* data)
        {
            if (on_header_field != null)
            {
                if (on_header_field(this, data) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_header_field);
                    return false;
                }
            }

            return true;
        }


        public http_data_cb on_header_value;
        private unsafe bool RaiseHeaderValue(byte* data)
        {
            if (on_header_value != null)
            {
                if (on_header_value(this, data) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_header_value);
                    return false;
                }
            }

            return true;
        }


        public http_cb on_headers_complete;
        private bool RaiseHeaderComplete()
        {
            if (on_headers_complete != null)
            {
                if (on_headers_complete(this) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_headers_complete);
                    return false;
                }
            }

            return true;
        }
        public http_data_cb on_body;
        private unsafe bool RaiseBody(byte* data)
        {
            if (on_body != null)
            {
                if (on_body(this, data) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_body);
                    return false;
                }
            }

            return true;
        }


        public http_cb on_message_complete;
        private bool RaiseMessageComplete()
        {
            if (on_message_complete != null)
            {
                if (on_message_complete(this) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_message_complete);
                    return false;
                }
            }

            return true;
        }
        /* When on_chunk_header is called, the current chunk length is stored
         * in this.content_length.
         */
        public event http_cb on_chunk_header;
        private bool RaiseChunkHeader()
        {
            if (on_chunk_header != null)
            {
                if (on_chunk_header(this) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_chunk_header);
                    return false;
                }
            }

            return true;
        }
        public event http_cb on_chunk_complete;
        private bool RaiseChunkComplete()
        {
            if (on_chunk_complete != null)
            {
                if (on_chunk_complete(this) != 0)
                {
                    SET_ERRNO(http_errno.HPE_CB_chunk_complete);
                    return false;
                }
            }

            return true;
        }
        #endregion


        #region header const

        private const char CR = '\r';
        private const char LF = '\n';

        private const string PROXY_CONNECTION = "proxy-connection";
        private const string CONNECTION = "connection";
        private const string CONTENT_LENGTH = "content-length";
        private const string TRANSFER_ENCODING = "transfer-encoding";
        private const string UPGRADE = "upgrade";
        private const string CHUNKED = "chunked";
        private const string KEEP_ALIVE = "keep-alive";
        private const string CLOSE = "close";
        #endregion

        #region http error string

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
