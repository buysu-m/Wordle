using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Lib.WordCheck
{
    public class WordList :IWordService
    {
        public string GetRandomWord()
        {
            string word = "";
            using (var sr = new StreamReader(@".\WordList.txt"))
            {
                var file = sr.ReadToEnd();
                var wordList = file.Split('\n');
                var rand = new Random();
                word = wordList[rand.Next(wordList.Length - 1)];
            }
            return word;
        }
    }
}
