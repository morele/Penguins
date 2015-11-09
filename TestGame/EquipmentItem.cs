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
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public EquipmentItem(Texture2D texture)
        {
            Texture = texture;
        }
    }
}
