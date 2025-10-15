using System;
using System.Collections.Generic;
using System.Text;

namespace CompresorArchivos
{
    public class Compresor
    {
        public Compresor() {}

        public string Comprimir(string text, Dictionary<char, string> traductor)
        {
            StringBuilder textComprimido = new StringBuilder();

            foreach (char c in text)
            {
                if (traductor.TryGetValue(c, out string? code))
                {
                    textComprimido.Append(c);
                }
                else
                {
                    throw new ArgumentException($"Caracter {c} no encontrado en el diccionario traductor.");
                }
            }
            return textComprimido.ToString();
        }
    }
}
