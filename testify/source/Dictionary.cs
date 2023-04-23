namespace testify.source
{
    /// <summary>
    /// Class <c>Dictionary</c> contains a word and its serial number
    /// </summary>
    public class Dictionary
    {
        public uint WordId { get; }
        public string Word { get; }

        /// <summary>
        /// This constructor initializes the new dictionary item with the values
        /// (<paramref name="n"/>,<paramref name="w"/>).
        /// </summary>
        public Dictionary(uint n, string w)
        {
            WordId = n;
            Word = w;
        }

        /// <summary>
        /// Method <c>ToString</c> to string method
        /// </summary>
        /// <returns>
        /// A string representing a word and it's serial number, in the form of:  "n: word",
        /// to make it easily readable
        /// </returns>
        public override string ToString()
        {
            return WordId.ToString() + ": " + Word;
        }

        /// <summary>
        /// Method <c>Equals</c> checks whether the words of two different objects are the same or not
        /// </summary>
        /// <returns>
        /// Returns true if the words are the same, the serial number doesn't matter
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Dictionary d) return false;
            return obj.Equals(d) && Word == d.Word;
        }

        /// <summary>
        /// Method <c>GetHashCode</c> generates the hashcode
        /// </summary>
        /// <returns>
        /// Returns the hashcode of the item as an integer
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(WordId, Word);
        }
    }
}