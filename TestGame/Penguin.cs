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
        public void UpdatePositionRelativePlatform(float positionY)
        {
            positionY -= 197; // ponieważ taką ma wysokość pingwin
            position.Y = positionY;
        }

        public bool IsOnTopOf(Platform platform)
        {
            Rectangle r2 = platform.PlatformRectangle;
            return (rectangle.Bottom >= r2.Top &&
                    rectangle.Bottom < r2.Top + r2.Height &&
                    rectangle.Right  >= r2.Left &&
                    rectangle.Left   <= r2.Right);
        }

        public void PutMeOn(Platform platform)
        {
            position.Y = platform.PlatformRectangle.Y - 197;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
        }
    }
}