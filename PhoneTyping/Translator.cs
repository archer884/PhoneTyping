    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    namespace PhoneTyping
    {
        public static class Translator
        {
            #region internals
            private static IDictionary<int, char[]> Mapping = CreateMapping();
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

            private static List<string> Words = ReadWords();
            private static List<string> ReadWords()
            {
                return File.ReadLines(@"C:\Development\Data\Words\enable1.txt")
                    .Select(word => word.ToUpper())
                    .OrderBy(word => word)
                    .ToList();
            }
            #endregion

            #region public methods
            public static TimeSpan Initialize()
            {
                var clock = Stopwatch.StartNew();
                var x = Mapping.Count;
                var y = Words.Count;
                return clock.Elapsed;
            }

            public static string TranslateExplicit(string explicitCodes)
            {
                return TranslateExplicit(explicitCodes.Split(' '));
            }

            public static string TranslateExplicit(IEnumerable<string> explicitCodes)
            {
                return explicitCodes
                    .Select(code => Mapping[int.Parse(code.Substring(0, 1))][code.Length - 1])
                    .AsString();
            }

            public static IEnumerable<string> TranslateImplicit(string implicitCodes)
            {
                return implicitCodes
                    .Select(character => Mapping[int.Parse(character.ToString())])
                    .CartesianProduct()
                    .Select(characters => characters.AsString())
                    .OrderBy(word => word);
            }

            public static IEnumerable<string> WordsStartingWith(IEnumerable<string> tokens)
            {
                return tokens
                    .AsParallel()
                    .SelectMany(WordsStartingWith)
                    .Distinct();
            }

            public static IEnumerable<string> WordsStartingWith(string token)
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

            #region extensions
            public static string AsString(this IEnumerable<char> characters)
            {
                return new string(characters.ToArray());
            }

            /// <summary>
            /// Eric Lippert's CartesianProduct Linq extension. Pretty cool, hooah?
            /// http://blogs.msdn.com/b/ericlippert/archive/2010/06/28/computing-a-cartesian-product-with-linq.aspx
            /// </summary>
            public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
            {
                IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
                return sequences.Aggregate(
                    emptyProduct,
                    (accumulator, sequence) => 
                        from accseq in accumulator
                        from item in sequence
                        select accseq.Concat(new[] {item }));
            }
            #endregion
        }
    }
