using System;

namespace HeartsOfGold
{
    public class Game
    {
        private World world;
        private Country selectedCountry;
        private Random rng;

        public Game()
        {
            world = new World();
            selectedCountry = null;
            rng = new Random();
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Hearts of Gold - Main Menu");
                Console.WriteLine("1. Show countries");
                Console.WriteLine("2. Attack");
                Console.WriteLine("3. Form alliance");
                Console.WriteLine("4. Show map");
                Console.WriteLine("5. Select country");
                Console.WriteLine("6. Train selected country");
                Console.WriteLine("0. Quit");
                Console.Write("Choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        world.PrintAllCountries();
                        Pause();
                        break;
                    case "2":
                        HandleAttack();
                        Pause();
                        break;
                    case "3":
                        HandleAlliance();
                        Pause();
                        break;
                    case "4":
                        PrintMap();
                        Pause();
                        break;
                    case "5":
                        HandleSelect();
                        Pause();
                        break;
                    case "6":
                        HandleTrain();
                        Pause();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void HandleAttack()
        {
            Console.Write("Attacker (exact name): ");
            string attackerName = Console.ReadLine();
            var attacker = world.GetCountryByName(attackerName);
            if (attacker == null)
            {
                Console.WriteLine("Attacker not found.");
                return;
            }

            Console.Write("Defender (exact name): ");
            string defenderName = Console.ReadLine();
            var defender = world.GetCountryByName(defenderName);
            if (defender == null)
            {
                Console.WriteLine("Defender not found.");
                return;
            }

            bool success = attacker.Attack(defender);

            // If attack failed, 10% chance defender counterattacks
            if (!success)
            {
                double roll = rng.NextDouble();
                if (roll < 0.10)
                {
                    Console.WriteLine($"{defender.Name} attempts a counterattack!");
                    bool counterSuccess = defender.Attack(attacker);
                    if (counterSuccess)
                    {
                        Console.WriteLine($"{defender.Name} successfully counter-conquered {attacker.Name}!");
                    }
                    else
                    {
                        Console.WriteLine($"{defender.Name} failed the counterattack.");
                    }
                }
                else
                {
                    Console.WriteLine("No counterattack occurred.");
                }
            }
        }

        private void HandleAlliance()
        {
            Console.Write("Country A (exact name): ");
            string a = Console.ReadLine();
            var cA = world.GetCountryByName(a);
            if (cA == null) { Console.WriteLine("Country A not found."); return; }

            Console.Write("Country B (exact name): ");
            string b = Console.ReadLine();
            var cB = world.GetCountryByName(b);
            if (cB == null) { Console.WriteLine("Country B not found."); return; }

            bool ok = cA.FormAlliance(cB);
            if (ok) Console.WriteLine($"{cA.Name} and {cB.Name} are now allies.");
            else Console.WriteLine("Could not form alliance (maybe already allies or conquered).");
        }

        private void HandleSelect()
        {
            Console.Write("Select country by exact name: ");
            string name = Console.ReadLine();
            var c = world.GetCountryByName(name);
            if (c == null)
            {
                Console.WriteLine("Country not found.");
                selectedCountry = null;
            }
            else
            {
                selectedCountry = c;
                Console.WriteLine($"Selected: {selectedCountry.Name}");
            }
        }

        private void HandleTrain()
        {
            if (selectedCountry == null)
            {
                Console.WriteLine("No country selected. Use option 5 to select a country first.");
                return;
            }

            Console.WriteLine($"Training menu for {selectedCountry.Name} (Eng: {selectedCountry.Energy})");
            Console.WriteLine("1. Train Attack (+5 attack, -5 energy)");
            Console.WriteLine("2. Train Defense (+5 defense, -5 energy)");
            Console.WriteLine("3. Train Energy (+15 energy)");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (selectedCountry.Energy < 5)
                    {
                        Console.WriteLine("Not enough energy to train attack.");
                    }
                    else
                    {
                        selectedCountry.AttackPower += 5;
                        selectedCountry.Energy -= 5;
                        Console.WriteLine($"Trained attack. New attack: {selectedCountry.AttackPower}. Energy: {selectedCountry.Energy}");
                    }
                    break;
                case "2":
                    if (selectedCountry.Energy < 5)
                    {
                        Console.WriteLine("Not enough energy to train defense.");
                    }
                    else
                    {
                        selectedCountry.DefensePower += 5;
                        selectedCountry.Energy -= 5;
                        Console.WriteLine($"Trained defense. New defense: {selectedCountry.DefensePower}. Energy: {selectedCountry.Energy}");
                    }
                    break;
                case "3":
                    selectedCountry.Energy += 15;
                    Console.WriteLine($"Trained energy. Energy: {selectedCountry.Energy}");
                    break;
                default:
                    Console.WriteLine("Invalid training choice.");
                    break;
            }
        }

        private void PrintMap()
        {
        // conquered countries with 'XXX'
            string[] map =
            {
                "+--------------------------------------+",
                "|  NOR     SWE     GER     POL     UK  |",
                "|   *       *       *       *       *  |",
                "|                                      |",
                "|                FRA                   |",
                "+--------------------------------------+"
            };

            foreach (var line in map)
            {
                string output = line;
            
                output = ReplaceShort(output, "SWE", world.GetCountryByName("Sweden"));
                output = ReplaceShort(output, "GER", world.GetCountryByName("Germany"));
                output = ReplaceShort(output, "UK", world.GetCountryByName("United Kingdom"));
                output = ReplaceShort(output, "FRA", world.GetCountryByName("France"));
                output = ReplaceShort(output, "POL", world.GetCountryByName("Poland"));
                output = ReplaceShort(output, "NOR", world.GetCountryByName("Norway"));
                Console.WriteLine(output);
            }
        }

        private string ReplaceShort(string line, string shortCode, Country country)
        {
            if (country == null) return line;
            string label = country.IsConquered ? "XXX" : shortCode;
            return line.Replace(shortCode, label);
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
