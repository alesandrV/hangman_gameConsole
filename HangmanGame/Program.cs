using System;
using System.IO;

namespace HangmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            HangmanGame game = new HangmanGame();

            string word = game.GenerateWord();

            //add a simple UI
            Console.WriteLine($"The word consists of {word.Length} letters.");
            Console.WriteLine("Try ro guess the word.");

            //this loop works as long as the game status is InProgres
            while (game.GameStatus == GameStatus.InProgres)
            {
                Console.WriteLine("Pick a letter.");
                char c = Console.ReadLine().ToCharArray()[0];

                string curState = game.GuessLetter(c);
                Console.WriteLine(curState);

                Console.WriteLine($"Remaining tries = {game.RemainingTries}");
                Console.WriteLine($"Tries letters: {game.TriedLetters}");
            }

            //lost and win logic
            if (game.GameStatus == GameStatus.Lost)
            {
                Console.WriteLine("You`re hanged.");
                Console.WriteLine($"The word was: {game.Word}");
            }
            else if(game.GameStatus == GameStatus.Won)
            {
                Console.WriteLine("You won!");
            }

            Console.ReadLine();
        }
    }
}
