using System.Diagnostics.CodeAnalysis;

namespace Wordle.Lib.WordCheck
{
    public class LetterValidation
    {
        public LetterValidation(char letter)
        {
            Letter = letter;
        }
        public char Letter {  get; set; }
        public LetterCheck Validity { get; set; }
    }
}
