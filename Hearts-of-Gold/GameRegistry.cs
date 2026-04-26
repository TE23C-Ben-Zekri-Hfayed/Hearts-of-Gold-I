using System;
using System.Collections.Generic;

namespace HeartsOfGold
{
    // Generic collection that can hold any type of thing
    public class GameRegistry<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        public T Get(int index)
        {
            return items[index];
        }

        public int Count()
        {
            return items.Count;
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public List<T> GetAll()
        {
            return items;
        }
    }
}