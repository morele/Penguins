using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public class Equipment
    {
        public List<EquipmentItem> Items { get; private set; }

        public Equipment()
        {
            Items = new List<EquipmentItem>();
        }

        public void AddItem(EquipmentItem equipmentItem)
        {
            if (Items.Count == 0)
            {
                equipmentItem.Item.Position = new Point(200, 50);
            }
            else
            {
                int lastIndex = Items.Count - 1;
                Point lastPositon = Items[lastIndex].Item.Position;
                lastPositon.X += 70;
                equipmentItem.Item.Position = lastPositon;    
            }
            Items.Add(equipmentItem);
        }

        public void RemoveItem(EquipmentItem equipmentItem)
        {
            Items.Remove(equipmentItem);
        }

    }
}
