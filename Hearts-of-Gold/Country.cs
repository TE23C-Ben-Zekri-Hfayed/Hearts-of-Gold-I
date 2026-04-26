using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartsOfGold
{
    public class Country : Entity  // enkelt arv - Country ärver från Entity
    {
        private bool isConquered;
        private List<Country> allies;

        public bool IsConquered
        {
            get { return isConquered; }
            set { isConquered = value; }
        }

        // Constructor calls base (Entity) for the shared stats
        public Country(string name, int attack, int defense, int energy)
            : base(name, attack, defense, energy)
        {
            this.isConquered = false;
            this.allies = new List<Country>();
        }

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

            int attackStrength = this.AttackPower + (this.Energy / 5);
            int defenseStrength = defender.DefensePower + (defender.Energy / 5);

            int allyBonus = defender.allies.Sum(a => a.AttackPower / 10 + a.DefensePower / 20);
            defenseStrength += allyBonus;

            Console.WriteLine(Name + " attacks " + defender.Name + "!");
            Console.WriteLine("Attack strength: " + attackStrength +
                              "  Defense strength: " + defenseStrength +
                              " (ally bonus " + allyBonus + ")");

            if (attackStrength > defenseStrength)
            {
                Console.WriteLine(Name + " has conquered " + defender.Name + "!");
                defender.isConquered = true;
                defender.Energy = 0;

                this.Energy = Math.Max(0, this.Energy - 20);
                this.DefensePower = Math.Max(0, this.DefensePower - (defenseStrength / 20));

                return true;
            }
            else
            {
                Console.WriteLine(Name + " failed to conquer " + defender.Name + ".");

                int attackerLossEnergy = Math.Min(this.Energy, Math.Max(1, defenseStrength / 10));
                this.Energy = Math.Max(0, this.Energy - attackerLossEnergy);

                this.AttackPower = Math.Max(0, this.AttackPower - (defender.DefensePower / 20));
                defender.DefensePower = Math.Max(0, defender.DefensePower - (this.AttackPower / 25));

                return false;
            }
        }

        public IEnumerable<string> GetAlliesNames()
        {
            return allies.Select(a => a.Name);
        }
    }
}