using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Lib.WordCheck;

namespace Wordle.Lib.UI
{
    public static class ColorFromCheck
    {
        public static ConsoleColor GetBackgroundColor(this LetterCheck check)
        {
            switch (check)
            {
                case LetterCheck.Valid:
                    return ConsoleColor.Green;
                case LetterCheck.InWord:
                    return ConsoleColor.DarkYellow;
                case LetterCheck.NotInWord:
                    return ConsoleColor.DarkGray;
                default:
                    return ConsoleColor.DarkGray;
            }
        }

        public static string GetColorCode(this LetterCheck check)
        {
            switch (check)
            {
                case LetterCheck.Valid:
                    return "^g";
                case LetterCheck.InWord:
                    return "^y";
                case LetterCheck.NotInWord:
                    return "^r";
                default:
                    return "^w";
            }
        }
    }
}
