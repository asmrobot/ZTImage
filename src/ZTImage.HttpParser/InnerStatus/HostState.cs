using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.HttpParser
{
    public enum HostState:byte
    {
        s_http_host_dead = 1
      , s_http_userinfo_start
      , s_http_userinfo
      , s_http_host_start
      , s_http_host_v6_start
      , s_http_host
      , s_http_host_v6
      , s_http_host_v6_end
      , s_http_host_v6_zone_start
      , s_http_host_v6_zone
      , s_http_host_port_start
      , s_http_host_port
    }
}
