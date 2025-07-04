using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Lib.WordCheck
{
    public interface IWordService
    {
        public string GetRandomWord();
    }
}
