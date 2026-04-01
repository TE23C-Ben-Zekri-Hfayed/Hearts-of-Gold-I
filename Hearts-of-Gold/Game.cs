using System;
using System.Collections.Concurrent;

//THIS IS THE GAME FILE, EVERYTHING THAT A PLAYER CAN DO IN THE GAME IS HANDLED HERE, THIS IS THE BIG ONE. ALSO THE DEV MODE OPTIONS ARE ALL HERE, BUT THEY ARE HIDDEN UNLESS YOU KNOW THE SECRET CODE (line 128 btw) TO TURN DEV MODE ON (AND OFF).

namespace HeartsOfGold
{
    public class Game
    {
        private World world;
        private Country selectedCountry;
        private Random rng;

        // Tracks whether developer mode is currently active
        private bool devModeActive;
        int landwon = 0;

        public Game()
        {
            world = new World();
            selectedCountry = null;
            rng = new Random();
            devModeActive = false;
            landwon = 0;
        }


        public void Run()
        {
            bool running = true;

            while (running)
            {

                if (landwon == 5)
                {
                    Console.WriteLine("Congratulations! You have conquered all the land and won the game!");
                    Console.WriteLine("Thanks for playing Hearts of Gold I!");
                    Console.ReadLine();
                    running = false;
                }
                Console.Clear();
                Console.WriteLine("Hearts of Gold - Main Menu");
                Console.WriteLine("1. Show countries");
                Console.WriteLine("2. Attack");
                Console.WriteLine("3. Form alliance");
                Console.WriteLine("4. Show map");
                Console.WriteLine("5. Select country");
                Console.WriteLine("6. Train selected country");

                // Only show dev options if dev mode is on
                if (devModeActive)
                {
                    Console.WriteLine("----------- DEV MODE -----------");
                    Console.WriteLine("7. Edit selected country stats");
                    Console.WriteLine("8. Add new country");
                    Console.WriteLine("9. Toggle conquered status");
                    Console.WriteLine("D. Delete selected country");
                    Console.WriteLine("--------------------------------");
                }

                Console.WriteLine("Q. Quit");
                Console.Write("Choice: ");

                string input = Console.ReadLine();

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
                else if (input == "7" && devModeActive)
                {
                    DevEditCountry();
                    Pause();
                }
                else if (input == "8" && devModeActive)
                {
                    DevAddCountry();
                    Pause();
                }
                else if (input == "9" && devModeActive)
                {
                    DevToggleConquered();
                    Pause();
                }
                else if ((input == "D" || input == "d") && devModeActive)
                {
                    DevDeleteCountry();
                    Pause();
                }
                else if (input == "Q" || input == "q")
                {

                    Console.WriteLine("You are about to exit the game, are you sure? Yes/No"); // Quit function

                    string answer = Console.ReadLine();

                    if (answer == "Yes" || answer == "yes")
                    {
                        running = false;
                    }
                    else if (answer == "No" || answer == null || answer == "no")
                    {
                        Pause();
                    }


                }
                else if (input == "P" || input == "p")
                {
                    Console.Clear();
                    Console.WriteLine("You're not supposed to see this btw");
                    Console.ReadLine();
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

        // -------------------------------------------------------
        // HANDLE SELECT 
        // -------------------------------------------------------
        private void HandleSelect()
        {
            Console.Write("Select country by exact name: ");
            string name = Console.ReadLine();

            // Secret code to turn dev mode ON
            if (name == "Devmode")
            {
                devModeActive = true;
                Console.WriteLine("Developer mode activated. New options are now visible in the menu.");
                return;
            }

            // Secret code to turn dev mode OFF
            if (name == "DevEXIT")
            {
                devModeActive = false;
                Console.WriteLine("Developer mode deactivated.");
                selectedCountry = null;
                return;
            }

            // Normal country selection
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

        // -------------------------------------------------------
        // DEV OPTION 7 — Edit all stats of the selected country
        // -------------------------------------------------------
        private void DevEditCountry()
        {
            if (selectedCountry == null)
            {
                Console.WriteLine("[DEV] No country selected. Use option 5 first.");
                return;
            }

            Console.WriteLine("[DEV] Editing: " + selectedCountry.Name);
            Console.WriteLine("      Leave a field blank and press Enter to keep the current value.");
            Console.WriteLine();

            // Edit name
            Console.Write("New name (current: " + selectedCountry.Name + "): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                selectedCountry.Name = newName;
                Console.WriteLine("  Name updated to: " + selectedCountry.Name);
            }

            // Edit attack
            Console.Write("New attack power (current: " + selectedCountry.AttackPower + "): ");
            string attackInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(attackInput))
            {
                int newAttack;
                bool attackValid = int.TryParse(attackInput, out newAttack);
                if (attackValid)
                {
                    selectedCountry.AttackPower = newAttack;
                    Console.WriteLine("  Attack updated to: " + selectedCountry.AttackPower);
                }
                else
                {
                    Console.WriteLine("  Invalid number, attack not changed.");
                }
            }

            // Edit defense
            Console.Write("New defense power (current: " + selectedCountry.DefensePower + "): ");
            string defenseInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(defenseInput))
            {
                int newDefense;
                bool defenseValid = int.TryParse(defenseInput, out newDefense);
                if (defenseValid)
                {
                    selectedCountry.DefensePower = newDefense;
                    Console.WriteLine("  Defense updated to: " + selectedCountry.DefensePower);
                }
                else
                {
                    Console.WriteLine("  Invalid number, defense not changed.");
                }
            }

            // Edit energy
            Console.Write("New energy (current: " + selectedCountry.Energy + "): ");
            string energyInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(energyInput))
            {
                int newEnergy;
                bool energyValid = int.TryParse(energyInput, out newEnergy);
                if (energyValid)
                {
                    selectedCountry.Energy = newEnergy;
                    Console.WriteLine("  Energy updated to: " + selectedCountry.Energy);
                }
                else
                {
                    Console.WriteLine("  Invalid number, energy not changed.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("[DEV] Done editing " + selectedCountry.Name + ".");
        }

        // -------------------------------------------------------
        // DEV OPTION 8 — Add a brand new country to the world
        // -------------------------------------------------------
        private void DevAddCountry()
        {
            Console.WriteLine("[DEV] Add a new country.");

            Console.Write("Country name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("[DEV] Name cannot be empty. Cancelled.");
                return;
            }

            if (world.GetCountryByName(name) != null)
            {
                Console.WriteLine("[DEV] A country with that name already exists. Cancelled.");
                return;
            }

            Console.Write("Attack power: ");
            string attackInput = Console.ReadLine();
            int attack;
            bool attackValid = int.TryParse(attackInput, out attack);
            if (!attackValid)
            {
                Console.WriteLine("[DEV] Invalid attack value. Cancelled.");
                return;
            }

            Console.Write("Defense power: ");
            string defenseInput = Console.ReadLine();
            int defense;
            bool defenseValid = int.TryParse(defenseInput, out defense);
            if (!defenseValid)
            {
                Console.WriteLine("[DEV] Invalid defense value. Cancelled.");
                return;
            }

            Console.Write("Energy: ");
            string energyInput = Console.ReadLine();
            int energy;
            bool energyValid = int.TryParse(energyInput, out energy);
            if (!energyValid)
            {
                Console.WriteLine("[DEV] Invalid energy value. Cancelled.");
                return;
            }

            Country newCountry = new Country(name, attack, defense, energy);
            world.AddCountry(newCountry);
            Console.WriteLine("[DEV] " + name + " has been added to the world.");
        }

        // -------------------------------------------------------
        // DEV OPTION 9 — Flip a country's conquered status
        // -------------------------------------------------------
        private void DevToggleConquered()
        {
            if (selectedCountry == null)
            {
                Console.WriteLine("[DEV] No country selected. Use option 5 first.");
                return;
            }

            selectedCountry.IsConquered = !selectedCountry.IsConquered;

            if (selectedCountry.IsConquered)
            {
                Console.WriteLine("[DEV] " + selectedCountry.Name + " has been marked as CONQUERED.");
            }
            else
            {
                Console.WriteLine("[DEV] " + selectedCountry.Name + " has been marked as FREE.");
            }
        }

        // -------------------------------------------------------
        // DEV OPTION D — Delete the selected country entirely
        // -------------------------------------------------------
        private void DevDeleteCountry()
        {
            if (selectedCountry == null)
            {
                Console.WriteLine("[DEV] No country selected. Use option 5 first.");
                return;
            }

            Console.Write("[DEV] Are you sure you want to delete " + selectedCountry.Name + "? (yes/no): ");
            string confirm = Console.ReadLine();

            if (confirm == "yes")
            {
                world.Countries.Remove(selectedCountry);
                Console.WriteLine("[DEV] " + selectedCountry.Name + " has been removed from the world.");
                selectedCountry = null;
            }
            else
            {
                Console.WriteLine("[DEV] Deletion cancelled.");
            }
        }

        // -------------------------------------------------------
        // All original methods below — unchanged
        // -------------------------------------------------------

        private void HandleAttack()
        {
            Console.Write("Attacker (exact name): ");
            string attackerName = Console.ReadLine();
            Country attacker = world.GetCountryByName(attackerName);

            if (attacker == null)
            {
                Console.WriteLine("Attacker not found.");
                return;
            }

            Console.Write("Defender (exact name): ");
            string defenderName = Console.ReadLine();
            Country defender = world.GetCountryByName(defenderName);

            if (defender == null)
            {
                Console.WriteLine("Defender not found.");
                return;
            }

            if (defenderName == attackerName) //Added for anti attacking yourself
            {
                Console.WriteLine("You can't attack your own country! Choose different countries.");
                return;
            }

            bool attackSucceeded = attacker.Attack(defender);

            if (attackSucceeded == false)
            {
                double counterChance = rng.NextDouble();
                bool counterattackHappens = counterChance < 0.10;

                if (counterattackHappens)
                {
                    Console.WriteLine(defender.Name + " attempts a counterattack!");
                    bool counterSucceeded = defender.Attack(attacker);

                    if (counterSucceeded)
                    {
                        Console.WriteLine(defender.Name + " successfully counter-conquered " + attacker.Name + "!");
                        landwon++;
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

        private void HandleTrain()
        {
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
                output = ReplaceShort(output, "UK", world.GetCountryByName("United Kingdom"));
                output = ReplaceShort(output, "FRA", world.GetCountryByName("France"));
                output = ReplaceShort(output, "POL", world.GetCountryByName("Poland"));
                output = ReplaceShort(output, "NOR", world.GetCountryByName("Norway"));

                Console.WriteLine(output);
            }
        }

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