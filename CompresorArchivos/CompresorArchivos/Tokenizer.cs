using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompresorArchivos
{
    public class Tokenizer
    {
        public Tokenizer() {}

        public Dictionary<char, int> TokenizeText(string text)
        {
            Dictionary<char, int> frequencies = new Dictionary<char, int>();

            foreach (char c in text)
            {
                if (!frequencies.ContainsKey(c))
                    frequencies[c] = 0;

                frequencies[c]++;
            }

            return frequencies;
        }
    }
}
