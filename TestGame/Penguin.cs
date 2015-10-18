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
        private Texture2D imageHorizontal;
        private Texture2D imageVertical;
        private bool change = false;

        public bool jump = true;

        public Penguin(Texture2D Image, Texture2D imageHorizontal, Vector2 position, float speedValue, float gravity) : base(Image, position, speedValue, gravity)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = Image;
        }

        override public void UpdatePosition()
        {
            position += speed;
            
            if(Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Image = imageHorizontal;
                rectangle = new Rectangle((int)position.X, (int)position.Y, this.Image.Width, this.Image.Height);
                change = true;
            }else
            {
                Image = imageVertical;
               // if (change)
               // {
                    rectangle = new Rectangle((int)position.X, (int)position.Y - 90, this.Image.Width, this.Image.Height);
                    change = false;
              //  } else
               //     rectangle = new Rectangle((int)position.X, (int)position.Y, this.Image.Width, this.Image.Height);

            }


             if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Down)) speed.X = speedValue * 2;  else
             if (Keyboard.GetState().IsKeyDown(Keys.Left)  && Keyboard.GetState().IsKeyDown(Keys.Down)) speed.X = -speedValue * 2; else
             if (Keyboard.GetState().IsKeyDown(Keys.Right)) speed.X = speedValue;  else 
             if (Keyboard.GetState().IsKeyDown(Keys.Left))  speed.X = -speedValue; else speed.X = 0;               



            if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false)
            {
                position.Y -= 10;
                speed.Y = -gravitation;
                jump = true;
            }
            speed.Y += 0.15f;

            
        }

        public bool IsOnTopOf(Platform platform)
        {
            return rectangle.Intersects(platform.PlatformRectangle);
        }

        public void PutMeOn(Platform platform)
        {
            // związanie pozycji pingwina z poruszającą się platformą
            position.Y = platform.PlatformRectangle.Y - Const.PENGUIN_HEIGHT;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
        }
    }
}