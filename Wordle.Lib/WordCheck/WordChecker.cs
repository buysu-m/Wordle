namespace Wordle.Lib.WordCheck
{
    public class WordChecker
    {
        private char[] _word;
        private List<List<LetterValidation>> _attempts;
        private Dictionary<char, int> _letterCount;
        private readonly int _attemptMax;
        private List<char> _correctFound;
        private bool _hasWin = false;
        private static IWordService _wordService = new WordList();

        /// <summary>
        /// <see langword="Get"/> the length of the secret word to find
        /// </summary>
        public int WordLength => _word.Length;

        /// <summary>
        /// <see langword="Get"/> the attempt list with the valid/invalid data
        /// </summary>
        public List<List<LetterValidation>> Attempts => _attempts;

        /// <summary>
        /// <see langword="Get"/> the max attempt to reach
        /// </summary>
        public int AttemptMax => _attemptMax;

        /// <summary>
        /// <see langword="Get"/> the place of the correct letter already found
        /// </summary>
        public List<char> CorrectFound => _correctFound;

        /// <summary>
        /// Tell if this instance was lost
        /// </summary>
        public bool HasLost => _attempts.Count == _attemptMax;

        //public bool HasWin => !_correctFound.Any(validation => validation.Validity != LetterCheck.Valid); // If all letter validation is valid, then hasWin is true
        
        /// <summary>
        /// Tell if this instance was won
        /// </summary>
        public bool HasWin => _hasWin;

        /// <summary>
        /// Single constructor
        /// </summary>
        /// <param name="word">The word to save and try to find (keep it a secret to other player)
        /// <br/>Enter only letters
        /// </param>
        /// <param name="attemptMax">The max attempt to limit the player to find the word</param>
        /// <exception cref="ArgumentException">Throw an exception if you enter anything else than letter for the word</exception>
        public WordChecker(string word, int attemptMax)
        {
            word = word.Trim().ToLower();
            if (!word.All(c => char.IsLetter(c) || c == '-'))
                throw new ArgumentException("word entered should only be letter");
            _word = word.ToCharArray();
            _correctFound = new List<char>();
            for (var i = 0; i < word.Length; i++)
            {
                _correctFound.Add(default);
            }
            _letterCount = countLetters(word);
            _attempts = new List<List<LetterValidation>>();
            _attemptMax = attemptMax;
            _wordService = new WordList();
        }

        private Dictionary<char, int> countLetters(string word)
        {
            var dic = new Dictionary<char, int>();
            foreach (var letter in word)
            {
                if (dic.ContainsKey(letter))
                    dic[letter]++;
                else dic.Add(letter, 1);
            }
            return dic;
        }

        /// <summary>
        /// Check if <paramref name="word"/> is the correct word
        /// </summary>
        /// <param name="word"></param>
        /// <returns>A list of <see cref="LetterValidation"/> to tell for each letter if they are correct</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public List<LetterValidation> CheckWord(string word)
        {
            word = word.ToLower();
            if (HasWin)
            {
                throw new InvalidOperationException("You already won this game");
            }
            // Block method usage if the word entered is not the correctLength
            if (word.Length != _word.Length)
                throw new IndexOutOfRangeException("Word entered is not the same length as the word to check. Word Length must be: " + _word.Length);

            var validations = new List<LetterValidation>();

            var countLetterCheck = new Dictionary<char, int>();

            // Letter by letter check
            for (var i = 0; i < _word.Length; i++)
            {
                var enteredLetter = char.ToLower(word[i]);
                var correctLetter = char.ToLower(_word[i]);

                var letterValidation = new LetterValidation(enteredLetter);

                // Count the iteration of the letter in the entered word
                if (countLetterCheck.ContainsKey(enteredLetter))
                    countLetterCheck[enteredLetter]++;
                else countLetterCheck.Add(enteredLetter, 1);

                var enteredAmount = countLetterCheck[enteredLetter];

                
                letterValidation.Validity = _letterCount.TryGetValue(enteredLetter, out int correctAmount)
                    ? letterValidity(enteredLetter, correctLetter, enteredAmount, correctAmount)
                    : LetterCheck.NotInWord;

                if (letterValidation.Validity == LetterCheck.Valid && enteredAmount > correctAmount)
                    validations.Last(v => v.Letter == enteredLetter && v.Validity == LetterCheck.InWord).Validity = LetterCheck.NotInWord;


                validations.Add(letterValidation);
                if (letterValidation.Validity == LetterCheck.Valid)
                    _correctFound[i] = letterValidation.Letter;
            }
            // Increment attempt if the word entered is not the word to check
            if (validations.Any(v => v.Validity != LetterCheck.Valid))
                _attempts.Add(validations);
            else _hasWin = true;

            return validations;
        }

        private LetterCheck letterValidity(char enteredLetter, char correctLetter, int enteredAmount, int correctAmount)
        {
            if (enteredLetter == correctLetter)
                return LetterCheck.Valid;
            else if (_word.Contains(enteredLetter) && enteredAmount <= correctAmount)
                return LetterCheck.InWord;
            else return LetterCheck.NotInWord;
        }

        public static WordChecker NewGame()
        {
            var word = _wordService.GetRandomWord();
            var attempts = (int)Math.Floor(word.Length * 0.75);
            return new WordChecker(word, attempts);
        }

        public static void ChangeWordService(IWordService service)
        {
            _wordService = service;
        }
    }
}
