using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompresorArchivos
{
    public class HuffmanNode<T>
    {
        public T? Element { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode<T> Left { get; set; }
        public HuffmanNode<T> Right { get; set; }

        public HuffmanNode(T? e, int freq)
        {
            Element = e;
            Frequency = freq;
            Left = Right = null;
        }

        public HuffmanNode(T? e, int freq, HuffmanNode<T> left, HuffmanNode<T> right)
        {
            Element = e;
            Frequency = freq;
            Left = left;
            Right = right;
        }

        public bool EsHoja() => Left == null && Right == null;
        public int CompareTo(HuffmanNode<T> node) => Frequency.CompareTo(node.Frequency);
    }
}
