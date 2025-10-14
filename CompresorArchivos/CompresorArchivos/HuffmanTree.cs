using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompresorArchivos
{
    public class HuffmanTree<T>
    {
        public HuffmanNode<T>? Root;

        public HuffmanTree() { Root = null; }

        
        public void BuildHuffmanTree(Dictionary<T, int> frequencies)
        {
            List<HuffmanNode<T>> nodes = frequencies
                .Select(freq => new HuffmanNode<T>(freq.Key, freq.Value))
                .ToList();

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(n => n.Frequency).ToList();

                HuffmanNode<T> left = nodes[0];
                HuffmanNode<T> right = nodes[1];

                HuffmanNode<T> parent = new HuffmanNode<T>(default(T), left.Frequency + right.Frequency);
                parent.Left = left;
                parent.Right = right;

                nodes.Remove(left);
                nodes.Remove(right);
                nodes.Add(parent);
            }

            Root = nodes[0];
        }

        public Dictionary<T, string> GenerateBits()
        {
            Dictionary<T, string> bits = new Dictionary<T, string>();
            GenerateBitsPr(Root, "", bits);
            return bits;
        }

        private void GenerateBitsPr(HuffmanNode<T> act, string bit, Dictionary<T, string> bits)
        {
            if (act == null) return;

            if (act.IsLeaf() && (act.Element != null))
                bits[act.Element] = bit;

            GenerateBitsPr(act.Left, bit + "1", bits);
            GenerateBitsPr(act.Right, bit + "0", bits);

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

        public List<T> PreOrder()
        {
            var temp = new List<T>();
            PreOrder(Root, temp);
            return temp;
        }
    }
}
