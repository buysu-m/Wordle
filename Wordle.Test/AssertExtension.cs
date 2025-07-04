
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Lib.WordCheck;

namespace Wordle.Test
{
    public static class AssertExtension
    {
        public static void ListAreEqual(List<LetterValidation> expected, List<LetterValidation> actual, string? message = null)
        {
            Assert.AreEqual(expected.Count, actual.Count, message);
            for (int i = 0; i < expected.Count; i++)
            {
                var exp = expected[i];
                var act = actual[i];
                AreEqual(exp, act, message);
            }
        }
        public static void ListAreEqual(List<char> expected, List<char> actual, string? message = null)
        {
            Assert.AreEqual(expected.Count, actual.Count, message);
            for (int i = 0; i < expected.Count; i++)
            {
                var exp = expected[i];
                var act = actual[i];
                Assert.AreEqual(exp, act, message);
            }
        }


        public static void AreEqual(LetterValidation expected, LetterValidation actual, string? message = null)
        {
            Assert.AreEqual(expected.Letter, actual.Letter, message);
            Assert.AreEqual(expected.Validity, actual.Validity, message);   
        }
    }
}
