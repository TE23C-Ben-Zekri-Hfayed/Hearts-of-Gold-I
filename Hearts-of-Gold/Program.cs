using System;

namespace HeartsOfGold
{
    class Program
    {
        static void Main(string[] args)
        {
            // Welcome the player
            Console.WriteLine("Welcome to Hearts of Gold I");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            // Ask for the player's name and keep asking until they type something
            Console.WriteLine("Please enter your name:");
            string playerName = Console.ReadLine();

            while (string.IsNullOrEmpty(playerName))
            {
                Console.WriteLine("Invalid name, try again:");
                playerName = Console.ReadLine();
            }

            Console.WriteLine("Welcome, " + playerName + "!");

            // Ask the player to pick a difficulty (stored but not yet used)
            Console.WriteLine("Choose the game's difficulty:");
            Console.WriteLine("1. Easy   2. Normal   3. Hard");
            string difficulty = Console.ReadLine();

            if (difficulty == "1")
            {
                Console.WriteLine("You chose Easy. Good luck!");
            }
            else if (difficulty == "2")
            {
                Console.WriteLine("You chose Normal. Good luck!");
            }
            else if (difficulty == "3")
            {
                Console.WriteLine("You chose Hard. Good luck!");
            }
            else
            {
                Console.WriteLine("Unrecognised choice, starting on Normal.");
            }

            Console.WriteLine("Press Enter to start...");
            Console.ReadLine();

            // Start the game
            Game game = new Game();
            game.Run();
        }
    }
}