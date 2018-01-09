#define HTTP_PARSER_STRICT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public class ParserEngine
    {

        #region const

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

        public ParserEngine()
        {
            this.callback = EmptyParserCallback.Define;
        }
        public ParserEngine(IParserCallback callback)
        {
            this.callback = callback;
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

        private IParserCallback callback;

        private void assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception("assert error");
            }
        }

        private void SET_ERRNO(HttpFrame frame, HttpErrNO e)
        {
            frame.http_errno = e;
        }

        private bool COUNT_HEADER_SIZE(HttpFrame frame, UInt32 V)
        {
            frame.nread += (V);
            if (frame.nread > HTTP_MAX_HEADER_SIZE)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_HEADER_OVERFLOW);
                return false;
            }
            return true;
        }

        private unsafe Int32 RETURN(byte* currentPTR, byte* sourcePTR)
        {
            return (Int32)(currentPTR - sourcePTR);
        }

        private void UPDATE_STATE(HttpFrame frame, State state)
        {
            frame.state = state;
        }

#if HTTP_PARSER_STRICT
        private bool STRICT_CHECK(HttpFrame frame, bool condition)
        {
            if (condition)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_STRICT);
                return true;
            }
            return false;
        }
#else
        private bool STRICT_CHECK(frame,bool condition)
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

        private bool STRICT_TOKEN(char c)
        {
            return tokens[(byte)(c)] != 0;
        }

        private bool BIT_AT(byte[] a, byte i)
        {
            return (
                (UInt32)((a)[(UInt32)(i) >> 3]) &
                (1 << ((Int32)(i) & 7))
                ) != 0;
        }

        /**
         * Verify that a char is a valid visible (printable) US-ASCII
         * character or %x80-FF
         **/
        private bool IS_HEADER_CHAR(char ch)
        {
            return (ch == CR || ch == LF || ch == 9 || ((byte)ch > 31 && ch != 127));
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


        private State parse_url_char(State s, char ch)
        {
            if (ch == ' ' || ch == '\r' || ch == '\n')
            {
                return State.s_dead;
            }

#if HTTP_PARSER_STRICT
            if (ch == '\t' || ch == '\f')
            {
                return State.s_dead;
            }
#endif

            switch (s)
            {
                case State.s_req_spaces_before_url:
                    /* Proxied requests are followed by scheme of an absolute URI (alpha).
                     * All methods except CONNECT are followed by '/' or '*'.
                     */

                    if (ch == '/' || ch == '*')
                    {
                        return State.s_req_path;
                    }

                    if (IS_ALPHA(ch))
                    {
                        return State.s_req_schema;
                    }

                    break;

                case State.s_req_schema:
                    if (IS_ALPHA(ch))
                    {
                        return s;
                    }

                    if (ch == ':')
                    {
                        return State.s_req_schema_slash;
                    }

                    break;

                case State.s_req_schema_slash:
                    if (ch == '/')
                    {
                        return State.s_req_schema_slash_slash;
                    }

                    break;

                case State.s_req_schema_slash_slash:
                    if (ch == '/')
                    {
                        return State.s_req_server_start;
                    }

                    break;

                case State.s_req_server_with_at:

                /* FALLTHROUGH */
                case State.s_req_server_start:
                case State.s_req_server:

                    if (s == State.s_req_server_with_at && ch == '@')
                    {
                        return State.s_dead;
                    }

                    if (ch == '/')
                    {
                        return State.s_req_path;
                    }

                    if (ch == '?')
                    {
                        return State.s_req_query_string_start;
                    }

                    if (ch == '@')
                    {
                        return State.s_req_server_with_at;
                    }

                    if (IS_USERINFO_CHAR(ch) || ch == '[' || ch == ']')
                    {
                        return State.s_req_server;
                    }

                    break;

                case State.s_req_path:
                    if (IS_URL_CHAR(ch))
                    {
                        return s;
                    }

                    switch (ch)
                    {
                        case '?':
                            return State.s_req_query_string_start;

                        case '#':
                            return State.s_req_fragment_start;
                    }

                    break;

                case State.s_req_query_string_start:
                case State.s_req_query_string:
                    if (IS_URL_CHAR(ch))
                    {
                        return State.s_req_query_string;
                    }

                    switch (ch)
                    {
                        case '?':
                            /* allow extra '?' in query string */
                            return State.s_req_query_string;

                        case '#':
                            return State.s_req_fragment_start;
                    }

                    break;

                case State.s_req_fragment_start:
                    if (IS_URL_CHAR(ch))
                    {
                        return State.s_req_fragment;
                    }

                    switch (ch)
                    {
                        case '?':
                            return State.s_req_fragment;

                        case '#':
                            return s;
                    }

                    break;

                case State.s_req_fragment:
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
            return State.s_dead;
        }


        /* Does the parser need to see an EOF to find the end of the message? */
        private bool http_message_needs_eof(HttpFrame frame)
        {
            if (frame.type == HttpParserType.HTTP_REQUEST)
            {
                return false;
            }

            /* See RFC 2616 section 4.4 */
            if (frame.status_code / 100 == 1 || /* 1xx e.g. Continue */
                frame.status_code == 204 ||     /* No Content */
                frame.status_code == 304 ||     /* Not Modified */
                ((frame.flags & Flags.F_SKIPBODY) == Flags.F_SKIPBODY))
            {     /* response to a HEAD request */
                return false;
            }

            if (((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED) || frame.content_length != UInt64.MaxValue)
            {
                return false;
            }

            return true;
        }



        private bool http_should_keep_alive(HttpFrame frame)
        {
            if (frame.http_major > 0 && frame.http_minor > 0)
            {
                /*todo:read it  HTTP/1.1 */
                if ((frame.flags & Flags.F_CONNECTION_CLOSE) == Flags.F_CONNECTION_CLOSE)
                {
                    return false;
                }
            }
            else
            {
                /* todo:readit HTTP/1.0 or earlier */
                if ((frame.flags & Flags.F_CONNECTION_KEEP_ALIVE) != Flags.F_CONNECTION_KEEP_ALIVE)
                {
                    return false;
                }
            }

            return !http_message_needs_eof(frame);
        }

        private State start_state(HttpFrame frame)
        {
            return (frame.type == HttpParserType.HTTP_REQUEST ? State.s_start_req : State.s_start_res);
        }

        private State NEW_MESSAGE(HttpFrame frame)
        {
#if HTTP_PARSER_STRICT
            return (http_should_keep_alive(frame) ? start_state(frame) : State.s_dead);
#else
            return start_state();
#endif
        }

        /// <summary>
        /// 查找字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private unsafe byte* memchr(byte* str, char c, Int32 limit)
        {
            byte* p = str;
            byte* target = str + limit;
            while (p < target)
            {
                if (*p == c)
                {
                    return p;
                }
                p++;
            }
            return null;
        }


        public Int32 Execute(HttpFrame frame, ArraySegment<byte> data)
        {
            return Execute(frame, data.Array, data.Offset, data.Count);
        }

        public unsafe Int32 Execute(HttpFrame frame, byte[] data)
        {
            return Execute(frame, data, 0, data.Length);
        }


        public unsafe Int32 Execute(HttpFrame frame, byte[] array, Int32 offset, Int32 count)
        {
            GCHandle handle=GCHandle.Alloc(array, GCHandleType.Pinned);
            
            try
            {
                byte* source = (byte*)handle.AddrOfPinnedObject().ToPointer();
                return Execute(frame, array,offset, count, source);
            }
            finally
            {
                handle.Free();
            }
        }

        private unsafe Int32 Execute(HttpFrame frame,byte[] array,Int32 offset, Int32 count, byte* data)
        {
            char ch,c;
            sbyte unhex_val;
            byte* p = data;

            Int32 header_field_mark = -1;
            Int32 header_value_mark = -1;
            Int32 url_mark = -1;
            Int32 body_mark = -1;
            Int32 status_mark = -1;

            bool lenient = frame.lenient_http_headers;

            if (frame.http_errno != HttpErrNO.HPE_OK)
            {
                return 0;
            }
            if (count == 0)
            {
                switch (frame.state)
                {
                    case State.s_body_identity_eof:
                        /* Use of CALLBACK_NOTIFY() here would erroneously return 1 byte read if
                        * we got paused.
                        */
                        if (!RaiseMessageComplete(frame))
                        {
                            return RETURN(p, data);
                        }
                        return 0;
                    case State.s_dead:
                    case State.s_start_req_or_res:
                    case State.s_start_res:
                    case State.s_start_req:
                        return 0;
                    default:
                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_EOF_STATE);
                        return 1;
                }
            }

            if (frame.state == State.s_header_field)
            {
                header_field_mark = offset;
            }

            if (frame.state == State.s_header_value)
            {
                header_value_mark = offset;
            }

            switch (frame.state)
            {
                case State.s_req_path:
                case State.s_req_schema:
                case State.s_req_schema_slash:
                case State.s_req_schema_slash_slash:
                case State.s_req_server_start:
                case State.s_req_server:
                case State.s_req_server_with_at:
                case State.s_req_query_string_start:
                case State.s_req_query_string:
                case State.s_req_fragment_start:
                case State.s_req_fragment:
                    url_mark = offset;
                    break;
                case State.s_res_status:
                    status_mark = offset;
                    break;
                default:
                    break;
            }

            for (p = data; p != data + count; p++)
            {
                ch = (char)*p;
                if (frame.state <= State.s_headers_done)
                {
                    if (!COUNT_HEADER_SIZE(frame, 1))
                    {
                        goto error;
                    }
                }

                reexecute:

                switch (frame.state)
                {
                    case State.s_dead:
                        if (ch == CR || ch == LF)
                        {
                            break;
                        }
                        SET_ERRNO(frame, HttpErrNO.HPE_CLOSED_CONNECTION);
                        goto error;

                    case State.s_start_req_or_res:
                        {
                            if (ch == CR || ch == LF)
                            {
                                break;
                            }
                            frame.flags = 0;
                            frame.content_length = UInt64.MaxValue;

                            if (ch == 'H')
                            {
                                UPDATE_STATE(frame, State.s_res_or_resp_H);

                                if (!RaiseMessageBegin(frame))
                                {
                                    return RETURN(p, data);
                                }
                            }
                            else
                            {
                                frame.type = HttpParserType.HTTP_REQUEST;
                                UPDATE_STATE(frame, State.s_start_req);
                                goto reexecute;
                            }
                            break;
                        }
                    case State.s_res_or_resp_H:
                        if (ch == 'T')
                        {
                            frame.type = HttpParserType.HTTP_RESPONSE;
                            UPDATE_STATE(frame, State.s_res_HT);
                        }
                        else
                        {
                            if (ch != 'E')
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONSTANT);
                                goto error;
                            }

                            frame.type = HttpParserType.HTTP_REQUEST;
                            frame.method = HttpMethod.HEAD;
                            frame.index = 2;
                            UPDATE_STATE(frame, State.s_req_method);
                        }
                        break;
                    case State.s_start_res:
                        {
                            frame.flags = 0;
                            frame.content_length = UInt64.MaxValue;

                            switch (ch)
                            {
                                case 'H':
                                    UPDATE_STATE(frame, State.s_res_H);
                                    break;

                                case CR:
                                case LF:
                                    break;

                                default:
                                    SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONSTANT);
                                    goto error;
                            }

                            if (!RaiseMessageBegin(frame))
                            {
                                return RETURN(p, data);
                            }
                            break;
                        }
                    case State.s_res_H:
                        if (STRICT_CHECK(frame, ch != 'T'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(frame, State.s_res_HT);
                        break;

                    case State.s_res_HT:
                        if (STRICT_CHECK(frame, ch != 'T'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(frame, State.s_res_HTT);
                        break;

                    case State.s_res_HTT:
                        if (STRICT_CHECK(frame, ch != 'P'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(frame, State.s_res_HTTP);
                        break;

                    case State.s_res_HTTP:
                        if (STRICT_CHECK(frame, ch != '/'))
                        {
                            goto error;
                        }
                        UPDATE_STATE(frame, State.s_res_http_major);
                        break;

                    case State.s_res_http_major:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                            goto error;
                        }

                        frame.http_major = (byte)(ch - '0');
                        UPDATE_STATE(frame, State.s_res_http_dot);
                        break;

                    case State.s_res_http_dot:
                        {
                            if (ch != '.')
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                                goto error;
                            }

                            UPDATE_STATE(frame, State.s_res_http_minor);
                            break;
                        }

                    case State.s_res_http_minor:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                            goto error;
                        }

                        frame.http_minor = (byte)(ch - '0');
                        UPDATE_STATE(frame, State.s_res_http_end);
                        break;

                    case State.s_res_http_end:
                        {
                            if (ch != ' ')
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                                goto error;
                            }

                            UPDATE_STATE(frame, State.s_res_first_status_code);
                            break;
                        }

                    case State.s_res_first_status_code:
                        {
                            if (!IS_NUM(ch))
                            {
                                if (ch == ' ')
                                {
                                    break;
                                }

                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_STATUS);
                                goto error;
                            }
                            frame.status_code = (UInt16)(ch - '0');
                            UPDATE_STATE(frame, State.s_res_status_code);
                            break;
                        }

                    case State.s_res_status_code:
                        {
                            if (!IS_NUM(ch))
                            {
                                switch (ch)
                                {
                                    case ' ':
                                        UPDATE_STATE(frame, State.s_res_status_start);
                                        break;
                                    case CR:
                                    case LF:
                                        UPDATE_STATE(frame, State.s_res_status_start);
                                        goto reexecute;
                                    default:
                                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_STATUS);
                                        goto error;
                                }
                                break;
                            }

                            frame.status_code *= 10;
                            frame.status_code += (UInt16)(ch - '0');

                            if (frame.status_code > 999)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_STATUS);
                                goto error;
                            }

                            break;
                        }

                    case State.s_res_status_start:
                        {
                            if (status_mark == -1)
                            {
                                status_mark = (Int32)(p - data) + offset;
                            }
                            UPDATE_STATE(frame, State.s_res_status);
                            frame.index = 0;

                            if (ch == CR || ch == LF)
                                goto reexecute;

                            break;
                        }

                    case State.s_res_status:
                        if (ch == CR)
                        {
                            UPDATE_STATE(frame, State.s_res_line_almost_done);
                            if (!RaiseStatus(frame,array, ref status_mark, (Int32)(p - data) + offset))
                            {
                                return RETURN(p, data);
                            }
                            //CALLBACK_DATA(status);
                            break;
                        }

                        if (ch == LF)
                        {
                            UPDATE_STATE(frame, State.s_header_field_start);
                            if (!RaiseStatus(frame,array, ref status_mark, (Int32)(p - data) + offset))
                            {
                                return RETURN(p, data);
                            }
                            //CALLBACK_DATA(status);
                            break;
                        }

                        break;

                    case State.s_res_line_almost_done:
                        STRICT_CHECK(frame, ch != LF);
                        UPDATE_STATE(frame, State.s_header_field_start);
                        break;
                    case State.s_start_req:
                        {
                            if (ch == CR || ch == LF)
                                break;
                            frame.flags = 0;
                            frame.content_length = UInt64.MaxValue;

                            if (!IS_ALPHA(ch))
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_METHOD);
                                goto error;
                            }

                            frame.method = (HttpMethod)0;
                            frame.index = 1;
                            switch (ch)
                            {
                                case 'A': frame.method = HttpMethod.ACL; break;
                                case 'B': frame.method = HttpMethod.BIND; break;
                                case 'C': frame.method = HttpMethod.CONNECT; /* or COPY, CHECKOUT */ break;
                                case 'D': frame.method = HttpMethod.DELETE; break;
                                case 'G': frame.method = HttpMethod.GET; break;
                                case 'H': frame.method = HttpMethod.HEAD; break;
                                case 'L': frame.method = HttpMethod.LOCK; /* or LINK */ break;
                                case 'M': frame.method = HttpMethod.MKCOL; /* or MOVE, MKACTIVITY, MERGE, M-SEARCH, MKCALENDAR */ break;
                                case 'N': frame.method = HttpMethod.NOTIFY; break;
                                case 'O': frame.method = HttpMethod.OPTIONS; break;
                                case 'P':
                                    frame.method = HttpMethod.POST;
                                    /* or PROPFIND|PROPPATCH|PUT|PATCH|PURGE */
                                    break;
                                case 'R': frame.method = HttpMethod.REPORT; /* or REBIND */ break;
                                case 'S': frame.method = HttpMethod.SUBSCRIBE; /* or SEARCH */ break;
                                case 'T': frame.method = HttpMethod.TRACE; break;
                                case 'U': frame.method = HttpMethod.UNLOCK; /* or UNSUBSCRIBE, UNBIND, UNLINK */ break;
                                default:
                                    SET_ERRNO(frame, HttpErrNO.HPE_INVALID_METHOD);
                                    goto error;
                            }
                            UPDATE_STATE(frame, State.s_req_method);
                            if (!RaiseMessageBegin(frame))
                            {
                                return RETURN(p, data);
                            }
                            //CALLBACK_NOTIFY(message_begin);

                            break;
                        }
                    case State.s_req_method:
                        {
                            if (ch == '\0')
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_METHOD);
                                goto error;
                            }

                            string matcher = method_strings[(byte)frame.method];
                            if (ch == ' ' && frame.index >= matcher.Length)
                            {
                                UPDATE_STATE(frame, State.s_req_spaces_before_url);
                            }
                            else if (ch == matcher[frame.index])
                            {
                                ; /* nada */
                            }
                            else if ((ch >= 'A' && ch <= 'Z') || ch == '-')
                            {
                                byte m = (byte)frame.method;
                                switch (((byte)frame.method) << 16 | frame.index << 8 | ch)
                                {
                                    case (((byte)HttpMethod.POST) << 16 | 1 << 8 | (byte)'U'):
                                        frame.method = HttpMethod.PUT;
                                        break;
                                    case (((byte)HttpMethod.POST) << 16 | 1 << 8 | (byte)'A'):
                                        frame.method = HttpMethod.PATCH;
                                        break;
                                    case (((byte)HttpMethod.POST) << 16 | 1 << 8 | (byte)'R'):
                                        frame.method = HttpMethod.PROPFIND;
                                        break;
                                    case (((byte)HttpMethod.PUT) << 16 | 2 << 8 | (byte)'R'):
                                        frame.method = HttpMethod.PURGE;
                                        break;

                                    case (((byte)HttpMethod.CONNECT) << 16 | 1 << 8 | (byte)'H'):
                                        frame.method = HttpMethod.CHECKOUT;
                                        break;
                                    case (((byte)HttpMethod.CONNECT) << 16 | 2 << 8 | (byte)'P'):
                                        frame.method = HttpMethod.COPY;
                                        break;


                                    case (((byte)HttpMethod.MKCOL) << 16 | 1 << 8 | (byte)'O'):
                                        frame.method = HttpMethod.MOVE;
                                        break;
                                    case (((byte)HttpMethod.MKCOL) << 16 | 1 << 8 | (byte)'E'):
                                        frame.method = HttpMethod.MERGE;
                                        break;
                                    case (((byte)HttpMethod.MKCOL) << 16 | 1 << 8 | (byte)'S'):
                                        frame.method = HttpMethod.MSEARCH;
                                        break;
                                    case (((byte)HttpMethod.MKCOL) << 16 | 2 << 8 | (byte)'A'):
                                        frame.method = HttpMethod.MKACTIVITY;
                                        break;
                                    case (((byte)HttpMethod.MKCOL) << 16 | 3 << 8 | (byte)'A'):
                                        frame.method = HttpMethod.MKCALENDAR;
                                        break;


                                    //XX(SUBSCRIBE, 1, 'E', SEARCH)
                                    case (((byte)HttpMethod.SUBSCRIBE) << 16 | 1 << 8 | (byte)'E'):
                                        frame.method = HttpMethod.SEARCH;
                                        break;
                                    //XX(REPORT, 2, 'B', REBIND)
                                    case (((byte)HttpMethod.REPORT) << 16 | 2 << 8 | (byte)'B'):
                                        frame.method = HttpMethod.REBIND;
                                        break;
                                    //XX(PROPFIND, 4, 'P', PROPPATCH)
                                    case (((byte)HttpMethod.PROPFIND) << 16 | 4 << 8 | (byte)'P'):
                                        frame.method = HttpMethod.PROPPATCH;
                                        break;
                                    //XX(LOCK, 1, 'I', LINK)
                                    case (((byte)HttpMethod.LOCK) << 16 | 1 << 8 | (byte)'I'):
                                        frame.method = HttpMethod.LINK;
                                        break;
                                    //XX(UNLOCK, 2, 'S', UNSUBSCRIBE)
                                    case (((byte)HttpMethod.UNLOCK) << 16 | 2 << 8 | (byte)'S'):
                                        frame.method = HttpMethod.UNSUBSCRIBE;
                                        break;
                                    //XX(UNLOCK, 2, 'B', UNBIND)
                                    case (((byte)HttpMethod.UNLOCK) << 16 | 2 << 8 | (byte)'B'):
                                        frame.method = HttpMethod.UNBIND;
                                        break;
                                    //XX(UNLOCK, 3, 'I', UNLINK)
                                    case (((byte)HttpMethod.UNLOCK) << 16 | 3 << 8 | (byte)'I'):
                                        frame.method = HttpMethod.UNLINK;
                                        break;

                                    default:
                                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_METHOD);
                                        goto error;
                                }
                            }
                            else
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_METHOD);
                                goto error;
                            }

                            ++frame.index;
                            break;
                        }

                    case State.s_req_spaces_before_url:
                        {
                            if (ch == ' ') break;
                            if (url_mark == -1)
                            {
                                url_mark = (Int32)(p - data) + offset;
                            }

                            if (frame.method == HttpMethod.CONNECT)
                            {
                                UPDATE_STATE(frame, State.s_req_server_start);
                            }

                            UPDATE_STATE(frame, parse_url_char(frame.state, ch));
                            if (frame.state == State.s_dead)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_URL);
                                goto error;
                            }

                            break;
                        }

                    case State.s_req_schema:
                    case State.s_req_schema_slash:
                    case State.s_req_schema_slash_slash:
                    case State.s_req_server_start:
                        {
                            switch (ch)
                            {
                                /* No whitespace allowed here */
                                case ' ':
                                case CR:
                                case LF:
                                    SET_ERRNO(frame, HttpErrNO.HPE_INVALID_URL);
                                    goto error;
                                default:
                                    UPDATE_STATE(frame, parse_url_char(frame.state, ch));
                                    if (frame.state == State.s_dead)
                                    {
                                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_URL);
                                        goto error;
                                    }
                                    break;
                            }

                            break;
                        }

                    case State.s_req_server:
                    case State.s_req_server_with_at:
                    case State.s_req_path:
                    case State.s_req_query_string_start:
                    case State.s_req_query_string:
                    case State.s_req_fragment_start:
                    case State.s_req_fragment:
                        {
                            switch (ch)
                            {
                                case ' ':
                                    UPDATE_STATE(frame, State.s_req_http_start);
                                    if (!RaiseUrl(frame,array, ref url_mark, (Int32)(p - data) + offset))
                                    {
                                        return RETURN(p, data);
                                    }
                                    //CALLBACK_DATA(url);
                                    break;
                                case CR:
                                case LF:
                                    frame.http_major = 0;
                                    frame.http_minor = 9;
                                    UPDATE_STATE(frame, (ch == CR) ?
                                      State.s_req_line_almost_done :
                                      State.s_header_field_start);
                                    if (!RaiseUrl(frame,array, ref url_mark, (Int32)(p - data) + offset))
                                    {
                                        return RETURN(p, data);
                                    }
                                    //CALLBACK_DATA(url);
                                    break;
                                default:
                                    UPDATE_STATE(frame, parse_url_char(frame.state, ch));
                                    if (frame.state == State.s_dead)
                                    {
                                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_URL);
                                        goto error;
                                    }
                                    break;
                            }
                            break;
                        }

                    case State.s_req_http_start:
                        switch (ch)
                        {
                            case 'H':
                                UPDATE_STATE(frame, State.s_req_http_H);
                                break;
                            case ' ':
                                break;
                            default:
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONSTANT);
                                goto error;
                        }
                        break;

                    case State.s_req_http_H:
                        STRICT_CHECK(frame, ch != 'T');
                        UPDATE_STATE(frame, State.s_req_http_HT);
                        break;

                    case State.s_req_http_HT:
                        STRICT_CHECK(frame, ch != 'T');
                        UPDATE_STATE(frame, State.s_req_http_HTT);
                        break;

                    case State.s_req_http_HTT:
                        STRICT_CHECK(frame, ch != 'P');
                        UPDATE_STATE(frame, State.s_req_http_HTTP);
                        break;

                    case State.s_req_http_HTTP:
                        STRICT_CHECK(frame, ch != '/');
                        UPDATE_STATE(frame, State.s_req_http_major);
                        break;

                    case State.s_req_http_major:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                            goto error;
                        }

                        frame.http_major = (byte)(ch - '0');
                        UPDATE_STATE(frame, State.s_req_http_dot);
                        break;

                    case State.s_req_http_dot:
                        {
                            if (ch != '.')
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                                goto error;
                            }

                            UPDATE_STATE(frame, State.s_req_http_minor);
                            break;
                        }

                    case State.s_req_http_minor:
                        if (!IS_NUM(ch))
                        {
                            SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                            goto error;
                        }

                        frame.http_minor = (byte)(ch - '0');
                        UPDATE_STATE(frame, State.s_req_http_end);
                        break;

                    case State.s_req_http_end:
                        {
                            if (ch == CR)
                            {
                                UPDATE_STATE(frame, State.s_req_line_almost_done);
                                break;
                            }

                            if (ch == LF)
                            {
                                UPDATE_STATE(frame, State.s_header_field_start);
                                break;
                            }

                            SET_ERRNO(frame, HttpErrNO.HPE_INVALID_VERSION);
                            goto error;
                            //break;
                        }

                    /* end of request line */
                    case State.s_req_line_almost_done:
                        {
                            if (ch != LF)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_LF_EXPECTED);
                                goto error;
                            }

                            UPDATE_STATE(frame, State.s_header_field_start);
                            break;
                        }

                    case State.s_header_field_start:
                        {
                            if (ch == CR)
                            {
                                UPDATE_STATE(frame, State.s_headers_almost_done);
                                break;
                            }

                            if (ch == LF)
                            {
                                /* they might be just sending \n instead of \r\n so this would be
                                 * the second \n to denote the end of headers*/
                                UPDATE_STATE(frame, State.s_headers_almost_done);
                                goto reexecute;
                            }

                            c = TOKEN(ch);

                            //todo:condition
                            if (c == 0)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_HEADER_TOKEN);
                                goto error;
                            }
                            if (header_field_mark ==-1)
                            {
                                header_field_mark = (Int32)(p-data)+offset;
                            }
                            //MARK(header_field);

                            frame.index = 0;
                            UPDATE_STATE(frame, State.s_header_field);

                            switch (c)
                            {
                                case 'c':
                                    frame.header_state = HeaderStates.h_C;
                                    break;

                                case 'p':
                                    frame.header_state = HeaderStates.h_matching_proxy_connection;
                                    break;

                                case 't':
                                    frame.header_state = HeaderStates.h_matching_transfer_encoding;
                                    break;

                                case 'u':
                                    frame.header_state = HeaderStates.h_matching_upgrade;
                                    break;

                                default:
                                    frame.header_state = HeaderStates.h_general;
                                    break;
                            }
                            break;
                        }

                    case State.s_header_field:
                        {
                            byte* start = p;
                            for (; p != data + count; p++)
                            {
                                ch = (char)*p;
                                c = TOKEN(ch);

                                if (c == 0)
                                    break;

                                switch (frame.header_state)
                                {
                                    case HeaderStates.h_general:
                                        break;

                                    case HeaderStates.h_C:
                                        frame.index++;
                                        frame.header_state = (c == 'o' ? HeaderStates.h_CO : HeaderStates.h_general);
                                        break;

                                    case HeaderStates.h_CO:
                                        frame.index++;
                                        frame.header_state = (c == 'n' ? HeaderStates.h_CON : HeaderStates.h_general);
                                        break;

                                    case HeaderStates.h_CON:
                                        frame.index++;
                                        switch (c)
                                        {
                                            case 'n':
                                                frame.header_state = HeaderStates.h_matching_connection;
                                                break;
                                            case 't':
                                                frame.header_state = HeaderStates.h_matching_content_length;
                                                break;
                                            default:
                                                frame.header_state = HeaderStates.h_general;
                                                break;
                                        }
                                        break;

                                    /* connection */

                                    case HeaderStates.h_matching_connection:
                                        frame.index++;
                                        if (frame.index > CONNECTION.Length - 1
                                            || c != CONNECTION[frame.index])
                                        {
                                            frame.header_state = HeaderStates.h_general;
                                        }
                                        else if (frame.index == CONNECTION.Length - 2)
                                        {
                                            frame.header_state = HeaderStates.h_connection;
                                        }
                                        break;

                                    /* proxy-connection */

                                    case HeaderStates.h_matching_proxy_connection:
                                        frame.index++;
                                        if (frame.index > PROXY_CONNECTION.Length - 1
                                            || c != PROXY_CONNECTION[frame.index])
                                        {
                                            frame.header_state = HeaderStates.h_general;
                                        }
                                        else if (frame.index == PROXY_CONNECTION.Length - 2)
                                        {
                                            frame.header_state = HeaderStates.h_connection;
                                        }
                                        break;

                                    /* content-length */

                                    case HeaderStates.h_matching_content_length:
                                        frame.index++;
                                        if (frame.index > CONTENT_LENGTH.Length - 1
                                            || c != CONTENT_LENGTH[frame.index])
                                        {
                                            frame.header_state = HeaderStates.h_general;
                                        }
                                        else if (frame.index == CONTENT_LENGTH.Length - 2)
                                        {
                                            frame.header_state = HeaderStates.h_content_length;
                                        }
                                        break;

                                    /* transfer-encoding */

                                    case HeaderStates.h_matching_transfer_encoding:
                                        frame.index++;
                                        if (frame.index > TRANSFER_ENCODING.Length - 1
                                            || c != TRANSFER_ENCODING[frame.index])
                                        {
                                            frame.header_state = HeaderStates.h_general;
                                        }
                                        else if (frame.index == TRANSFER_ENCODING.Length - 2)
                                        {
                                            frame.header_state = HeaderStates.h_transfer_encoding;
                                        }
                                        break;

                                    /* upgrade */

                                    case HeaderStates.h_matching_upgrade:
                                        frame.index++;
                                        if (frame.index > UPGRADE.Length - 1
                                            || c != UPGRADE[frame.index])
                                        {
                                            frame.header_state = HeaderStates.h_general;
                                        }
                                        else if (frame.index == UPGRADE.Length - 2)
                                        {
                                            frame.header_state = HeaderStates.h_upgrade;
                                        }
                                        break;

                                    case HeaderStates.h_connection:
                                    case HeaderStates.h_content_length:
                                    case HeaderStates.h_transfer_encoding:
                                    case HeaderStates.h_upgrade:
                                        if (ch != ' ') frame.header_state = HeaderStates.h_general;
                                        break;

                                    default:
                                        assert(false);//Unknown header_state
                                        break;
                                }
                            }

                            COUNT_HEADER_SIZE(frame, (UInt32)(p - start));

                            if (p == data + count)
                            {
                                --p;
                                break;
                            }

                            if (ch == ':')
                            {
                                UPDATE_STATE(frame, State.s_header_value_discard_ws);
                                if (!RaiseHeaderField(frame,array, ref header_field_mark, (Int32)(p - data) + offset))
                                {
                                    return RETURN(p, data);
                                }
                                //CALLBACK_DATA(header_field);
                                break;
                            }

                            SET_ERRNO(frame, HttpErrNO.HPE_INVALID_HEADER_TOKEN);
                            goto error;
                        }

                    case State.s_header_value_discard_ws:
                    case State.s_header_value_start:
                        {
                            if (frame.state == State.s_header_value_discard_ws)
                            {
                                if (ch == ' ' || ch == '\t') break;

                                if (ch == CR)
                                {
                                    UPDATE_STATE(frame, State.s_header_value_discard_ws_almost_done);
                                    break;
                                }

                                if (ch == LF)
                                {
                                    UPDATE_STATE(frame, State.s_header_value_discard_lws);
                                    break;
                                }
                            }
                            /* FALLTHROUGH */
                            if (header_value_mark == -1)
                            {
                                header_value_mark = (Int32)(p - data) + offset;
                            }
                            //MARK(header_value);

                            UPDATE_STATE(frame, State.s_header_value);
                            frame.index = 0;

                            c = LOWER(ch);

                            switch (frame.header_state)
                            {
                                case HeaderStates.h_upgrade:
                                    frame.flags |= Flags.F_UPGRADE;
                                    frame.header_state = HeaderStates.h_general;
                                    break;

                                case HeaderStates.h_transfer_encoding:
                                    /* looking for 'Transfer-Encoding: chunked' */
                                    if ('c' == c)
                                    {
                                        frame.header_state = HeaderStates.h_matching_transfer_encoding_chunked;
                                    }
                                    else
                                    {
                                        frame.header_state = HeaderStates.h_general;
                                    }
                                    break;

                                case HeaderStates.h_content_length:
                                    if (!IS_NUM(ch))
                                    {
                                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONTENT_LENGTH);
                                        goto error;
                                    }

                                    if ((frame.flags & Flags.F_CONTENTLENGTH) == Flags.F_CONTENTLENGTH)
                                    {
                                        SET_ERRNO(frame, HttpErrNO.HPE_UNEXPECTED_CONTENT_LENGTH);
                                        goto error;
                                    }

                                    frame.flags |= Flags.F_CONTENTLENGTH;
                                    frame.content_length = (byte)(ch - '0');
                                    break;

                                case HeaderStates.h_connection:
                                    /* looking for 'Connection: keep-alive' */
                                    if (c == 'k')
                                    {
                                        frame.header_state = HeaderStates.h_matching_connection_keep_alive;
                                        /* looking for 'Connection: close' */
                                    }
                                    else if (c == 'c')
                                    {
                                        frame.header_state = HeaderStates.h_matching_connection_close;
                                    }
                                    else if (c == 'u')
                                    {
                                        frame.header_state = HeaderStates.h_matching_connection_upgrade;
                                    }
                                    else
                                    {
                                        frame.header_state = HeaderStates.h_matching_connection_token;
                                    }
                                    break;

                                /* Multi-value `Connection` header */
                                case HeaderStates.h_matching_connection_token_start:
                                    break;

                                default:
                                    frame.header_state = HeaderStates.h_general;
                                    break;
                            }
                            break;
                        }

                    case State.s_header_value:
                        {
                            byte* start = p;
                            HeaderStates h_state = frame.header_state;
                            for (; p != data + count; p++)
                            {
                                ch = (char)*p;
                                if (ch == CR)
                                {
                                    UPDATE_STATE(frame, State.s_header_almost_done);
                                    frame.header_state = h_state;
                                    if (!RaiseHeaderValue(frame,array, ref header_value_mark, (Int32)(p - data) + offset))
                                    {
                                        return RETURN(p, data);
                                    }
                                    //CALLBACK_DATA(header_value);
                                    break;
                                }

                                if (ch == LF)
                                {
                                    UPDATE_STATE(frame, State.s_header_almost_done);
                                    COUNT_HEADER_SIZE(frame, (UInt32)(p - start));
                                    frame.header_state = h_state;
                                    //CALLBACK_DATA_NOADVANCE(header_value);
                                    if (!RaiseHeaderValue(frame,array, ref header_value_mark, (Int32)(p - data) + offset))
                                    {
                                        return RETURN(p, data);
                                    }
                                    goto reexecute;
                                }

                                if (!lenient && !IS_HEADER_CHAR(ch))
                                {
                                    SET_ERRNO(frame, HttpErrNO.HPE_INVALID_HEADER_TOKEN);
                                    goto error;
                                }

                                c = LOWER(ch);

                                switch (h_state)
                                {
                                    case HeaderStates.h_general:
                                        {
                                            byte* p_cr;
                                            byte* p_lf;
                                            Int32 limit = (Int32)(data + count - p);

                                            limit = Math.Min(limit, HTTP_MAX_HEADER_SIZE);

                                            p_cr = memchr(p, CR, limit);
                                            p_lf = memchr(p, LF, limit);
                                            if (p_cr != null)
                                            {
                                                if (p_lf != null && p_cr >= p_lf)
                                                    p = p_lf;
                                                else
                                                    p = p_cr;
                                            }
                                            else if (p_lf != null)
                                            {
                                                p = p_lf;
                                            }
                                            else
                                            {
                                                p = data + count;
                                            }
                                            --p;

                                            break;
                                        }

                                    case HeaderStates.h_connection:
                                    case HeaderStates.h_transfer_encoding:
                                        //assert(0 && "Shouldn't get here.");
                                        break;

                                    case HeaderStates.h_content_length:
                                        {
                                            UInt64 t;

                                            if (ch == ' ') break;

                                            if (!IS_NUM(ch))
                                            {
                                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONTENT_LENGTH);
                                                frame.header_state = h_state;
                                                goto error;
                                            }

                                            t = frame.content_length;


                                            t *= 10;


                                            t += (byte)(ch - '0');

                                            /* Overflow? Test against a conservative limit for simplicity. */
                                            if ((UInt64.MaxValue - 10) / 10 < frame.content_length)
                                            {
                                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONTENT_LENGTH);
                                                frame.header_state = h_state;
                                                goto error;
                                            }

                                            frame.content_length = t;
                                            break;
                                        }

                                    /* Transfer-Encoding: chunked */
                                    case HeaderStates.h_matching_transfer_encoding_chunked:
                                        frame.index++;
                                        if (frame.index > CHUNKED.Length - 1
                                            || c != CHUNKED[frame.index])
                                        {
                                            h_state = HeaderStates.h_general;
                                        }
                                        else if (frame.index == CHUNKED.Length - 2)
                                        {
                                            h_state = HeaderStates.h_transfer_encoding_chunked;
                                        }
                                        break;

                                    case HeaderStates.h_matching_connection_token_start:
                                        /* looking for 'Connection: keep-alive' */
                                        if (c == 'k')
                                        {
                                            h_state = HeaderStates.h_matching_connection_keep_alive;
                                            /* looking for 'Connection: close' */
                                        }
                                        else if (c == 'c')
                                        {
                                            h_state = HeaderStates.h_matching_connection_close;
                                        }
                                        else if (c == 'u')
                                        {
                                            h_state = HeaderStates.h_matching_connection_upgrade;
                                        }
                                        else if (STRICT_TOKEN(c))
                                        {
                                            h_state = HeaderStates.h_matching_connection_token;
                                        }
                                        else if (c == ' ' || c == '\t')
                                        {
                                            /* Skip lws */
                                        }
                                        else
                                        {
                                            h_state = HeaderStates.h_general;
                                        }
                                        break;

                                    /* looking for 'Connection: keep-alive' */
                                    case HeaderStates.h_matching_connection_keep_alive:
                                        frame.index++;
                                        if (frame.index > KEEP_ALIVE.Length - 1
                                            || c != KEEP_ALIVE[frame.index])
                                        {
                                            h_state = HeaderStates.h_matching_connection_token;
                                        }
                                        else if (frame.index == KEEP_ALIVE.Length - 2)
                                        {
                                            h_state = HeaderStates.h_connection_keep_alive;
                                        }
                                        break;

                                    /* looking for 'Connection: close' */
                                    case HeaderStates.h_matching_connection_close:
                                        frame.index++;
                                        if (frame.index > CLOSE.Length - 1 || c != CLOSE[frame.index])
                                        {
                                            h_state = HeaderStates.h_matching_connection_token;
                                        }
                                        else if (frame.index == CLOSE.Length - 2)
                                        {
                                            h_state = HeaderStates.h_connection_close;
                                        }
                                        break;

                                    /* looking for 'Connection: upgrade' */
                                    case HeaderStates.h_matching_connection_upgrade:
                                        frame.index++;
                                        if (frame.index > UPGRADE.Length - 1 ||
                                            c != UPGRADE[frame.index])
                                        {
                                            h_state = HeaderStates.h_matching_connection_token;
                                        }
                                        else if (frame.index == UPGRADE.Length - 2)
                                        {
                                            h_state = HeaderStates.h_connection_upgrade;
                                        }
                                        break;

                                    case HeaderStates.h_matching_connection_token:
                                        if (ch == ',')
                                        {
                                            h_state = HeaderStates.h_matching_connection_token_start;
                                            frame.index = 0;
                                        }
                                        break;

                                    case HeaderStates.h_transfer_encoding_chunked:
                                        if (ch != ' ') h_state = HeaderStates.h_general;
                                        break;

                                    case HeaderStates.h_connection_keep_alive:
                                    case HeaderStates.h_connection_close:
                                    case HeaderStates.h_connection_upgrade:
                                        if (ch == ',')
                                        {
                                            if (h_state == HeaderStates.h_connection_keep_alive)
                                            {
                                                frame.flags |= Flags.F_CONNECTION_KEEP_ALIVE;
                                            }
                                            else if (h_state == HeaderStates.h_connection_close)
                                            {
                                                frame.flags |= Flags.F_CONNECTION_CLOSE;
                                            }
                                            else if (h_state == HeaderStates.h_connection_upgrade)
                                            {
                                                frame.flags |= Flags.F_CONNECTION_UPGRADE;
                                            }
                                            h_state = HeaderStates.h_matching_connection_token_start;
                                            frame.index = 0;
                                        }
                                        else if (ch != ' ')
                                        {
                                            h_state = HeaderStates.h_matching_connection_token;
                                        }
                                        break;

                                    default:
                                        UPDATE_STATE(frame, State.s_header_value);
                                        h_state = HeaderStates.h_general;
                                        break;
                                }
                            }
                            frame.header_state = h_state;

                            COUNT_HEADER_SIZE(frame, (UInt32)(p - start));

                            if (p == data + count)
                                --p;
                            break;
                        }

                    case State.s_header_almost_done:
                        {
                            if (ch != LF)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_LF_EXPECTED);
                                goto error;
                            }

                            UPDATE_STATE(frame, State.s_header_value_lws);
                            break;
                        }

                    case State.s_header_value_lws:
                        {
                            if (ch == ' ' || ch == '\t')
                            {
                                UPDATE_STATE(frame, State.s_header_value_start);
                                goto reexecute;
                            }

                            /* finished the header */
                            switch (frame.header_state)
                            {
                                case HeaderStates.h_connection_keep_alive:
                                    frame.flags |= Flags.F_CONNECTION_KEEP_ALIVE;
                                    break;
                                case HeaderStates.h_connection_close:
                                    frame.flags |= Flags.F_CONNECTION_CLOSE;
                                    break;
                                case HeaderStates.h_transfer_encoding_chunked:
                                    frame.flags |= Flags.F_CHUNKED;
                                    break;
                                case HeaderStates.h_connection_upgrade:
                                    frame.flags |= Flags.F_CONNECTION_UPGRADE;
                                    break;
                                default:
                                    break;
                            }

                            UPDATE_STATE(frame, State.s_header_field_start);
                            goto reexecute;
                        }

                    case State.s_header_value_discard_ws_almost_done:
                        {
                            STRICT_CHECK(frame, ch != LF);
                            UPDATE_STATE(frame, State.s_header_value_discard_lws);
                            break;
                        }

                    case State.s_header_value_discard_lws:
                        {
                            if (ch == ' ' || ch == '\t')
                            {
                                UPDATE_STATE(frame, State.s_header_value_discard_ws);
                                break;
                            }
                            else
                            {
                                switch (frame.header_state)
                                {
                                    case HeaderStates.h_connection_keep_alive:
                                        frame.flags |= Flags.F_CONNECTION_KEEP_ALIVE;
                                        break;
                                    case HeaderStates.h_connection_close:
                                        frame.flags |= Flags.F_CONNECTION_CLOSE;
                                        break;
                                    case HeaderStates.h_connection_upgrade:
                                        frame.flags |= Flags.F_CONNECTION_UPGRADE;
                                        break;
                                    case HeaderStates.h_transfer_encoding_chunked:
                                        frame.flags |= Flags.F_CHUNKED;
                                        break;
                                    default:
                                        break;
                                }

                                /* header value was empty */
                                if (header_value_mark == -1)
                                {
                                    header_value_mark = (Int32)(p - data) + offset;
                                }
                                //MARK(header_value);
                                UPDATE_STATE(frame, State.s_header_field_start);
                                if (!RaiseHeaderValue(frame,array, ref header_value_mark, (Int32)(p - data) + offset))
                                {
                                    return RETURN(p, data);
                                }
                                //CALLBACK_DATA_NOADVANCE(header_value);
                                goto reexecute;
                            }
                        }

                    case State.s_headers_almost_done:
                        {
                            STRICT_CHECK(frame, ch != LF);

                            if ((frame.flags & Flags.F_TRAILING) == Flags.F_TRAILING)
                            {
                                /* End of a chunked request */
                                UPDATE_STATE(frame, State.s_message_done);
                                //CALLBACK_NOTIFY_NOADVANCE(chunk_complete);
                                if (!RaiseChunkComplete(frame))
                                {
                                    return RETURN(p, data);
                                }
                                goto reexecute;
                            }

                            /* Cannot use chunked encoding and a content-length header together
                               per the HTTP specification. */
                            if (((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED) &&
                                ((frame.flags & Flags.F_CONTENTLENGTH) == Flags.F_CONTENTLENGTH))
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_UNEXPECTED_CONTENT_LENGTH);
                                goto error;
                            }

                            UPDATE_STATE(frame, State.s_headers_done);

                            /* Set this here so that on_headers_complete() callbacks can see it */
                            if (((frame.flags & Flags.F_UPGRADE) == Flags.F_UPGRADE) &&
                                ((frame.flags & Flags.F_CONNECTION_UPGRADE) == Flags.F_CONNECTION_UPGRADE))
                            {
                                /* For responses, "Upgrade: foo" and "Connection: upgrade" are
                                 * mandatory only when it is a 101 Switching Protocols response,
                                 * otherwise it is purely informational, to announce support.
                                 */
                                frame.upgrade = (frame.type == HttpParserType.HTTP_REQUEST || frame.status_code == 101);
                            }
                            else
                            {
                                frame.upgrade = (frame.method == HttpMethod.CONNECT);
                            }

                            /* Here we call the headers_complete callback. This is somewhat
                             * different than other callbacks because if the user returns 1, we
                             * will interpret that as saying that this message has no body. This
                             * is needed for the annoying case of recieving a response to a HEAD
                             * request.
                             *
                             * We'd like to use CALLBACK_NOTIFY_NOADVANCE() here but we cannot, so
                             * we have to simulate it by handling a change in errno below.
                             */

                            Int32 ret = callback.on_headers_complete(frame);
                            switch (ret)
                            {
                                case 0:
                                    break;

                                case 2:
                                case 1:
                                    if (ret == 2)
                                    {
                                        frame.upgrade = true;
                                    }
                                    /* FALLTHROUGH */
                                    frame.flags |= Flags.F_SKIPBODY;
                                    break;

                                default:
                                    SET_ERRNO(frame, HttpErrNO.HPE_CB_headers_complete);
                                    //RETURN(p - data); /* Error */
                                    return RETURN(p, data);
                            }



                            //if (HTTP_PARSER_ERRNO(parser) != HPE_OK)
                            if (frame.http_errno != HttpErrNO.HPE_OK)
                            {
                                //RETURN(p - data);
                                return RETURN(p, data);
                            }
                            goto reexecute;
                            //REEXECUTE();
                        }

                    case State.s_headers_done:
                        {
                            bool hasBody;
                            STRICT_CHECK(frame, ch != LF);

                            frame.nread = 0;

                            hasBody = ((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED) ||
                              (frame.content_length > 0 && frame.content_length != UInt64.MaxValue);
                            if (frame.upgrade && (frame.method == HttpMethod.CONNECT ||
                                                    ((frame.flags & Flags.F_SKIPBODY) == Flags.F_SKIPBODY) || !hasBody))
                            {
                                /* Exit, the rest of the message is in a different protocol. */
                                UPDATE_STATE(frame, NEW_MESSAGE(frame));
                                if (!RaiseMessageComplete(frame))
                                {
                                    return RETURN(p, data);
                                }
                                //CALLBACK_NOTIFY(message_complete);
                                //RETURN((p - data) + 1);
                                return RETURN(p, data);
                            }

                            if ((frame.flags & Flags.F_SKIPBODY) == Flags.F_SKIPBODY)
                            {
                                UPDATE_STATE(frame, NEW_MESSAGE(frame));
                                if (!RaiseMessageComplete(frame))
                                {
                                    return RETURN(p, data);
                                }
                                //CALLBACK_NOTIFY(message_complete);
                            }
                            else if ((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED)
                            {
                                /* chunked encoding - ignore Content-Length header */
                                UPDATE_STATE(frame, State.s_chunk_size_start);
                            }
                            else
                            {
                                if (frame.content_length == 0)
                                {
                                    /* Content-Length header given but zero: Content-Length: 0\r\n */
                                    UPDATE_STATE(frame, NEW_MESSAGE(frame));
                                    if (!RaiseMessageComplete(frame))
                                    {
                                        return RETURN(p, data);
                                    }
                                    //CALLBACK_NOTIFY(message_complete);
                                }
                                else if (frame.content_length != UInt64.MaxValue)
                                {
                                    /* Content-Length header given and non-zero */
                                    UPDATE_STATE(frame, State.s_body_identity);
                                }
                                else
                                {
                                    if (!http_message_needs_eof(frame))
                                    {
                                        /* Assume content-length 0 - read the next */
                                        UPDATE_STATE(frame, NEW_MESSAGE(frame));
                                        if (!RaiseMessageComplete(frame))
                                        {
                                            return RETURN(p, data);
                                        }
                                        //CALLBACK_NOTIFY(message_complete);
                                    }
                                    else
                                    {
                                        /* Read body until EOF */
                                        UPDATE_STATE(frame, State.s_body_identity_eof);
                                    }
                                }
                            }

                            break;
                        }

                    case State.s_body_identity:
                        {
                            UInt64 to_read = Math.Min(frame.content_length, (UInt64)((data + count) - p));

                            assert(frame.content_length != 0
                                && frame.content_length != UInt64.MaxValue);

                            /* The difference between advancing content_length and p is because
                             * the latter will automaticaly advance on the next loop iteration.
                             * Further, if content_length ends up at 0, we want to see the last
                             * byte again for our message complete callback.
                             */
                            if (body_mark == -1)
                            {
                                body_mark = (Int32)(p - data) + offset;
                            }
                            //MARK(body);
                            frame.content_length -= to_read;
                            p += to_read - 1;

                            if (frame.content_length == 0)
                            {
                                UPDATE_STATE(frame, State.s_message_done);

                                /* Mimic CALLBACK_DATA_NOADVANCE() but with one extra byte.
                                 *
                                 * The alternative to doing this is to wait for the next byte to
                                 * trigger the data callback, just as in every other case. The
                                 * problem with this is that this makes it difficult for the test
                                 * harness to distinguish between complete-on-EOF and
                                 * complete-on-length. It's not clear that this distinction is
                                 * important for applications, but let's keep it for now.
                                 */
                                //CALLBACK_DATA_(body, p - body_mark + 1, p - data);
                                if (!RaiseBody(frame,array, ref body_mark, (Int32)(p - data) + offset))
                                {
                                    return RETURN(p, data);
                                }
                                goto reexecute;
                            }

                            break;
                        }

                    /* read until EOF */
                    case State.s_body_identity_eof:
                        if (body_mark == -1)
                        {
                            body_mark = (Int32)(p - data) + offset;
                        }
                        //MARK(body);
                        p = data + count - 1;

                        break;

                    case State.s_message_done:
                        UPDATE_STATE(frame, NEW_MESSAGE(frame));
                        //CALLBACK_NOTIFY(message_complete);
                        if (!RaiseMessageComplete(frame))
                        {
                            return RETURN(p, data);
                        }
                        if (frame.upgrade)
                        {
                            /* todo:Exit, the rest of the message is in a different protocol. */
                            //return RETURN((p - data) + 1);
                            return RETURN(p, data);
                        }
                        break;

                    case State.s_chunk_size_start:
                        {
                            assert(frame.nread == 1);
                            assert((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED);

                            unhex_val = unhex[(byte)ch];
                            if (unhex_val == -1)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CHUNK_SIZE);
                                goto error;
                            }

                            frame.content_length = (byte)unhex_val;
                            UPDATE_STATE(frame, State.s_chunk_size);
                            break;
                        }

                    case State.s_chunk_size:
                        {
                            UInt64 t;

                            //assert(frame.flags & flags.F_CHUNKED);

                            if (ch == CR)
                            {
                                UPDATE_STATE(frame, State.s_chunk_size_almost_done);
                                break;
                            }

                            unhex_val = unhex[(byte)ch];

                            if (unhex_val == -1)
                            {
                                if (ch == ';' || ch == ' ')
                                {
                                    UPDATE_STATE(frame, State.s_chunk_parameters);
                                    break;
                                }

                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CHUNK_SIZE);
                                goto error;
                            }

                            t = frame.content_length;
                            t *= 16;
                            t += (byte)unhex_val;

                            /* Overflow? Test against a conservative limit for simplicity. */
                            if ((Int64.MaxValue - 16) / 16 < frame.content_length)
                            {
                                SET_ERRNO(frame, HttpErrNO.HPE_INVALID_CONTENT_LENGTH);
                                goto error;
                            }

                            frame.content_length = t;
                            break;
                        }

                    case State.s_chunk_parameters:
                        {
                            assert((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED);

                            /* just ignore this shit. TODO check for overflow */
                            if (ch == CR)
                            {
                                UPDATE_STATE(frame, State.s_chunk_size_almost_done);
                                break;
                            }
                            break;
                        }

                    case State.s_chunk_size_almost_done:
                        {
                            assert((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED);
                            STRICT_CHECK(frame, ch != LF);

                            frame.nread = 0;

                            if (frame.content_length == 0)
                            {
                                frame.flags |= Flags.F_TRAILING;
                                UPDATE_STATE(frame, State.s_header_field_start);
                            }
                            else
                            {
                                UPDATE_STATE(frame, State.s_chunk_data);
                            }
                            if (!RaiseChunkHeader(frame))
                            {
                                return RETURN(p, data);
                            }

                            break;
                        }

                    case State.s_chunk_data:
                        {
                            UInt64 to_read = Math.Min(frame.content_length,
                                                   (UInt64)((data + count) - p));

                            assert((frame.flags & Flags.F_CHUNKED) == Flags.F_CHUNKED);
                            assert(frame.content_length != 0
                                && frame.content_length != UInt64.MaxValue);

                            /* See the explanation in s_body_identity for why the content
                             * length and data pointers are managed this way.
                             */
                            if (body_mark == -1)
                            {
                                body_mark = (Int32)(p-data)+offset;
                            }
                            //MARK(body);
                            frame.content_length -= to_read;
                            p += to_read - 1;

                            if (frame.content_length == 0)
                            {
                                UPDATE_STATE(frame, State.s_chunk_data_almost_done);
                            }

                            break;
                        }

                    case State.s_chunk_data_almost_done:
                        //assert(frame.flags & F_CHUNKED);
                        //assert(frame.content_length == 0);
                        STRICT_CHECK(frame, ch != CR);
                        UPDATE_STATE(frame, State.s_chunk_data_done);
                        if (!RaiseBody(frame,array, ref body_mark, (Int32)(p - data) + offset))
                        {
                            return RETURN(p, data);
                        }
                        //CALLBACK_DATA(body);
                        break;

                    case State.s_chunk_data_done:
                        //assert(frame.flags & F_CHUNKED);
                        STRICT_CHECK(frame, ch != LF);
                        frame.nread = 0;
                        UPDATE_STATE(frame, State.s_chunk_size_start);
                        if (!RaiseChunkComplete(frame))
                        {
                            return RETURN(p, data);
                        }
                        //CALLBACK_NOTIFY(chunk_complete);
                        break;

                    default:
                        //assert(0 && "unhandled state");
                        SET_ERRNO(frame, HttpErrNO.HPE_INVALID_INTERNAL_STATE);
                        goto error;


                }//switch end



            }//loop end



            //System.Diagnostics.Debug.Assert(((header_field_mark != null ? 1 : 0) + (header_value_mark != null ? 1 : 0) + (url_mark != null ? 1 : 0) + (body_mark != null ? 1 : 0) + (status_mark != null ? 1 : 0)) <= 1);


            //CALLBACK_DATA_NOADVANCE(header_field);
            RaiseHeaderField(frame,array, ref header_field_mark, (Int32)(p - data) + offset);
            //CALLBACK_DATA_NOADVANCE(header_value);
            RaiseHeaderValue(frame,array, ref header_value_mark, (Int32)(p - data) + offset);
            //CALLBACK_DATA_NOADVANCE(url);
            RaiseUrl(frame,array, ref url_mark, (Int32)(p - data) + offset);
            //CALLBACK_DATA_NOADVANCE(body);
            RaiseBody(frame,array, ref body_mark, (Int32)(p - data) + offset);
            //CALLBACK_DATA_NOADVANCE(status);
            RaiseStatus(frame,array, ref status_mark, (Int32)(p - data) + offset);

            return count;


            error:
            if (frame.http_errno == HttpErrNO.HPE_OK)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_UNKNOWN);
            }
            return RETURN(p, data);
        }
        

        #region events

        private bool RaiseMessageBegin(HttpFrame frame)
        {
            if (callback.on_message_begin(frame) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_message_begin);
                return false;
            }

            return true;
        }

        private unsafe bool RaiseStatus(HttpFrame frame, byte[] array, ref Int32 start, Int32 end)
        {
            if (start == -1)
            {
                return false;
            }
            
            if (callback.on_status(frame, frame.status_code,  new ArraySegment<byte>(array, start, end - start)) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_status);
                return false;
            }
            start = -1;
            return true;
        }

        private unsafe bool RaiseUrl(HttpFrame frame, byte[] array, ref Int32 start, Int32 end)
        {
            if (start == -1)
            {
                return false;
            }
            
            if (callback.on_uri(frame, new ArraySegment<byte>(array, start, end - start)) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_url);
                return false;
            }
            start = -1;
            return true;
        }



        private unsafe bool RaiseHeaderField(HttpFrame frame,byte[] array,ref Int32 start,Int32 end)
        {
            if (start == -1)
            {
                return false;
            }
            
            if (callback.on_header_field(frame,  new ArraySegment<byte>(array, start, end - start)) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_header_field);
                return false;
            }
            start = -1;
            return true;
        }

        private unsafe bool RaiseHeaderValue(HttpFrame frame, byte[] array, ref Int32 start, Int32 end)
        {
            if (start == -1)
            {
                return false;
            }
            

            if (callback.on_header_value(frame, new ArraySegment<byte>(array, start, end - start)) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_header_value);
                return false;
            }
            start = -1;
            return true;
        }

        private unsafe bool RaiseBody(HttpFrame frame, byte[] array, ref Int32 start, Int32 end)
        {
            if (start == -1)
            {
                return false;
            }
            if (callback.on_body(frame, new ArraySegment<byte>(array,start,end-start)) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_body);
                return false;
            }
            start = -1;
            return true;
        }

        private bool RaiseMessageComplete(HttpFrame frame)
        {
            if (callback.on_message_complete(frame) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_message_complete);
                return false;
            }

            return true;
        }

        /* When on_chunk_header is called, the current chunk length is stored
         * in frame.content_length.
         */
        private bool RaiseChunkHeader(HttpFrame frame)
        {
            if (callback.on_chunk_header(frame) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_chunk_header);
                return false;
            }

            return true;
        }

        private bool RaiseChunkComplete(HttpFrame frame)
        {
            if (callback.on_chunk_complete(frame) != 0)
            {
                SET_ERRNO(frame, HttpErrNO.HPE_CB_chunk_complete);
                return false;
            }

            return true;
        }
        #endregion


    }
}
