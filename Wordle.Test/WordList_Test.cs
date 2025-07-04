using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wordle.Lib.WordCheck;

namespace Wordle.Test
{
    [TestClass]
    public class WordList_Test
    {
        [TestMethod]
        public void TestCollectingFromFile()
        {
            var wordList = new WordList();
            var word = wordList.GetRandomWord();
            Assert.IsNotNull(word, "Word list file don't load properly");
        }
    }
}
