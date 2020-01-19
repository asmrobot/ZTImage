using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Collections.Tree
{
    /// <summary>
    /// 节点类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ZTNode<T>:IEnumerable<ZTNode<T>>,IEnumerator<ZTNode<T>> where T : class
    {
        public ZTNode()
        {
            this.Value = null;
            this.ID = null;

            this.Parent = null;
            this.Childs = new List<ZTNode<T>>();
        }


        public ZTNode(T node,string id)
        {
            this.Value = node;            
            this.ID = id;
            
            this.Parent = null;
            this.m_NextSbiling = null;
            this.m_PreSbiling = null;
            this.Childs = new List<ZTNode<T>>();
        }

        #region Fields

        /// <summary>
        /// 当前结点的值
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        /// 当前深度
        /// </summary>
        public int Deep
        {
            get
            {
                int deep = 0;
                ZTNode<T> self = this;
                while (self.Parent!=null )
                {
                    self = self.Parent;
                    deep++;
                }
                return deep;
            }
        }

        /// <summary>
        /// 标识
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 父结点
        /// </summary>
        public ZTNode<T> Parent
        {
            get;
            set;
        }        

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ZTNode<T>> Childs
        {
            get;
            set;
        }

        private ZTNode<T> m_NextSbiling;
        /// <summary>
        /// 下个兄弟节点
        /// </summary>
        /// <returns></returns>
        public ZTNode<T> NextSbiling
        {
            get
            {
                return m_NextSbiling;
            }
        }


        private ZTNode<T> m_PreSbiling;
        /// <summary>
        /// 上一个兄弟节点
        /// </summary>
        public ZTNode<T> PreSbiling
        {
            get
            {
                return m_PreSbiling;
            }
        }

        /// <summary>
        /// 首节点
        /// </summary>
        public ZTNode<T> First
        {
            get {
                if (Childs.Count > 0)
                {
                    return Childs[0];
                }
                return null;
            }
        }

        /// <summary>
        /// 最后一个节点
        /// </summary>
        public ZTNode<T> Last
        {
            get
            {
                if (Childs.Count > 0)
                { 
                    return Childs [Childs .Count -1];
                }
                return null;
            }
        }
        #endregion


        #region Methods

        /// <summary>
        /// 通过ID得到节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ZTNode<T> GetNodeByID(string id)
        {
            if (this.ID == id)
            {
                return this;
            }
            for (int i = 0; i < this.Childs.Count; i++)
            {
                ZTNode<T> temp=this.Childs[i].GetNodeByID(id);
                if (temp != null)
                {
                    return temp;
                }                
            }
            return null; 
        }

        /// <summary>
        /// 是否包含某节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsID(string id)
        {
            if (GetNodeByID(id) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ZTNode<T> AppendChild(T node, string id)
        {
            ZTNode<T> leaf = new ZTNode<T>(node, id);
            leaf.Parent = this;
            
            if (this.Childs.Count > 0)
            {
                leaf.m_PreSbiling = this.Childs[this.Childs.Count - 1];
                leaf.m_PreSbiling.m_NextSbiling = leaf;
            }
            
            this.Childs.Add(leaf);

            return leaf;
        }

        /// <summary>
        /// 在此节点前添加一个节点
        /// </summary>
        /// <param name="index">从0开始</param>
        /// <param name="node"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ZTNode<T> InsertBefore(int index,T node, string id)
        {
            if (index < 0 || index >= this.Childs.Count)
            {
                return this.AppendChild(node, id);
            }

            ZTNode<T> leaf = new ZTNode<T>(node, id);
            leaf.Parent = this;

            ZTNode<T> temp= this.Childs[index];
            leaf.m_PreSbiling = temp.m_PreSbiling;
            if (leaf.m_PreSbiling != null)
            {
                leaf.m_PreSbiling.m_NextSbiling = leaf;
            }

            leaf.m_NextSbiling = temp;
            leaf.m_NextSbiling.m_PreSbiling = leaf;


            this.Childs.Insert(index, leaf);

            return leaf;
        }

        

        /// <summary>
        /// 从树中移除一个叶子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveChild(string id)
        {
            for (int i = 0; i < this.Childs.Count; i++)
            {
                if (this.Childs[i].ID == id)
                {
                    ZTNode<T> ztnode = this.Childs[i];

                    if (ztnode.m_PreSbiling != null)
                    {
                        ztnode.m_PreSbiling.m_NextSbiling = ztnode.m_NextSbiling;
                    }

                    if (ztnode.m_NextSbiling != null)
                    {
                        ztnode.m_NextSbiling.m_PreSbiling = ztnode.m_PreSbiling;
                    }
                    this.Childs.RemoveAt(i);
                }
            }
            
            return true;
        }

        /// <summary>
        /// 从树中移除一个叶子节点
        /// </summary>
        /// <param name="node"></param>
        public bool RemoveChild(T node)
        {
            for (int i = 0; i < this.Childs.Count; i++)
            {
                if (this.Childs[i].Value ==node )
                {
                    ZTNode<T> ztnode = this.Childs[i];

                    if (ztnode.m_PreSbiling != null)
                    {
                        ztnode.m_PreSbiling.m_NextSbiling = ztnode.m_NextSbiling;
                    }

                    if (ztnode.m_NextSbiling != null)
                    {
                        ztnode.m_NextSbiling.m_PreSbiling = ztnode.m_PreSbiling;
                    }
                    this.Childs.RemoveAt(i);
                }
            }
            return false;
        }


        

        #endregion



        #region IEnumerable
        public IEnumerator<ZTNode<T>> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }
        #endregion


        #region IEnumerator
        private Stack<ZTNode<T>> _queue = new Stack<ZTNode<T>>();

        private ZTNode<T> _current;
        public ZTNode<T> Current
        {
            get 
            {
                if (_current != null)
                {
                    return _current;
                }
                return null;
            }
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get {
                if (_current != null)
                {
                    return _current;
                }
                return null;
            }
        }

        public bool MoveNext()
        {
            if (_current == null)
            {
                _current = this;
                return true;
            }

            if (_current.Childs.Count > 0)
            {
                _queue.Push(_current);
                _current = _current.First;
            }
            else
            {
                _current = _current.NextSbiling;
                while (_current == null)
                {
                    if (_queue.Count <= 0)
                    {
                        return false;
                    }
                    _current = _queue.Pop();
                    if (_current.Parent==null || _current.ID == this.ID)
                    {
                        return false;
                    }

                    _current = _current.NextSbiling;
                }
            }
            return true;
        }

        public void Reset()
        {
            _current = null;
        }
        #endregion
        
    }
}
