using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Lib.WordCheck;

namespace Wordle.Lib.UI
{
    public class KeyboardOverlay
    {
        private static readonly string _keyboard = "azertyuiop\nqsdfghjklm\nwxcvbn";
        public List<LetterValidation> EnteredKey { get; set; } = new List<LetterValidation>();
    
        public void LoadData(List<LetterValidation> validations)
        {
            foreach (var validation in validations)
            {
                if (EnteredKey.Any(k => k.Letter == validation.Letter))
                {
                    var key = EnteredKey.Find(k => k.Letter == validation.Letter);
                    if (key != null && key.Validity == LetterCheck.InWord)
                        key.Validity = validation.Validity;
                }
                else EnteredKey.Add(validation);
            }
        }

        /// <summary>
        /// Display the keyboard with correct/Incorrect/Not used letter
        /// </summary>
        public void Display()
        {
            var line = "";
            foreach (var letter in _keyboard)
            {
                if (letter == '\n' || _keyboard.Last() == letter)
                {

                    Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                    WriteKeyboadLine(line.Trim());
                    line = "";
                    continue;
                }
                string colorCode = "";
                if (EnteredKey.Any(k => k.Letter == letter))
                {
                    var entered = EnteredKey.First(k => k.Letter == letter);
                    colorCode = entered.Validity.GetColorCode();                    
                }
                line += $"{colorCode}{letter}  ";
            }
        }

        private void WriteKeyboadLine(string line)
        {
            bool isChangingColor = false;
            foreach (var letter in line)
            {
                if (letter == '^')
                {
                    isChangingColor = true;
                    continue;
                }
                if (isChangingColor)
                {
                    switch (letter)
                    {
                        case 'g':
                            Console.BackgroundColor = ConsoleColor.Green; break;
                        case 'y':
                            Console.BackgroundColor = ConsoleColor.DarkYellow; break;

                        case 'r':
                            Console.BackgroundColor = ConsoleColor.Red; break;

                        case 'w':
                            Console.BackgroundColor = ConsoleColor.Black; break;
                    }
                    isChangingColor = false;
                    continue;
                }
                Console.Write(letter);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
                    
    }
}
