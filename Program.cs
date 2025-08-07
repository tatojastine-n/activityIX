using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGameSimulator
{
    internal class Program
    {
        private static readonly int Size = 4;
        private static char[,] board = new char[Size, Size];
        private static bool[,] revealed = new bool[Size, Size];
        private static int matchedPairs = 0;
        private static void Main(string[] args)
        {
            InitializeBoard();
            PlayGame();
        }
        private static void InitializeBoard()
        {
            var letters = new List<char>();
            for (char c = 'A'; c <= 'H'; c++)
            {
                letters.Add(c);
                letters.Add(c);
            }

            var rng = new Random();
            letters = letters.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    board[i, j] = letters[i * Size + j];
                    revealed[i, j] = false;
                }
            }
        }

        static void PlayGame()
        {
            Console.WriteLine("Welcome to Memory Card Pair Matcher!");
            Console.WriteLine("Find all 8 matching pairs (A-H) in the 4x4 grid.");

            while (matchedPairs < 8)
            {
                DisplayBoard();

                // Get first card position
                var pos1 = GetValidPosition("Enter first card position (row,col): ");
                while (revealed[pos1.Item1, pos1.Item2])
                {
                    Console.WriteLine("That card is already revealed. Try again.");
                    pos1 = GetValidPosition("Enter first card position (row,col): ");
                }
                revealed[pos1.Item1, pos1.Item2] = true;
                DisplayBoard();

                // Get second card position
                var pos2 = GetValidPosition("Enter second card position (row,col): ");
                while (pos2.Equals(pos1) || revealed[pos2.Item1, pos2.Item2])
                {
                    if (pos2.Equals(pos1))
                        Console.WriteLine("Cannot pick the same card. Try again.");
                    else
                        Console.WriteLine("That card is already revealed. Try again.");
                    pos2 = GetValidPosition("Enter second card position (row,col): ");
                }
                revealed[pos2.Item1, pos2.Item2] = true;
                DisplayBoard();

                // Check for match
                if (board[pos1.Item1, pos1.Item2] == board[pos2.Item1, pos2.Item2])
                {
                    Console.WriteLine("Match found!");
                    matchedPairs++;
                }
                else
                {
                    Console.WriteLine("No match. Cards will be hidden again.");
                    revealed[pos1.Item1, pos1.Item2] = false;
                    revealed[pos2.Item1, pos2.Item2] = false;
                    System.Threading.Thread.Sleep(2000); // Pause to let player see
                }
            }
            Console.WriteLine("Congratulations! You matched all pairs!");
        }
            
        static Tuple<int, int> GetValidPosition(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine().Split(',');
                if (input.Length == 2 &&
                    int.TryParse(input[0], out int row) &&
                    int.TryParse(input[1], out int col) &&
                    row >= 1 && row <= Size &&
                    col >= 1 && col <= Size)
                {
                    return Tuple.Create(row - 1, col - 1);
                }
                Console.WriteLine($"Invalid input. Please enter row,col (1-{Size})");
            }
        }
        static void DisplayBoard()
        {
            Console.Clear();
            Console.WriteLine("Current Board:\n");

            // Column numbers
            Console.Write("   ");
            for (int j = 0; j < Size; j++)
            {
                Console.Write($" {j + 1}  ");
            }
            Console.Write("\n");

            for (int i = 0; i < Size; i++)
            {
                Console.Write($" {i + 1} ");

                // Card values
                for (int j = 0; j < Size; j++)
                {
                    if (revealed[i, j])
                        Console.Write($"[{board[i, j]}] ");
                    else
                        Console.Write("[ ] ");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
