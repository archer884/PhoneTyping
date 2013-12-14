using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trie;

namespace PhoneTyping
{
    /// <summary>
    /// Based on a Trie implementation from:
    /// http://chris-miceli.blogspot.com/2012/06/net-trie-in-c-implementing-idictionary.html
    /// </summary>
    class TrieTranslator : ITranslator
    {
        #region internals
        private static readonly IDictionary<int, char[]> Mapping;
        private static readonly Trie<char, object> Words; 
        #endregion

        #region constructors/initialization
        static TrieTranslator()
        {
            Mapping = CreateMapping();
            Words = LoadWords();
        }

        private static IDictionary<int, char[]> CreateMapping()
        {
            var values = new[]
                {
                    "2ABC", "3DEF", "4GHI",
                    "5JKL", "6MNO", "7PQRS",
                    "8TUV", "9WXYZ",
                };

            return values.ToDictionary(
                entry => int.Parse(entry.Substring(0, 1)),
                entry => entry.Substring(1).ToCharArray());
        }

        private static Trie<char, object> LoadWords()
        {
            var trie = new Trie<char, object>();
            foreach (var word in File.ReadLines(@"E:\Development\Data\Words\enable1.txt")
                .Select(word => word.ToUpper())
                .OrderBy(word => word))
            {
                trie.Add(word, word);
            }
            return trie;
        }
        #endregion

        #region methods
        private string TranslateExplicit(IEnumerable<string> explicitCodes)
        {
            return explicitCodes
                .Select(code => Mapping[int.Parse(code.Substring(0, 1))][code.Length - 1])
                .AsString();
        }

        private IEnumerable<string> TranslateImplicit(string implicitCodes)
        {
            return implicitCodes
                .Select(character => Mapping[int.Parse(character.ToString())])
                .CartesianProduct()
                .Select(characters => characters.AsString())
                .OrderBy(word => word);
        }

        private IEnumerable<string> WordsStartingWith(IEnumerable<string> tokens)
        {
            return tokens
                .AsParallel()
                .SelectMany(WordsStartingWith)
                .Distinct();
        }

        private IEnumerable<string> WordsStartingWith(string token)
        {
            return Words
                .Suffixes(token)
                .Select(kv => kv.Key.AsString());
        }
        #endregion

        #region itranslator
        public IEnumerable<string> Implicit(string sequence)
        {
            return WordsStartingWith(TranslateImplicit(sequence));
        }

        public IEnumerable<string> Explicit(IEnumerable<string> sequence)
        {
            return WordsStartingWith(TranslateExplicit(sequence));
        } 
        #endregion
    }
}
