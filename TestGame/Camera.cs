using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame
{
    public class Camera
    {
        public Matrix transform;
        public Vector2 centre;
        public bool Yaxis;

        public Camera(bool Yaxis = false)
        {
            this.Yaxis = Yaxis;
        }

        public void Update(Penguin player)
        {
            if(Yaxis) centre = new Vector2(player.rectangle.X + (player.rectangle.Width / 2) - 600, player.rectangle.Y + (player.rectangle.Height / 2 - 450)); else
                      centre = new Vector2(player.rectangle.X + (player.rectangle.Width / 2) - 600, 0);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }
    }
}
