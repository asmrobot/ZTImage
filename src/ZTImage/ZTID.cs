using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage
{
    public class ZTID:IdWorker
    {
        private ZTID() : base(1, 1)
        { }



        private static ZTID _Instance = null;
        private static object _Locker = new object();
        public static ZTID Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_Locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new ZTID();
                        }
                    }
                }
                return _Instance;
            }
        }
    }
}
