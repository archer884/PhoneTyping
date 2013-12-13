using System;
using System.Linq;

namespace PhoneTyping
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            if (input.Contains(' '))
                foreach (var word in Translator.WordsStartingWith(Translator.TranslateExplicit(input)))
                    Console.WriteLine(word);
            
            else
                foreach (var word in Translator.WordsStartingWith(Translator.TranslateImplicit(input)))
                    Console.WriteLine(word);
        }
    }
}
