using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Collections.Tree
{
    /// <summary>
    /// 树结构
    /// 删除叶子节点未完成
    /// 遍历未完成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KubiuTree<T>:KubiuNode<T> where T:class
    {
        public KubiuTree(T root,string id):base(root,id)
        { }

        public KubiuTree():base(default(T),"0")
        { }
    }
}
