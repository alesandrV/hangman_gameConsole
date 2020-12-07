using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HangmanGame
{
    class HangmanGame
    {
        private readonly int allowedMisses;
        private bool[] openIndexes;//to track letters with their indexes
        private int triesCounter = 0;
        private string triedLetters;//we will return used letters sorted

        public GameStatus GameStatus { get; private set; } = GameStatus.NotStarted;//default game status

        public string Word { get; private set; }

        public string TriedLetters
        {
            get
            {
                var chars = triedLetters.ToCharArray();
                Array.Sort(chars);
                return new string(chars);
            }
        }

        //number of possible tries
        public int RemainingTries 
        {
            get
            {
                return allowedMisses - triesCounter;
            }        
        }

        public HangmanGame(int allowedMisses = 6)
        {
            //a number of tries
            if (allowedMisses < 5 || allowedMisses > 8)
            {
                throw new ArgumentException("Number of allowed misses should be betwen 5 and 8");
            }

            this.allowedMisses = allowedMisses;
        }

        //returns a number of letters of the chosen word
        public string GenerateWord()
        {
            string[] words = File.ReadAllLines("WordsStockRus.txt");//read our file
            Random r = new Random(DateTime.Now.Millisecond);
            int randomIndex = r.Next(words.Length - 1);//generate random number

            //take a word from the array with a randomly generated index
            Word = words[randomIndex];

            //create an array with letters in chosen word
            openIndexes = new bool[Word.Length];

            GameStatus = GameStatus.InProgres;//our game started

            return Word;
        }

        public string GuessLetter(char letter)
        {
            if (triesCounter == allowedMisses)
            {
                throw new InvalidOperationException($"Exceeded the max misses number:{allowedMisses}");
            }

            if (GameStatus != GameStatus.InProgres)
            {
                throw new InvalidOperationException($"Inaproppriate status of the game:{GameStatus}");
            }

            //find out if any letters have been guessed
            bool openAny = false;

            //try to find players letter in chosen word
            string result = string.Empty;
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == letter)
                {
                    openIndexes[i] = true;
                    openAny = true;
                }

                if (openIndexes[i])
                {
                    result += Word[i];
                }

                else
                {
                    result += "-";
                }
            }

            //increase the number of tries if player made a mistake
            if (!openAny)
            {
                triesCounter++;
            }

            triedLetters += letter;

            //determine win or lose
            if (IsWin())
            {
                GameStatus = GameStatus.Won;
            }
            else if (triesCounter == allowedMisses)
            {
                GameStatus = GameStatus.Lost;
            }

            return result;
        }

        //checking the number of correct letters
        private bool IsWin()
        {
            foreach (var cur in openIndexes)
            {
                if (cur == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}