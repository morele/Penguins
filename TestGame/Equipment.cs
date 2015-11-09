using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public class Equipment
    {
        public List<EquipmentItem> Items { get; }

        public Equipment()
        {
            Items = new List<EquipmentItem>();
        }

        public void AddItem(EquipmentItem equipmentItem)
        {
            if (Items.Count == 0)
            {
                equipmentItem.Position = new Vector2(200, 50);
            }
            else
            {
                int lastIndex = Items.Count - 1;
                Vector2 lastPositon = Items[lastIndex].Position;
                lastPositon.X += 70;
                equipmentItem.Position = lastPositon;    
            }
            Items.Add(equipmentItem);
        }

        public void RemoveItem(EquipmentItem equipmentItem)
        {
            Items.Remove(equipmentItem);

            // aktualizacja listy 
            for (int i = 0; i < Items.Count; i++)
            {
                Items.Add(Items[i]);
            }

        }

    }
}
