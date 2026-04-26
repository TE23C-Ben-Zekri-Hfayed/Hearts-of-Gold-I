using System;

namespace HeartsOfGold
{
    // Superpower is a stronger country, it inherits from Country 
    public class Superpower : Country
    {
        private string powerName; // Superpower title, "Empire", change maybe??

        public string PowerName
        {
            get { return powerName; }
            set { powerName = value; }
        }

        public Superpower(string name, int attack, int defense, int energy, string powerName)
            : base(name, attack, defense, energy)
        {
            this.powerName = powerName;
        }

      
        public override string GetDescription()
        {
            return Name + " is a superpower (" + powerName + ") with " + AttackPower + " attack and " + DefensePower + " defense.";
        }

    
        public override bool Attack(Country defender)
        {
            Console.WriteLine("[" + powerName + "] " + Name + " unleashes a superpower strike!");
            AttackPower = (int)(AttackPower * 1.25);
            bool result = base.Attack(defender);
            AttackPower = (int)(AttackPower / 1.25);
            return result;
        }
    }
}