using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartsOfGold
{
    public class Country
    {
        // Private fields
        private string name;
        private int attack;
        private int defense;
        private int energy;
        private bool isConquered;
        private List<Country> allies;

        // Public properties (encapsulation)
        public string Name => name;
        public int AttackPower
        {
            get => attack;
            set => attack = Math.Max(0, value);
        }
        public int DefensePower
        {
            get => defense;
            set => defense = Math.Max(0, value);
        }
        public int Energy
        {
            get => energy;
            set => energy = Math.Clamp(value, 0, 200); // cap to avoid runaway values
        }
        public bool IsConquered => isConquered;

        // Constructor
        public Country(string name, int attack, int defense, int energy)
        {
            this.name = name;
            this.attack = Math.Max(0, attack);
            this.defense = Math.Max(0, defense);
            this.energy = Math.Max(0, energy);
            this.isConquered = false;
            this.allies = new List<Country>(); // start empty
        }

        // Print status
        public void PrintStatus()
        {
            string allyNames = allies.Count == 0 ? "None" : string.Join(", ", allies.Select(a => a.Name));
            Console.WriteLine($"{Name} - Atk: {AttackPower}, Def: {DefensePower}, Eng: {Energy}, Conquered: {IsConquered}. Allies: {allyNames}");
        }

        // Form alliance (two-way)
        public bool FormAlliance(Country other)
        {
            if (other == null) return false;
            if (other == this) return false;
            if (this.isConquered || other.isConquered) return false;
            if (allies.Contains(other)) return false;

            allies.Add(other);
            if (!other.allies.Contains(this))
                other.allies.Add(this);
            return true;
        }

        // Attack another country; returns true if attacker conquered defender
        public bool Attack(Country defender)
        {
            if (defender == null)
            {
                Console.WriteLine("Defender not found.");
                return false;
            }
            if (this.isConquered)
            {
                Console.WriteLine($"{Name} is conquered and cannot attack.");
                return false;
            }
            if (defender.isConquered)
            {
                Console.WriteLine($"{defender.Name} is already conquered.");
                return false;
            }

            // Basic strengths: attack vs defense, energy gives small bonus
            int attackStrength = this.AttackPower + (this.Energy / 5);
            int defenseStrength = defender.DefensePower + (defender.Energy / 5);

            // Allies give a small defense bonus
            int allyBonus = defender.allies.Sum(a => a.AttackPower / 10 + a.DefensePower / 20);
            defenseStrength += allyBonus;

            Console.WriteLine($"{Name} attacks {defender.Name}!");
            Console.WriteLine($"Attack strength: {attackStrength}  Defense strength: {defenseStrength} (ally bonus {allyBonus})");

            if (attackStrength > defenseStrength)
            {
                Console.WriteLine($"{Name} has conquered {defender.Name}!");
                defender.isConquered = true;
                defender.Energy = 0;
                // Attacker takes some cost in energy and loses a bit of defense
                this.Energy = Math.Max(0, this.Energy - 20);
                this.DefensePower = Math.Max(0, this.DefensePower - (defenseStrength / 20));
                return true;
            }
            else
            {
                Console.WriteLine($"{Name} failed to conquer {defender.Name}.");
                // Losses for both: attacker loses energy and some attack, defender loses some defense
                int attackerLossEnergy = Math.Min(this.Energy, Math.Max(1, defenseStrength / 10));
                this.Energy = Math.Max(0, this.Energy - attackerLossEnergy);
                this.AttackPower = Math.Max(0, this.AttackPower - (defender.DefensePower / 20));
                defender.DefensePower = Math.Max(0, defender.DefensePower - (this.AttackPower / 25));
                return false;
            }
        }

        // Expose allies as read-only names (for display)
        public IEnumerable<string> GetAlliesNames()
        {
            return allies.Select(a => a.Name);
        }
    }
}
