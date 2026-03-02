using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartsOfGold
{
    public class World
    {
        public List<Country> Countries { get; } = new List<Country>();

        public World()
        {
            // countries (attack, defense, energy)
            //if New country is added here, remember to update Game.cs country selection logic!!!! It does not auto update!!!
            AddCountry(new Country("Sweden", 40, 50, 80));
            AddCountry(new Country("Germany", 80, 100, 120));
            AddCountry(new Country("United Kingdom", 70, 90, 100));
            AddCountry(new Country("France", 65, 85, 95));
            AddCountry(new Country("Poland", 45, 55, 70));
            AddCountry(new Country("Norway", 30, 35, 60));
        }

        public void AddCountry(Country country) //Makes sure to not make duplicates of countries.
        {
            if (country != null)
            {
                bool alreadyExists = false;

                foreach (Country c in Countries)
                {
                    if (c.Name == country.Name)
                    {
                        alreadyExists = true;
                    }
                }

                if (!alreadyExists)
                {
                    // add country
                }
            }
        }

        public Country GetCountryByName(string name) // Checks if the country exists by name + to avoid duplicates. Example ignore Sweden and sweden.
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return Countries.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void PrintAllCountries() // basically just print the countries.
        {
            Console.WriteLine("=== Countries ===");
            foreach (var c in Countries)
                c.PrintStatus();
        }
    }
}
