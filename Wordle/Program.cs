using Wordle.Lib.UI;
using Wordle.Lib.WordCheck;

var ui = new WordleUI();

while (true)
{
    var checker = WordChecker.NewGame();

    var word = "";
    while (!checker.HasLost && !checker.HasWin)
    {
        ui.Display(word, checker);
        var key = Console.ReadKey();

        if (key.Key == ConsoleKey.Backspace && word != string.Empty)
        {
            word = word.Remove(word.Length - 1, 1);
        }

        else if ((key.Key == ConsoleKey.Tab || key.Key == ConsoleKey.Enter) && word.Length == checker.WordLength)
        {
            checker.CheckWord(word);
            if (!checker.HasWin)
                word = "";
        }
        else if (char.IsLetter(key.KeyChar) || key.KeyChar == '-')
        {
            if (word.Length >= checker.WordLength)
            {
                Console.Beep();
            }
            else
            {
                word += key.KeyChar;
            }
        }
    }

    if (checker.HasLost)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("You LOSE, try again next time!");
        Console.ForegroundColor = ConsoleColor.White;
    }

    if (checker.HasWin)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"GG WP, you win the word were in fact {word}!");
        Console.ForegroundColor = ConsoleColor.White;
    }

    Console.WriteLine("Do you want to play another game of wordle ? (y/n, default: n)");
    var quitConfirm = Console.ReadKey();
    if (quitConfirm.Key != ConsoleKey.N)
        break;
}