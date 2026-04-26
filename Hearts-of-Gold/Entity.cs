using System;

namespace HeartsOfGold
{
    public class Entity
    {
        private string name;
        private int attack;
        private int defense;
        private int energy;

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
            set { energy = Math.Clamp(value, 0, 200); }
        }

        public Entity(string name, int attack, int defense, int energy)
        {
            this.name = name;
            this.attack = Math.Max(0, attack);
            this.defense = Math.Max(0, defense);
            this.energy = Math.Max(0, energy);
        }

        // Virtual – kan overridas av subklasser
        public virtual string GetDescription()
        {
            return Name + " is a standard entity.";
        }
    }
}