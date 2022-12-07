using System.Drawing;

namespace testify
{
    /// <summary>
    /// Class <c>Dictionary</c> contains a word and its serial number
    /// </summary>
    internal class Dictionary
    {
        public int nthWord { get; }
        public string word { get; }

        /// <summary>
        /// This constructor initializes the new dictionary item with the values
        /// (<paramref name="n"/>,<paramref name="w"/>).
        /// </summary>
        public Dictionary(int n, string w)
        {
            nthWord = n;
            word = w;
        }

        /// <summary>
        /// Method <c>ToString</c> to string method
        /// </summary>
        /// <returns>
        /// A string representing a word and it's serial nubmer, in the form n: word,
        /// to make it easily readable
        /// </returns>
        public override string ToString()
        {
            return nthWord.ToString() + ": " + word;
        }

        /// <summary>
        /// Method <c>Equals</c> checks whether the words of two different objects are the same or not
        /// </summary>
        /// <returns>
        /// Returns true if the words are the same, the serial number doesn't matter
        /// </returns>
        public override bool Equals(Object obj)
        {
            Dictionary d = obj as Dictionary;
            if (d == null) return false;
            else return base.Equals((Dictionary)obj) && word == d.word;
        }
    }
}