using Wordle.Lib.WordCheck;

namespace Wordle.Test
{
    [TestClass]
    public sealed class WordChercker_Property_Test
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
        public void AttemptMaxVerification()
        {
            var attemptMax = 15;
            var checker = new WordChecker("Word", attemptMax);

            Assert.AreEqual(attemptMax, checker.AttemptMax);
        }
    }
}
