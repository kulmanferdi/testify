using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testify
{
    internal class Dictionary
    {
        private int nthWord;
        private string word;

        public Dictionary(int n, string w)
        {
            nthWord = n;
            word = w;
        }

        public int GetnthWord()
        {
            return nthWord;
        }
        public string GetWord()
        {
            return word;
        }

        public override string ToString()
        {
            return nthWord.ToString() + ": " + word;
        }
    }
}
