﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZTImage.Collections
{
    

    public class MultiList<T1, T2>:List<MultiObject<T1,T2>>
    {
        public void Add(T1 item1, T2 item2)
        {
            this.Add(new MultiObject<T1, T2>(item1, item2));
        }
    }


    public class MultiList<T1, T2, T3>:List<MultiObject<T1,T2,T3>>
    {
        public void Add(T1 item1, T2 item2, T3 item3)
        {
            this.Add(new MultiObject<T1, T2, T3>(item1, item2, item3));
        }
    }

    public class MultiList<T1, T2, T3, T4>:List<MultiObject<T1,T2,T3,T4>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Add(new MultiObject<T1, T2, T3, T4>(item1, item2, item3, item4));
        }
    }


    public class MultiList<T1,T2,T3,T4,T5>:List<MultiObject<T1,T2,T3,T4,T5>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            this.Add(new MultiObject<T1, T2, T3, T4, T5>(item1,item2,item3,item4,item5));
        }
    }


    public class MultiList<T1, T2, T3, T4, T5,T6> : List<MultiObject<T1, T2, T3, T4, T5, T6>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5,T6 item6)
        {
            this.Add(new MultiObject<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5,item6));
        }
    }


    public class MultiList<T1, T2, T3, T4, T5, T6, T7> : List<MultiObject<T1, T2, T3, T4, T5, T6, T7>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5,T6 item6,T7 item7)
        {
            this.Add(new MultiObject<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5,item6,item7));
        }
    }


    public class MultiList<T1, T2, T3, T4, T5, T6, T7, T8> : List<MultiObject<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5,T6 item6,T7 item7,T8 item8)
        {
            this.Add(new MultiObject<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5,item6,item7,item8));
        }
    }


}
