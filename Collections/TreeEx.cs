using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTImage.Reflection;

namespace ZTImage.Collections
{
    public class TreeEx<T>  where T : class,IComparable<T>
    {
        public TreeEx()
        {
            this.Value = null;
        }


        private TreeEx(T val)
        {
            this.Value = val;
        }

        public T Value { get; private set; }

        [UnSerialized]
        public int Deep { get; set; }
        
        [UnSerialized]
        public TreeEx<T> Parent { get; set; }

        public List<TreeEx<T>> Childrens { get; set; }

        #region Methods

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="child"></param>
        public TreeEx<T> AddChild(T child)
        {
            var surezen = new TreeEx<T>(child) { Deep = this.Deep + 1 };
            if (this.Childrens == null)
            {
                this.Childrens = new List<TreeEx<T>>() { surezen};
            }
            else
            {   
                //排序插入
                bool isInsert = false;
                for (int i = 0; i < this.Childrens.Count; i++)
                {
                    if(child.CompareTo(this.Childrens[i].Value)>=0)
                    {
                        this.Childrens.Insert(i, surezen);
                        isInsert = true;
                        break;
                    }
                }
                if (!isInsert)
                {
                    this.Childrens.Add(surezen);
                }
            }
            return surezen;
        }

        /// <summary>
        /// 查找指定元素
        /// </summary>
        /// <param name="itemSelector"></param>
        /// <returns></returns>
        public TreeEx<T> Find(Func<T, bool> itemSelector)
        {
            if (itemSelector == null )
            {
                throw new ArgumentNullException("项选择器不能为空");
            }

            if (this.Value!=null && itemSelector(this.Value))
            {
                return this;
            }
            if (this.Childrens != null && this.Childrens.Count > 0)
            {
                for (int i = 0; i < this.Childrens.Count; i++)
                {
                    var item = this.Childrens[i].Find(itemSelector);
                    if (item!= null)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 过滤不符合条件的元素
        /// </summary>
        /// <param name="itemSelector"></param>
        public void Filter(Func<T, bool> itemSelector)
        {
            if (itemSelector == null)
            {
                throw new ArgumentNullException("项选择器不能为空");
            }

            if (this.Childrens != null && this.Childrens.Count > 0)
            {
                for (int i = 0; i < this.Childrens.Count; i++)
                {
                    if (!itemSelector(this.Childrens[i].Value))
                    {
                        this.Childrens.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        this.Childrens[i].Filter(itemSelector);
                    }
                }
            }
            
        }

        /// <summary>
        /// 从列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static TreeEx<T> FromList(IList<T> list,Func<T,string> keySelector,Func<T,string> parentKeySelector)
        {
            TreeEx<T> tree = new TreeEx<T>();
            if (list == null || list.Count <= 0)
            {
                return tree;
            }
            if (keySelector == null || parentKeySelector == null)
            {
                throw new ArgumentNullException("KEY选择器和父键选择器不能为空");
            }


            FromListIterator(tree,list, keySelector, parentKeySelector);
            return tree;
        }

        private static void FromListIterator(TreeEx<T> node,IList<T> list,Func<T,string> keySelector, Func<T, string> parentKeySelector)
        {
            string parentKey = keySelector(node.Value);
            for (int i = 0; i < list.Count; i++)
            {
                if(parentKeySelector(list[i])==parentKey)
                {
                    var subNode=node.AddChild(list[i]);
                    FromListIterator(subNode, list, keySelector, parentKeySelector);
                }
            }
        }
        #endregion


        #region IEnumerable
        public IEnumerable<TreeEx<T>> GetIterator(bool containsSelf)
        {
            if (containsSelf)
            {
                yield return this;
            }
            if (this.Childrens == null || this.Childrens.Count <= 0)
            {
                yield break;
            }

            for (int i = 0; i < this.Childrens.Count;i++ )
            {
                yield return this.Childrens[i];
                foreach (var item in this.Childrens[i].GetIterator(false))
                {
                    yield return item;                    
                }
            }
        }
        #endregion
    }
}
