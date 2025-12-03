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

            Console.WriteLine("Choose the game's difficulty (not implemented, just press Enter)");
            Console.WriteLine("1. Easy  2. Normal  3. Hard");
            Console.ReadLine();

            // Start the game
            Game game = new Game();
            game.Run();
        }
    }
}
