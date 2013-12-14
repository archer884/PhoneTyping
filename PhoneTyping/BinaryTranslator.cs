    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    namespace PhoneTyping
    {
        public class BinaryTranslator : ITranslator
        {
            #region internals
            private static readonly IDictionary<int, char[]> Mapping;
            private static readonly List<string> Words;
            #endregion

            #region constructors/initialization
            static BinaryTranslator()
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

            private static List<string> LoadWords()
            {
                return File.ReadLines(@"E:\Development\Data\Words\enable1.txt")
                    .Select(word => word.ToUpper())
                    .OrderBy(word => word)
                    .ToList();
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
                // if firstIndex is less than zero, our exact combination of characters 
                // was not found, but we will begin searching at the binary inverse of 
                // the value returned by BinarySearch()
                var firstIndex = Words.BinarySearch(token);
                if (firstIndex < 0)
                    firstIndex = ~firstIndex;

                return Words
                    .Skip(firstIndex)
                    .TakeWhile(word => word.StartsWith(token));
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
