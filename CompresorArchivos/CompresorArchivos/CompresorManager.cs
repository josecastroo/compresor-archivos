using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public (string, HuffmanTree<char>, Dictionary<char, int>) ComprimirArchivo(string filePath)
        {
            Dictionary<char, int> frequencies = _tokenizer.TokenizeFile(filePath);
            HuffmanTree<char> huffmanTree = new HuffmanTree<char>();
            huffmanTree.BuildHuffmanTree(frequencies);

            Dictionary<char, string> traductor = huffmanTree.GenerarBits();
            string text = File.ReadAllText(filePath);
            string textoComprimido = _compresor.Comprimir(text, traductor);

            return (textoComprimido, huffmanTree, frequencies);
        }

        public string DescomprimirArchivo(string textoComprimido, HuffmanTree<char> huffmanTree)
        {
            return _descompresor.Descomprimir(textoComprimido, huffmanTree);
        }

        public void GuardarArchivoComprimido(string filePath, string textoComprimido, Dictionary<char, int> frequencies)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                // metadatos
                writer.Write(frequencies.Count);

                // tabla de símbolos/frecuencias
                foreach (var kvp in frequencies)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }

                byte padding = (byte)((8 - textoComprimido.Length % 8) % 8);
                writer.Write(padding);

                List<byte> bytes = new List<byte>();
                for (int i = 0; i < textoComprimido.Length; i += 8)
                {
                    string byteString = textoComprimido.Substring(i, Math.Min(8, textoComprimido.Length - i));
                    if (byteString.Length < 8)
                    {
                        byteString = byteString.PadRight(8, '0');
                    }
                    bytes.Add(Convert.ToByte(byteString, 2));
                }
                writer.Write(bytes.ToArray());
            }
        }

        public (string, HuffmanTree<char>) CargarArchivoComprimido(string filePath)
        {
            Dictionary<char, int> frequencies = new Dictionary<char, int>();
            string textoComprimido;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                int frequencyCount = reader.ReadInt32();

                for (int i = 0; i < frequencyCount; i++)
                {
                    char character = reader.ReadChar();
                    int frequency = reader.ReadInt32();
                    frequencies[character] = frequency;
                }

                byte lect = reader.ReadByte();
                byte[] compressedBytes = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                
                StringBuilder sb = new StringBuilder();
                foreach (byte b in compressedBytes)
                {
                    sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                }

                textoComprimido = sb.ToString();
                textoComprimido = textoComprimido.Substring(0, textoComprimido.Length - lect);
            }

            HuffmanTree<char> huffmanTree = new HuffmanTree<char>();
            huffmanTree.BuildHuffmanTree(frequencies);

            return (textoComprimido, huffmanTree);
        }
    }
}
