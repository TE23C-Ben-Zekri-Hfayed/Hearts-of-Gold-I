using System;

namespace HeartsOfGold
{
    // This class controls the main game loop and handles player input
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

                // Check which option the player typed and run the right action
                if (input == "1")
                {
                    world.PrintAllCountries();
                    Pause();
                }
                else if (input == "2")
                {
                    HandleAttack();
                    Pause();
                }
                else if (input == "3")
                {
                    HandleAlliance();
                    Pause();
                }
                else if (input == "4")
                {
                    PrintMap();
                    Pause();
                }
                else if (input == "5")
                {
                    HandleSelect();
                    Pause();
                }
                else if (input == "6")
                {
                    HandleTrain();
                    Pause();
                }
                else if (input == "0")
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                    Pause();
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void HandleAttack()
        {
            // Ask the player for the name of the attacking country
            Console.Write("Attacker (exact name): ");
            string attackerName = Console.ReadLine();
            Country attacker = world.GetCountryByName(attackerName);

            if (attacker == null)
            {
                Console.WriteLine("Attacker not found.");
                return;
            }

            // Ask the player for the name of the defending country
            Console.Write("Defender (exact name): ");
            string defenderName = Console.ReadLine();
            Country defender = world.GetCountryByName(defenderName);

            if (defender == null)
            {
                Console.WriteLine("Defender not found.");
                return;
            }

            // Run the attack and store whether it succeeded
            bool attackSucceeded = attacker.Attack(defender);

            // If the attack failed, there is a 10% chance the defender strikes back
            if (attackSucceeded == false)
            {
                double counterChance = rng.NextDouble(); // gives a number between 0.0 and 1.0
                bool counterattackHappens = counterChance < 0.10;

                if (counterattackHappens)
                {
                    Console.WriteLine(defender.Name + " attempts a counterattack!");
                    bool counterSucceeded = defender.Attack(attacker);

                    if (counterSucceeded)
                    {
                        Console.WriteLine(defender.Name + " successfully counter-conquered " + attacker.Name + "!");
                    }
                    else
                    {
                        Console.WriteLine(defender.Name + " failed the counterattack.");
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
            // Ask for the two countries that want to form an alliance
            Console.Write("Country A (exact name): ");
            string nameA = Console.ReadLine();
            Country countryA = world.GetCountryByName(nameA);

            if (countryA == null)
            {
                Console.WriteLine("Country A not found.");
                return;
            }

            Console.Write("Country B (exact name): ");
            string nameB = Console.ReadLine();
            Country countryB = world.GetCountryByName(nameB);

            if (countryB == null)
            {
                Console.WriteLine("Country B not found.");
                return;
            }

            // Try to form the alliance
            bool allianceFormed = countryA.FormAlliance(countryB);

            if (allianceFormed)
            {
                Console.WriteLine(countryA.Name + " and " + countryB.Name + " are now allies.");
            }
            else
            {
                Console.WriteLine("Could not form alliance (maybe already allies or conquered).");
            }
        }

        private void HandleSelect()
        {
            Console.Write("Select country by exact name: ");
            string name = Console.ReadLine();
            Country found = world.GetCountryByName(name);

            if (found == null)
            {
                Console.WriteLine("Country not found.");
                selectedCountry = null;
            }
            else
            {
                selectedCountry = found;
                Console.WriteLine("Selected: " + selectedCountry.Name);
            }
        }

        private void HandleTrain()
        {
            // Make sure a country has been selected first
            if (selectedCountry == null)
            {
                Console.WriteLine("No country selected. Use option 5 to select a country first.");
                return;
            }

            Console.WriteLine("Training menu for " + selectedCountry.Name + " (Energy: " + selectedCountry.Energy + ")");
            Console.WriteLine("1. Train Attack  (+5 attack,  -5 energy)");
            Console.WriteLine("2. Train Defense (+5 defense, -5 energy)");
            Console.WriteLine("3. Train Energy  (+15 energy)");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                // Check the country has enough energy before training
                if (selectedCountry.Energy < 5)
                {
                    Console.WriteLine("Not enough energy to train attack.");
                }
                else
                {
                    selectedCountry.AttackPower += 5;
                    selectedCountry.Energy -= 5;
                    Console.WriteLine("Trained attack. New attack: " + selectedCountry.AttackPower + ". Energy: " + selectedCountry.Energy);
                }
            }
            else if (choice == "2")
            {
                if (selectedCountry.Energy < 5)
                {
                    Console.WriteLine("Not enough energy to train defense.");
                }
                else
                {
                    selectedCountry.DefensePower += 5;
                    selectedCountry.Energy -= 5;
                    Console.WriteLine("Trained defense. New defense: " + selectedCountry.DefensePower + ". Energy: " + selectedCountry.Energy);
                }
            }
            else if (choice == "3")
            {
                selectedCountry.Energy += 15;
                Console.WriteLine("Trained energy. Energy: " + selectedCountry.Energy);
            }
            else
            {
                Console.WriteLine("Invalid training choice.");
            }
        }

        private void PrintMap()
        {
            // Draw a simple text map, replacing country codes with XXX if conquered
            string[] map =
            {
                "+--------------------------------------+",
                "|  NOR     SWE     GER     POL     UK  |",
                "|   *       *       *       *       *  |",
                "|                                      |",
                "|                FRA                   |",
                "+--------------------------------------+"
            };

            foreach (string line in map)
            {
                string output = line;

                output = ReplaceShort(output, "SWE", world.GetCountryByName("Sweden"));
                output = ReplaceShort(output, "GER", world.GetCountryByName("Germany"));
                output = ReplaceShort(output, "UK",  world.GetCountryByName("United Kingdom"));
                output = ReplaceShort(output, "FRA", world.GetCountryByName("France"));
                output = ReplaceShort(output, "POL", world.GetCountryByName("Poland"));
                output = ReplaceShort(output, "NOR", world.GetCountryByName("Norway"));

                Console.WriteLine(output);
            }
        }

        // Replaces a short country code on the map with XXX if that country has been conquered
        private string ReplaceShort(string line, string shortCode, Country country)
        {
            if (country == null)
            {
                return line;
            }

            string label;

            if (country.IsConquered)
            {
                label = "XXX";
            }
            else
            {
                label = shortCode;
            }

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