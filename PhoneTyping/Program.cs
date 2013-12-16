using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhoneTyping
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new PrefixTree();
            tree.Add("tea");
            tree.Add("tee");
            foreach (var item in tree)
                Console.WriteLine(item);
            return;

            var values = CreateTestValues(10000, 16).ToList();
            var binaryTranslator = new BinaryTranslator();
            var trieTranslator = new TrieTranslator();

            Console.WriteLine(GetRuntime(() => Console.WriteLine(RunTest(values, binaryTranslator))));
            Console.WriteLine(GetRuntime(() => Console.WriteLine(RunTest(values, trieTranslator))));
        }

        static long RunTest(IEnumerable<string> values, ITranslator translator)
        {
            return values.Sum(value => translator.Implicit(value).Count());
        }

        static TimeSpan GetRuntime(Action action)
        {
            var clock = Stopwatch.StartNew();
            action();
            return clock.Elapsed;
        }

        static IEnumerable<string> CreateTestValues(int length, int? seed = null)
        {
            var random = seed.HasValue 
                ? new Random(seed.Value) 
                : new Random();

            for (int i = 0; i < length; i++)
            {
                yield return (random.Next(8) + 2).ToString()
                    + (random.Next(8) + 2).ToString()
                    + (random.Next(8) + 2).ToString()
                    + (random.Next(8) + 2).ToString();
            }
        }
    }
}
