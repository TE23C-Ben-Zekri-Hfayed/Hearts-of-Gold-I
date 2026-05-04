using System;
using System.Net.Http;
using System.Text.Json;

namespace HeartsOfGold
{
    public class Game
    {
        private World world;
        private Country selectedCountry;
        private Random rng;
        private bool devModeActive;
        private int landwon = 0;

        public Game()
        {
            world = new World();
            rng = new Random();
        }
        public void Run()
        {
            bool running = true;
            while (running)
            {
                if (landwon == 5)
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations! You have conquered all the land and won the game!");
                    Console.WriteLine("Thanks for playing Hearts of Gold I!");
                    Console.ReadLine();
                    break;
                }

                Console.Clear();
                Console.WriteLine("Hearts of Gold - Main Menu");
                Console.WriteLine("1. Show countries\n2. Attack\n3. Form alliance\n4. Show map\n5. Select country\n6. Train selected country");

                if (devModeActive)
                {
                    Console.WriteLine("----------- DEV MODE -----------");
                    Console.WriteLine("7. Edit selected country stats\n8. Add new country\n9. Toggle conquered status\nD. Delete selected country");
                    Console.WriteLine("--------------------------------");
                }

                Console.WriteLine("Q. Quit");
                Console.Write("Choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": world.PrintAllCountries(); Pause(); break;
                    case "2": HandleAttack(); Pause(); break;
                    case "3": HandleAlliance(); Pause(); break;
                    case "4": PrintMap(); Pause(); break;
                    case "5": HandleSelect(); Pause(); break;
                    case "6": HandleTrain(); Pause(); break;
                    case "I": Showint(); Pause(); break;
                    case "7" when devModeActive: DevEditCountry(); Pause(); break;
                    case "8" when devModeActive: DevAddCountry(); Pause(); break;
                    case "9" when devModeActive: DevToggleConquered(); Pause(); break;
                    case "D": case "d": if (devModeActive) { DevDeleteCountry(); Pause(); } break;
                    case "Q": case "q":
                        Console.WriteLine("You are about to exit the game, are you sure? Yes/No");
                        string answer = Console.ReadLine();
                        if (answer == "Yes" || answer == "yes") running = false;
                        else Pause();
                        break;
                    case "P": case "p":
                        Console.Clear();
                        Console.WriteLine("You're not supposed to see this btw");
                        Console.ReadLine();
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
        private void HandleSelect()
        {
            Console.Write("Select country by exact name: ");
            string name = Console.ReadLine();

            if (name == "Devmode") { devModeActive = true; Console.WriteLine("Developer mode activated."); return; }
            if (name == "DevEXIT") { devModeActive = false; selectedCountry = null; Console.WriteLine("Developer mode deactivated."); return; }

            selectedCountry = world.GetCountryByName(name);
            if (selectedCountry == null) Console.WriteLine("Country not found.");
            else { Console.WriteLine("Selected: " + selectedCountry.Name); ShowCountryIntel(selectedCountry.Name); }
        }
        // Helper: prompt for an int, returns false if blank (keep current) or invalid
        private bool TryReadInt(string prompt, out int result)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) { result = 0; return false; }
            if (!int.TryParse(input, out result)) { Console.WriteLine("  Invalid number, not changed."); return false; }
            return true;
        }
        private void DevEditCountry()
        {
            if (selectedCountry == null) { Console.WriteLine("[DEV] No country selected."); return; }
            Console.WriteLine("[DEV] Editing: " + selectedCountry.Name + " (blank = keep current)\n");

            Console.Write("New name (current: " + selectedCountry.Name + "): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) { selectedCountry.Name = newName; Console.WriteLine("  Name updated to: " + selectedCountry.Name); }

            if (TryReadInt("New attack power (current: " + selectedCountry.AttackPower + "): ", out int atk))
                { selectedCountry.AttackPower = atk; Console.WriteLine("  Attack updated to: " + atk); }

            if (TryReadInt("New defense power (current: " + selectedCountry.DefensePower + "): ", out int def))
                { selectedCountry.DefensePower = def; Console.WriteLine("  Defense updated to: " + def); }

            if (TryReadInt("New energy (current: " + selectedCountry.Energy + "): ", out int eng))
                { selectedCountry.Energy = eng; Console.WriteLine("  Energy updated to: " + eng); }

            Console.WriteLine("\n[DEV] Done editing " + selectedCountry.Name + ".");
        }
        private void DevAddCountry()
        {
            Console.WriteLine("[DEV] Add a new country.");
            Console.Write("Country name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("[DEV] Name cannot be empty. Cancelled."); return; }
            if (world.GetCountryByName(name) != null) { Console.WriteLine("[DEV] Country already exists. Cancelled."); return; }

            if (!TryReadInt("Attack power: ", out int attack) ||
                !TryReadInt("Defense power: ", out int defense) ||
                !TryReadInt("Energy: ", out int energy))
            {
                Console.WriteLine("[DEV] Invalid value. Cancelled.");
                return;
            }

            world.AddCountry(new Country(name, attack, defense, energy));
            Console.WriteLine("[DEV] " + name + " has been added to the world.");
        }
        private void DevToggleConquered()
        {
            if (selectedCountry == null) { Console.WriteLine("[DEV] No country selected."); return; }
            selectedCountry.IsConquered = !selectedCountry.IsConquered;
            Console.WriteLine("[DEV] " + selectedCountry.Name + " marked as " + (selectedCountry.IsConquered ? "CONQUERED" : "FREE") + ".");
        }
        private void DevDeleteCountry()
        {
            if (selectedCountry == null) { Console.WriteLine("[DEV] No country selected."); return; }
            Console.Write("[DEV] Are you sure you want to delete " + selectedCountry.Name + "? (yes/no): ");
            if (Console.ReadLine() == "yes")
            {
                Console.WriteLine("[DEV] " + selectedCountry.Name + " removed.");
                world.Countries.Remove(selectedCountry);
                selectedCountry = null;
            }
            else Console.WriteLine("[DEV] Deletion cancelled.");
        }
        private void Showint()
        {

            Console.Clear();
            Console.WriteLine($"You have {landwon} ");
            Console.ReadLine();


        }
        private void HandleAttack()
        {
            Console.Write("Attacker (exact name): ");
            Country attacker = world.GetCountryByName(Console.ReadLine());
            if (attacker == null) { Console.WriteLine("Attacker not found."); return; }

            Console.Write("Defender (exact name): ");
            string defenderName = Console.ReadLine();
            if (defenderName == attacker.Name) { Console.WriteLine("You can't attack your own country!"); return; }

            Country defender = world.GetCountryByName(defenderName);
            if (defender == null) { Console.WriteLine("Defender not found."); return; }

            landwon++;

            if (!attacker.Attack(defender) && rng.NextDouble() < 0.10)
            {
                Console.WriteLine(defender.Name + " attempts a counterattack!");
                if (defender.Attack(attacker))
                {
                    Console.WriteLine(defender.Name + " successfully counter-conquered " + attacker.Name + "!");
                }
                else Console.WriteLine(defender.Name + " failed the counterattack.");
            }
            else Console.WriteLine("No counterattack occurred.");
        }
        private void HandleAlliance()
        {
            Console.Write("Country A (exact name): ");
            Country countryA = world.GetCountryByName(Console.ReadLine());
            if (countryA == null) { Console.WriteLine("Country A not found."); return; }

            Console.Write("Country B (exact name): ");
            Country countryB = world.GetCountryByName(Console.ReadLine());
            if (countryB == null) { Console.WriteLine("Country B not found."); return; }

            Console.WriteLine(countryA.FormAlliance(countryB)
                ? countryA.Name + " and " + countryB.Name + " are now allies."
                : "Could not form alliance (maybe already allies or conquered).");
        }
        private void HandleTrain()
        {
            if (selectedCountry == null) { Console.WriteLine("No country selected. Use option 5 first."); return; }
            Console.WriteLine("Training menu for " + selectedCountry.Name + " (Energy: " + selectedCountry.Energy + ")");
            Console.WriteLine("1. Train Attack  (+5 attack,  -5 energy)\n2. Train Defense (+5 defense, -5 energy)\n3. Train Energy  (+15 energy)");
            Console.Write("Choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    if (selectedCountry.Energy < 5) Console.WriteLine("Not enough energy.");
                    else { selectedCountry.AttackPower += 5; selectedCountry.Energy -= 5; Console.WriteLine("Attack: " + selectedCountry.AttackPower + " | Energy: " + selectedCountry.Energy); }
                    break;
                case "2":
                    if (selectedCountry.Energy < 5) Console.WriteLine("Not enough energy.");
                    else { selectedCountry.DefensePower += 5; selectedCountry.Energy -= 5; Console.WriteLine("Defense: " + selectedCountry.DefensePower + " | Energy: " + selectedCountry.Energy); }
                    break;
                case "3":
                    selectedCountry.Energy += 15;
                    Console.WriteLine("Energy: " + selectedCountry.Energy);
                    break;
                default:
                    Console.WriteLine("Invalid training choice.");
                    break;
            }
        }
        private void PrintMap()
        {
            string[] map = {
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
                foreach (var (code, cName) in new[] { ("SWE","Sweden"),("GER","Germany"),("UK","United Kingdom"),("FRA","France"),("POL","Poland"),("NOR","Norway") })
                    output = ReplaceShort(output, code, world.GetCountryByName(cName));
                Console.WriteLine(output);
            }
        }
        private string ReplaceShort(string line, string shortCode, Country country)
        {
            if (country == null) return line;
            return line.Replace(shortCode, country.IsConquered ? "XXX" : shortCode);
        }
        private void ShowCountryIntel(string countryName)
        {
            HttpClient client = new HttpClient();
            string json = client.GetStringAsync("https://restcountries.com/v3.1/name/" + countryName + "?fullText=true").Result;
            JsonElement country = JsonDocument.Parse(json).RootElement[0];

            Console.WriteLine("--- INTEL REPORT ---");
            Console.WriteLine("Capital: " + country.GetProperty("capital")[0].GetString());
            Console.WriteLine("Population: " + country.GetProperty("population").GetInt64());
            Console.WriteLine("Region: " + country.GetProperty("subregion").GetString());
            Console.WriteLine("--------------------");
        }
        private void Pause()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}