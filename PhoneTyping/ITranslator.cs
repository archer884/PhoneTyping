using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTyping
{
    interface ITranslator
    {
        IEnumerable<string> Implicit(string sequence);
        IEnumerable<string> Explicit(IEnumerable<string> sequence);
    }
}
