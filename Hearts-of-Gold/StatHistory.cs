using System;
using System.Collections.Generic;

namespace HeartsOfGold
{
    // Basically saves history of a value over time, how a country's attack has changed and suuch
    public class StatHistory<T>
    {
        private string statName;
        private List<T> history = new List<T>();

        public StatHistory(string statName)
        {
            this.statName = statName;
        }

        public void Record(T value)
        {
            history.Add(value);
        }

        public void PrintHistory()
        {
            Console.WriteLine("--- " + statName + " history ---");

            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine("  " + (i + 1) + ": " + history[i]);
            }

            if (history.Count == 0)
            {
                Console.WriteLine("  No history yet.");
            }
        }

        public T GetLatest()
        {
            if (history.Count == 0)
            {
                return default(T);
            }
            return history[history.Count - 1];
        }
    }
}