using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZTImage.Collections
{
    

    public class MutilList<T1, T2>:List<MutilObject<T1,T2>>
    {
        public void Add(T1 item1, T2 item2)
        {
            this.Add(new MutilObject<T1, T2>(item1, item2));
        }
    }


    public class MutilList<T1, T2, T3>:List<MutilObject<T1,T2,T3>>
    {
        public void Add(T1 item1, T2 item2, T3 item3)
        {
            this.Add(new MutilObject<T1, T2, T3>(item1, item2, item3));
        }
    }

    public class MutilList<T1, T2, T3, T4>:List<MutilObject<T1,T2,T3,T4>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Add(new MutilObject<T1, T2, T3, T4>(item1, item2, item3, item4));
        }
    }


    public class MutilList<T1,T2,T3,T4,T5>:List<MutilObject<T1,T2,T3,T4,T5>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            this.Add(new MutilObject<T1, T2, T3, T4, T5>(item1,item2,item3,item4,item5));
        }
    }


}
