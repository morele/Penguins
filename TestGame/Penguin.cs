using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace TestGame
{
    public class Penguin : TextureManager
    {
        public Rectangle rectangle;
        public Vector2 speed;
        public bool jump = true;

        public Penguin(Texture2D Image, Vector2 position, float speedValue, float gravity) : base(Image, position, speedValue, gravity) { }

        override public void UpdatePosition()
        {
            position += speed;
            rectangle = new Rectangle((int)position.X, (int)position.Y,this.Image.Width, this.Image.Height);

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) speed.X = speedValue;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) speed.X = -speedValue; else speed.X = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false)
            {
                position.Y -= 10;
                speed.Y = -gravitation;
                jump = true;         
            }

            speed.Y += 0.15f;
         
        }
        public void UpdatePositionRelativePlatform(float positionY)
        {
            positionY -= 197; // ponieważ taką ma wysokość pingwin
            position.Y = positionY;
        }
        public bool isOnTopOf(Rectangle r2)
        {
            return (rectangle.Bottom >= r2.Top &&
                    rectangle.Bottom < r2.Top + r2.Height &&
                    rectangle.Right  >= r2.Left &&
                    rectangle.Left   <= r2.Right);
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
        }
    }
}