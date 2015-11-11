using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public class EquipmentItem
    {
        public GameObject Item { get; set; }

        public EquipmentItem(GameObject gameObject)
        {
            Item = gameObject;
        }
    }
}
