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

        private Vector2 speed;
        bool jump = false;

        public Penguin(Texture2D Image, Vector2 position, float speedValue, float gravity) : base(Image, position, speedValue, gravity) { }

        override public void UpdatePosition()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) speed.X = speedValue;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) speed.X = -speedValue; else speed.X = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false)
            {
                position.Y -= 10;
                speed.Y = -gravitation;
                jump = true;         
            }

            if(jump == true) speed.Y += (float)0.15;

            if(position.Y + Image.Height >= 650) jump = false;

            if (jump == false) speed.Y = 0;

            position += speed;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, position, Color.White);
        }
    }
}