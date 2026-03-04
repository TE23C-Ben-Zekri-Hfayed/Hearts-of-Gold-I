using System;
using System.Collections.Generic;

//THIS IS THE WORLD FILE, HERE YOU CHANGE EVERYTHING ABOUT THE COUNTRIES IN THE GAME.

namespace HeartsOfGold
{
    public class World
    {
        public List<Country> Countries { get; } = new List<Country>();

        public World()
        {
            // Add all countries in the world (attack, defense, energy)
            // If you add a new country here and not in-game, also add it to the map in Game.cs, it does NOT auto update!!!!!!!
            AddCountry(new Country("Sweden", 40, 50, 80));
            AddCountry(new Country("Germany", 80, 100, 120));
            AddCountry(new Country("United Kingdom", 70, 90, 100));
            AddCountry(new Country("France", 65, 85, 95));
            AddCountry(new Country("Poland", 45, 55, 70));
            AddCountry(new Country("Norway", 30, 35, 60));
        }

        // Adds a country to the world, but only if it doesn't already exist
        public void AddCountry(Country country)
        {
            // Don't add if the country object is empty
            if (country == null)
            {
                return;
            }

            // Check if a country with this name already exists
            bool alreadyExists = false;

            foreach (Country c in Countries)
            {
                if (c.Name == country.Name)
                {
                    alreadyExists = true;
                }
            }

            // Only add the country if it's not a duplicate
            if (alreadyExists == false)
            {
                Countries.Add(country); // BUG FIX: this line was missing!
            }
        }

        // Finds and returns a country by name, or null if not found
        // Works regardless of capitalisation e.g. "sweden" and "Sweden" both work because honesty it's annoying
        public Country GetCountryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            foreach (Country c in Countries)
            {
                bool sameName = c.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                if (sameName)
                {
                    return c;
                }
            }

            return null;
        }

        // Prints the status of every country in the world WITH the stats (Maybe make it look nicer later but for now it works and is functional so who cares)
        public void PrintAllCountries()
        {
            Console.WriteLine("=== Countries ===");

            foreach (Country c in Countries)
            {
                c.PrintStatus();
            }
        }
    }
}