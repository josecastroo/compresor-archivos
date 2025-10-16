using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompresorArchivos
{
    public class CompresorManager
    {
        private readonly Tokenizer _tokenizer;
        private readonly Compresor _compresor;
        private readonly Descompresor _descompresor;

        public CompresorManager()
        {
            _tokenizer = new Tokenizer();
            _compresor = new Compresor();
            _descompresor = new Descompresor();
        }

        public (string, HuffmanTree<char>) ComprimirArchivo(string filePath)
        {
            Dictionary<char, int> frequencies = _tokenizer.TokenizeFile(filePath);
            HuffmanTree<char> huffmanTree = new HuffmanTree<char>();
            huffmanTree.BuildHuffmanTree(frequencies);

            Dictionary<char, string> traductor = huffmanTree.GenerateBits();
            string text = File.ReadAllText(filePath);
            string textoComprimido = _compresor.Comprimir(text, traductor);

            return (textoComprimido, huffmanTree);
        }

        public string DescomprimirArchivo(string textoComprimido, HuffmanTree<char> huffmanTree)
        {
            return _descompresor.Descomprimir(textoComprimido, huffmanTree);
        }

        public void GuardarArchivoComprimido(string filePath, string textoComprimido, HuffmanTree<char> huffmanTree)
        {
            string treeData = ConvertirTreeAString(huffmanTree.Root);
            File.WriteAllText(filePath + ".huff", textoComprimido);
            File.WriteAllText(filePath + ".tree", treeData);
        }

        public (string, HuffmanTree<char>) CargarArchivoComprimido(string filePath)
        {
            string textoComprimido = File.ReadAllText(filePath + ".huff");
            string treeData = File.ReadAllText(filePath + ".tree");

            var huffmanTree = new HuffmanTree<char>();
            huffmanTree.Root = TreeDesdeString(treeData);

            return (textoComprimido, huffmanTree);
        }

        private string ConvertirTreeAString(HuffmanNode<char> node)
        {
            if (node == null) return "null";
            if (node.IsLeaf())
            {
                char element = node.Element;
                
                if (element == 'L' || element == 'I' || element == '(' || element == ')' || element == '\\')
                {
                    return $"L\\{element}";
                }
                return $"L{element}";
            }
            return $"I({ConvertirTreeAString(node.Left)})({ConvertirTreeAString(node.Right)})";
        }

        private HuffmanNode<char> TreeDesdeString(string data)
        {
            int index = 0;
            return TreeDesdeString(data, ref index);
        }

        private HuffmanNode<char> TreeDesdeString(string data, ref int index)
        {
            if (index >= data.Length) return null;

            if (data.Length - index >= 4 && data.Substring(index, 4) == "null")
            {
                index += 4;
                return null;
            }

            if (data[index] == 'L')
            {
                index++; // saltar L
                char element;
                
                if (index < data.Length && data[index] == '\\')
                {
                    index++; // saltar \\
                    element = data[index];
                }
                else
                {
                    element = data[index];
                }
                index++;
                return new HuffmanNode<char>(element, 0);
            }

            if (data[index] == 'I')
            {
                index++; // saltar I
                index++; // saltar (
                var left = TreeDesdeString(data, ref index);
                index++; // saltar )
                index++; // saltar (
                var right = TreeDesdeString(data, ref index);
                index++; // salta )
                return new HuffmanNode<char>(default(char), 0, left, right);
            }

            return null;
        }
    }
}