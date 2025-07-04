using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wordle.Lib.WordCheck;

namespace Wordle.Lib.UI
{
    public class WordleUI
    {
        private (int Left, int Top)? _wordStart;

        public void Display(string wordEntered, WordChecker checker)
        {
            Console.Clear();
            Console.WriteLine("====================");
            Console.WriteLine("#      WORDLE      #");
            Console.WriteLine("====================");
            Console.WriteLine($"Attempt: {checker.Attempts.Count+1}/{checker.AttemptMax}");

            var kbOverlay = new KeyboardOverlay();

            foreach(var attempt in checker.Attempts)
            {
                WriteAttempt(attempt);
                kbOverlay.LoadData(attempt);
            }

            Console.WriteLine('\n');
            kbOverlay.Display();
            Console.WriteLine('\n');

            Console.Write("The word is: ");
            setWord(wordEntered, checker);
        }

        private void WriteAttempt(List<LetterValidation> letterValidations)
        {
            Console.Write("Attempt: ");
            foreach (LetterValidation letterValidation in letterValidations)
            {
                Console.BackgroundColor = letterValidation.Validity.GetBackgroundColor();
                Console.Write(letterValidation.Letter);
            }
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
        }


        private void setWord(string wordEntered, WordChecker checker)
        {
            if (_wordStart is null)
                _wordStart = Console.GetCursorPosition();

            Console.SetCursorPosition(_wordStart.Value.Left, _wordStart.Value.Top + checker.Attempts.Count);
            Console.Write(wordEntered);
            for (int i = 0; i < checker.WordLength; i++)
            {
                if (i <= wordEntered.Length -1)
                    continue;

                var letter = checker.CorrectFound.ElementAtOrDefault(i);
                if (letter is default(char))
                {
                    Console.Write('_');
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write(letter);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            Console.SetCursorPosition(_wordStart.Value.Left + wordEntered.Length, _wordStart.Value.Top + checker.Attempts.Count);
        }

    }

    
}