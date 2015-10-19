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
        private Vector2 positionHorizontal;
        private Vector2 positionVertical;
        private bool change = false;
        public bool jump = true;
        public int scale = 8; //w przypadku zmiany skalowania przeliczyć wartości w UpdatePosiotion() -> new Rectangle.... (scale 7, +95 ; -45)

        public Penguin(Texture2D Image, Texture2D imageHorizontal, Vector2 position, float speedValue, float gravity) : base(Image, position, speedValue, gravity)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = Image;
        }

        override public void UpdatePosition()
        {
            position += speed;

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


            positionHorizontal = positionVertical = position;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Image = imageHorizontal;
                rectangle = new Rectangle((int)positionHorizontal.X, (int)positionHorizontal.Y - (this.Image.Width / scale) + 85, this.Image.Width/scale, this.Image.Height/scale); //NIE TYKAC WARTOŚCI!!!
                change = true;
            }
            else
            {
                Image = imageVertical;
                rectangle = new Rectangle((int)positionVertical.X, (int)positionVertical.Y- (this.Image.Width / scale) - 40, this.Image.Width/scale, this.Image.Height/scale); // NIE TYKAC WARTOŚCI!!!
                change = false;
            }

        }

        public bool IsOnTopOf(Platform platform)
        {
            return rectangle.Intersects(platform.PlatformRectangle);
        }

        public void PutMeOn(Platform platform)
        {
            position.Y = platform.PlatformRectangle.Y;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
        }
    }
}