using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public class Equipment
    {
        public List<Item> Items { get; }

        public Equipment()
        {
            Items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (Items.Count == 0)
            {
                item.Position = new Vector2(200, 50);
            }
            else
            {
                int lastIndex = Items.Count - 1;
                Vector2 lastPositon = Items[lastIndex].Position;
                lastPositon.X += 70;
                item.Position = lastPositon;    
            }
            Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);

            // aktualizacja listy 
            for (int i = 0; i < Items.Count; i++)
            {
                Items.Add(Items[i]);
            }

        }

    }
}
