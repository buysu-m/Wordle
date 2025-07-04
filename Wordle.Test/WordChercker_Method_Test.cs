using Moq;
using System.ComponentModel.DataAnnotations;
using Wordle.Lib.WordCheck;

namespace Wordle.Test
{
    [TestClass]
    public sealed class WordChercker_Method_Test
    {
        [TestMethod]
        public void IncorrectWordConstructor()
        {
            var incorrectWord = "Je ne suis pas correcte 12345";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new WordChecker(incorrectWord, 1);
            });
        }

        [TestMethod]
        public void DontLoseTooEarly()
        {
            var checker = new WordChecker("Tests", 3);
            for (int i = 0; i < 2; i++)
            {
                checker.CheckWord("testi");
                Assert.IsFalse(checker.HasLost, "should'nt lost till third try");
            }
            checker.CheckWord("lasts");
            Assert.IsTrue(checker.HasLost, "should have lost the third time");
        }

        [TestMethod]
        public void DontWinOnFirstTry()
        {
            var word = "Tests";
            var checker = new WordChecker(word, 3);

            // almost correct try
            checker.CheckWord(word.Replace(word.Last(), 'a'));
            Assert.IsFalse(checker.HasWin, "should not win, the word is not the first entered word to check");

            //Correct try
            checker.CheckWord(word);
            Assert.IsTrue(checker.HasWin, "should win");
        }

        [TestMethod]
        public void WordLengthIsCorrect()
        {
            var word = "soleil";
            var checker = new WordChecker(word, 1);
            Assert.AreEqual(word.Length, checker.WordLength, "WordChecker.WordLength property give the wrong word length");
        }

        [TestMethod]
        public void SaveAttemptsCorrectly()
        {
            var word = "Soleil";
            var checker = new WordChecker(word, 4);

            var wordTest1 = "Carafe";
            var validityWord1 = new List<LetterValidation>
            {
                new LetterValidation('c'),
                new LetterValidation('a'),
                new LetterValidation('r'),
                new LetterValidation('a'),
                new LetterValidation('f'),
                new LetterValidation('e') { Validity = LetterCheck.InWord },
            };
            checker.CheckWord(wordTest1);
            var wordTest2 = "Saoule";
            var validityWord2 = new List<LetterValidation>
            {
                new LetterValidation('s') { Validity = LetterCheck.Valid },
                new LetterValidation('a'),
                new LetterValidation('o') { Validity = LetterCheck.InWord },
                new LetterValidation('u'),
                new LetterValidation('l') { Validity = LetterCheck.InWord },
                new LetterValidation('e') { Validity = LetterCheck.InWord }
            };
            checker.CheckWord(wordTest2);

            AssertExtension.ListAreEqual(validityWord1, checker.Attempts[0], "the validity of the word 'carafe' should be equal");
            AssertExtension.ListAreEqual(validityWord2, checker.Attempts[1], "the validity of the word 'saoule' should be equal");
        }

        [TestMethod]
        public void CorrectFoundSaveCorrectly()
        {
            var word = "Soleil";
            var checker = new WordChecker(word, 4);

            var wordTest1 = "Corele";
            checker.CheckWord(wordTest1);
            var wordTest2 = "Saoule";
            checker.CheckWord(wordTest2);
            var validityWordTotal = new List<char>
            {
                's',
                'o',
                default,
                'e',
                default,
                default
            };
            AssertExtension.ListAreEqual(validityWordTotal, checker.CorrectFound);
        }

        [TestMethod]
        public void TryToPlayWhenAlreadyWon()
        {
            var word = "LeMot";
            var checker = new WordChecker(word, 2);

            checker.CheckWord(word);

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                checker.CheckWord("UnMot");
            });
        }

        [TestMethod]
        public void WordLengthMismatch()
        {
            var checker = new WordChecker("Test", 1);
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                checker.CheckWord("12345");
            },
            "Should throw when the word checked is not the same size as the word to find");
        }

        [TestMethod]
        public void MultipleLetterHandling()
        {
            var checker = new WordChecker("lele", 2);
            var testWord1 = "llou";
            var validityWord1 = new List<LetterValidation>
            {
                new LetterValidation('l') { Validity = LetterCheck.Valid},
                new LetterValidation('l') { Validity = LetterCheck.InWord},
                new LetterValidation('o'),
                new LetterValidation('u'),
            };
            checker.CheckWord(testWord1);

            var testWord2 = "lllu";
            var validityWord2 = new List<LetterValidation>
            {
                new LetterValidation('l') { Validity = LetterCheck.Valid},
                new LetterValidation('l'),
                new LetterValidation('l') { Validity = LetterCheck.Valid},
                new LetterValidation('u'),
            };
            checker.CheckWord(testWord2);

            AssertExtension.ListAreEqual(validityWord1, checker.CheckWord(testWord1));
            AssertExtension.ListAreEqual(validityWord2, checker.CheckWord(testWord2));
        }

        [TestMethod]
        public void GameCreation()
        {
            var mock = new Mock<IWordService>();
            mock.Setup(service => service.GetRandomWord()).Returns("TestWord");
            WordChecker.ChangeWordService(mock.Object);

            var checker = WordChecker.NewGame();
            checker.CheckWord("Testword");
            Assert.IsTrue(checker.HasWin, "The random game creation static method don't work");
        }
    }
}
