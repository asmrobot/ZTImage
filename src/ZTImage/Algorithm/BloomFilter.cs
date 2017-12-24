using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.Algorithm
{
    public class BloomFilter
    {
        /* BitSet初始分配2^30个bit */
        private static readonly int DEFAULT_SIZE = 1 << 30;

        /* 不同哈希函数的种子，一般应取质数 */
        private static readonly int[] SEEDS = new int[] { 5, 7, 11, 13, 31, 37, 61 };

        public BloomFilter()
        {
            this.mSize = DEFAULT_SIZE;
            this.mSeeds = SEEDS;
            init();
        }

        public BloomFilter(int size)
        {
            this.mSize = size;
            this.mSeeds = SEEDS;

            init();
        }


        public BloomFilter(int[] seeds)
        {
            this.mSize = DEFAULT_SIZE;
            this.mSeeds = seeds;
            init();
        }
        public BloomFilter(int size, int[] seeds)
        {
            this.mSize = size;
            this.mSeeds = seeds;
            init();
        }
        
        private int mSize;
        private int[] mSeeds;

        private BitArray bits;//冲突向量空间
        private SimpleHash[] func;/* 哈希函数对象 */

        private void init()
        {
            if (mSize < 2 || (mSize%2!=0))
            {
                throw new ArgumentException("size参数不正确");
            }
            if (this.mSeeds == null || this.mSeeds.Length <= 0)
            {
                throw new ArgumentException("seeds参数不正确");
            }
            this.bits= new BitArray(this.mSize, false);
            
        
            this.func = new SimpleHash[this.mSeeds.Length];

            for (int i = 0; i < this.mSeeds.Length; i++)
            {
                func[i] = new SimpleHash(this.mSize, this.mSeeds[i]);
            }
        }

        /// <summary>
        /// 将字符串标记到bits中  
        /// </summary>
        /// <param name="value"></param>
        public void Add(String value)
        {
            foreach (SimpleHash f in func)
            {
                bits.Set(f.hash(value), true);
            }
        }

        /// <summary>
        /// 判断字符串是否已经被bits标记  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(String value)
        {
            if (value == null)
            {
                return false;
            }

            bool ret = true;
            foreach (SimpleHash f in func)
            {
                ret = ret && bits.Get(f.hash(value));
                if (!ret)
                {
                    break;
                }
            }

            return ret;
        }

        /* 哈希函数类 */
        public  class SimpleHash
        {
            private int cap;
            private int seed;

            public SimpleHash(int capacity, int seed)
            {
                this.cap = capacity;
                this.seed = seed;
            }

            // hash函数，采用简单的加权和hash  
            public int hash(String value)
            {
                int result = 0;
                int len = value.Length;
                for (int i = 0; i < len; i++)
                {
                    result = seed * result + value[i];
                }
                return (cap - 1) & result;
            }
        }
    }
}
