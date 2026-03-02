using System;


namespace HeartsOfGold
{

    class Program
    {
        static void Main(string[] args)
        {


            // Intro
            Console.WriteLine("Welcome to Hearts of Gold I");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            string playerName = Console.ReadLine();

            while (string.IsNullOrEmpty(playerName))
            {
                Console.WriteLine("Please enter your name:");
                playerName = Console.ReadLine();

                if (string.IsNullOrEmpty(playerName))
                {
                    Console.WriteLine("Invalid name, try again.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            Console.ReadLine();

            Console.WriteLine("Choose the game's difficulty");
            Console.WriteLine("1. Easy  2. Normal  3. Hard");
            Console.ReadLine();

            // Start the game
            Game game = new Game();
            game.Run();
        }
    }
}
