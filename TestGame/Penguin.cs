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

            //if(jump == true)
            speed.Y += 0.15f;
           // if(position.Y + Image.Height >= 700) jump = false;
           // if (jump == false) speed.Y = 0;
          
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