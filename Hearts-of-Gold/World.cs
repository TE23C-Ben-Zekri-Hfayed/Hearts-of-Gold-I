using System;
using System.Collections.Generic;

namespace HeartsOfGold
{
    public class World
    {
        // Använder generisk GameRegistry istället för List direkt
        private GameRegistry<Country> registry = new GameRegistry<Country>();

        // Kept for compatibility with Game.cs
        public List<Country> Countries
        {
            get { return registry.GetAll(); }
        }

        public World()
        {
            // Germany and United Kingdom are Superpowers – starkare länder
            AddCountry(new Country("Sweden", 40, 50, 80));
            AddCountry(new Superpower("Germany", 80, 100, 120, "Empire"));
            AddCountry(new Superpower("United Kingdom", 70, 90, 100, "Commonwealth"));
            AddCountry(new Country("France", 65, 85, 95));
            AddCountry(new Country("Poland", 45, 55, 70));
            AddCountry(new Country("Norway", 30, 35, 60));
        }

        public void AddCountry(Country country)
        {
            if (country == null)
            {
                return;
            }

            bool alreadyExists = false;

            foreach (Country c in registry.GetAll())
            {
                if (c.Name == country.Name)
                {
                    alreadyExists = true;
                }
            }

            if (alreadyExists == false) // easy to understand duh 
            {
                registry.Add(country);
            }
        }

        public Country GetCountryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            foreach (Country c in registry.GetAll())
            {
                bool sameName = c.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                if (sameName)
                {
                    return c;
                }
            }

            return null;
        }

        public void PrintAllCountries()
        {
            Console.WriteLine("=== Countries ===");

            foreach (Country c in registry.GetAll())
            {
                c.PrintStatus();
                // Visar beskrivningen – använder polymorfism (GetDescription)
                Console.WriteLine("  > " + c.GetDescription());
            }
        }
    }
}