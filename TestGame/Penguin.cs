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

        public bool jump = true;
        public bool firstStart = true;
        public bool active = true;
        public int scale = 8; 
        public int platformSpeed = 0;
        private PenguinType penguinType;

        public int pinguinVertical = 0;
        public int pinguinHorizontal = 0;

        public Penguin(Texture2D Image, Texture2D imageHorizontal, Vector2 position, float speedValue, float gravity, PenguinType penguinType) : base(Image, position, speedValue, gravity)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = Image;
            this.penguinType = penguinType;

            //każdy typ pingwina ma róźną wysokość, wartości odpowiednio przeskalowane 
            switch(penguinType)
            {
                case PenguinType.KOWALSKI:
                    pinguinVertical = Const.PINGUIN_KOWALSKI_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_KOWALSKI_HORIZONTAL;
                break;
                case PenguinType.RICO:
                    pinguinVertical = Const.PINGUIN_RICO_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_RICO_HORIZONTAL;
                    break;
                case PenguinType.SZEREGOWY:
                    pinguinVertical = Const.PINGUIN_SZEREGOWY_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_SZEREGOWY_HORIZONTAL;
                    break;
                case PenguinType.SKIPPER:
                    pinguinHorizontal = Const.PINGUIN_SKIPPER_HORIZONTAL;
                    pinguinVertical = Const.PINGUIN_SKIPPER_VERTICAL;
                    break;

            }
        }

        override public void UpdatePosition()
        {
            position += speed;
            if(active)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Down)) speed.X = speedValue * 2; else
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Down)) speed.X = -speedValue * 2; else
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) speed.X = speedValue; else
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) speed.X = -speedValue; else speed.X = 0;

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false)
                {
                    position.Y -= 10;
                    speed.Y = -gravitation;
                    jump = true;
                }
            }

            speed.Y += 0.15f;

            if(active)
            {
                positionHorizontal = positionVertical = position;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    Image = imageHorizontal;
                    rectangle = new Rectangle((int)positionHorizontal.X, (int)positionHorizontal.Y - (this.Image.Width / scale) + (pinguinHorizontal + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); // na slizgu
                }
                else
                {
                    Image = imageVertical;
                    rectangle = new Rectangle((int)positionVertical.X, (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); //jak stoi
                }
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
        public void JumpStop(int platformSpeed)
        {
            speed.Y = 0f;
            jump = false;
            this.platformSpeed = platformSpeed;
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
        }
    }
}