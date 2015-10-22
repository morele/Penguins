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

        public Texture2D Avatar { get; private set; }
        private Texture2D imageHorizontal;
        private Texture2D imageVertical;

        private Vector2 positionHorizontal;
        private Vector2 positionVertical;

        public bool jump = true;
        public bool firstStart = true;
        public bool active = true;
        public int scale = 8; 
        public int platformSpeed = 0;
        public PenguinType penguinType;

        public int pinguinVertical = 0;
        public int pinguinHorizontal = 0;

        public Penguin(Texture2D Image, Texture2D imageHorizontal, Texture2D avatar, Vector2 position, float speedValue, float gravity, PenguinType penguinType) : base(Image, position, speedValue, gravity)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = Image;
            Avatar = avatar;
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
           
            if(active)
            {
                position += speed;
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

                speed.Y += 0.15f;
            
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
            else
            {
                position += speed;
                speed.Y += 0.15f;

                positionVertical = position;
                Image = imageVertical;
                rectangle = new Rectangle((int)positionVertical.X, (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); //jak stoi

            }


        }

        /// <summary>
        /// Metoda sprawdza czy pingwin znajduje się platformie
        /// </summary>
        /// <param name="platform">Platforma do sprawdzenia</param>
        /// <returns>True - gdy pingwin znajduje się na platformie
        ///          False - gdy pingwin nie znajduje się na platformie</returns>
        public bool IsOnTopOf(Platform platform)
        {
            int penguinWidth = rectangle.Width;
            int penguinHeight = rectangle.Height;
            int penguinX = rectangle.X;
            int penguinY = rectangle.Y;

            // sprawdzenie czy pingwin mieści się na platformie (w orientacji osi X)
            if (penguinX > (platform.PlatformRectangle.X - penguinWidth) && 
                 penguinX < (platform.PlatformRectangle.X + platform.PlatformRectangle.Width))

                if((penguinY + penguinHeight) > (platform.PlatformRectangle.Y) &&
                    (penguinY + penguinHeight) < (platform.PlatformRectangle.Y + platform.PlatformRectangle.Height/3))
            {

                    return true;
            }
            return false;
        }
        /// <summary>
        /// Wykrywa kolizje z innym przedmiotem(np pingwinem)
        /// </summary>
        /// <param name="r1">Obszar w którym znajduje się przedmiot</param>
        /// <returns>True - gdy pingwin zderzył się z przedmiotem 
        ///          False - gdy pingwin nie zderzył się z przedmiotem</returns>
        public bool Collision(Rectangle r1)
        {
           return rectangle.Intersects(r1);
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