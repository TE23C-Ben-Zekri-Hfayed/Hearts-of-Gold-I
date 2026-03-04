using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartsOfGold
{
    public class Country
    {
        // Private fields (data stored inside the object)
        private string name;
        private int attack;
        private int defense;
        private int energy;
        private bool isConquered;
        private List<Country> allies;

        // Public properties (controlled access to data)

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int AttackPower
        {
            get { return attack; }
            set { attack = Math.Max(0, value); }
        }

        public int DefensePower
        {
            get { return defense; }
            set { defense = Math.Max(0, value); }
        }

        public int Energy
        {
            get { return energy; }
            set { energy = Math.Clamp(value, 0, 200); } // Energy is limited between 0 and 200
        }

        public bool IsConquered
        {
            get { return isConquered; }
            set { isConquered = value; }
        }

        // Constructor – runs when a new Country is created
        public Country(string name, int attack, int defense, int energy)
        {
            this.name = name;
            this.attack = Math.Max(0, attack);
            this.defense = Math.Max(0, defense);
            this.energy = Math.Max(0, energy);
            this.isConquered = false;
            this.allies = new List<Country>(); // Starts with no allies
        }

        // Print information about the country
        public void PrintStatus()
        {
            string allyNames;

            if (allies.Count == 0)
            {
                allyNames = "None";
            }
            else
            {
                allyNames = string.Join(", ", allies.Select(a => a.Name));
            }

            Console.WriteLine(Name + " - Atk: " + AttackPower +
                              ", Def: " + DefensePower +
                              ", Eng: " + Energy +
                              ", Conquered: " + IsConquered +
                              ". Allies: " + allyNames);
        }

        // Create an alliance between two countries (both sides get added)
        public bool FormAlliance(Country other)
        {
            if (other == null) return false;
            if (other == this) return false;
            if (this.isConquered || other.isConquered) return false;
            if (allies.Contains(other)) return false;

            allies.Add(other);

            if (!other.allies.Contains(this))
            {
                other.allies.Add(this);
            }

            return true;
        }

        // Attack another country
        // Returns true if the defender was conquered
        public bool Attack(Country defender)
        {
            if (defender == null)
            {
                Console.WriteLine("Defender not found.");
                return false;
            }

            if (this.isConquered)
            {
                Console.WriteLine(Name + " is conquered and cannot attack.");
                return false;
            }

            if (defender.isConquered)
            {
                Console.WriteLine(defender.Name + " is already conquered.");
                return false;
            }

            // Base strength calculation
            int attackStrength = this.AttackPower + (this.Energy / 5);
            int defenseStrength = defender.DefensePower + (defender.Energy / 5);

            // Allies give the defender extra strength
            int allyBonus = defender.allies.Sum(a => a.AttackPower / 10 + a.DefensePower / 20);
            defenseStrength += allyBonus;

            Console.WriteLine(Name + " attacks " + defender.Name + "!");
            Console.WriteLine("Attack strength: " + attackStrength +
                              "  Defense strength: " + defenseStrength +
                              " (ally bonus " + allyBonus + ")");

            // If attacker wins
            if (attackStrength > defenseStrength)
            {
                Console.WriteLine(Name + " has conquered " + defender.Name + "!");
                defender.isConquered = true;
                defender.Energy = 0;

                // Attacker loses some energy and defense
                this.Energy = Math.Max(0, this.Energy - 20);
                this.DefensePower = Math.Max(0, this.DefensePower - (defenseStrength / 20));

                return true;
            }
            else
            {
                // If attacker fails
                Console.WriteLine(Name + " failed to conquer " + defender.Name + ".");

                int attackerLossEnergy = Math.Min(this.Energy, Math.Max(1, defenseStrength / 10));
                this.Energy = Math.Max(0, this.Energy - attackerLossEnergy);

                this.AttackPower = Math.Max(0, this.AttackPower - (defender.DefensePower / 20));
                defender.DefensePower = Math.Max(0, defender.DefensePower - (this.AttackPower / 25));

                return false;
            }
        }

        // Return names of all allies
        public IEnumerable<string> GetAlliesNames()
        {
            return allies.Select(a => a.Name);
        }
    }
}