using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompresorArchivos
{
    public class Descompresor
    {
        public Descompresor() {}

        public string Descomprimir(string textoComprimido, HuffmanTree<char> huffmanTree)
        {
            if (huffmanTree == null || huffmanTree.Root == null)
            {
                throw new ArgumentException("El árbol de Huffman es null o está vacío.");
            }

            StringBuilder textoDescomprimido = new StringBuilder();
            HuffmanNode<char> current = huffmanTree.Root;

            foreach (char bit in textoComprimido)
            {
                if (bit == '1')
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }

                if (current.IsLeaf())
                {
                    textoDescomprimido.Append(current.Element);
                    current = huffmanTree.Root;
                }
            }

            return textoDescomprimido.ToString();
        }
    }
}
