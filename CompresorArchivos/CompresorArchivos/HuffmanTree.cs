using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompresorArchivos
{
    public class HuffmanTree<T>
    {
        public HuffmanNode<T> Root;

        public HuffmanTree() { Root = null; }

        public List<T> PreOrder()
        {
            var temp = new List<T>();
            PreOrder(Root, temp);
            return temp;
        }

        private void PreOrder(HuffmanNode<T> act, List<T> list)
        {
            if (act != null)
            {
                list.Add(act.Element);
                PreOrder(act.Left, list);
                PreOrder(act.Right, list);
            }
        }
    }
}
