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
            AddCountry(new Country("Sweden", 40, 50, 80));
            AddCountry(new Country("Germany", 80, 100, 120));
            AddCountry(new Country("United Kingdom", 70, 90, 100));
            AddCountry(new Country("France", 65, 85, 95));
            AddCountry(new Country("Poland", 45, 55, 70));
            AddCountry(new Country("Norway", 30, 35, 60));
        }

        public void AddCountry(Country country)
        {
            if (country != null && !Countries.Any(c => c.Name.Equals(country.Name, StringComparison.OrdinalIgnoreCase)))
                Countries.Add(country);
        }

        public Country GetCountryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return Countries.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void PrintAllCountries()
        {
            Console.WriteLine("=== Countries ===");
            foreach (var c in Countries)
                c.PrintStatus();
        }
    }
}
