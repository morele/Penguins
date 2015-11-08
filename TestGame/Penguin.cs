using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace TestGame
{
    public class Penguin : TextureManager
    {
        public Rectangle rectangle;
        public Vector2 speed;
        private float collision = 1;

        public Equipment Equipment { get; private set; }    

        public Texture2D Avatar { get; private set; }
        private Texture2D imageHorizontal;
        private Texture2D imageVertical;

        private Vector2 positionHorizontal;
        private Vector2 positionVertical;
        private Vector2 tmpPosition = new Vector2();

        public bool jump = true;
        public bool firstStart = true;
        public bool active = true;
        private bool activeDirection = true; // true = prawo, false = lewo
        private bool blockDircetionLEFT = false;
        private bool blockDirectionRIGHT = false;
        private bool block = false;
        public int scale = 8; 
        public int platformSpeed = 0;
        public PenguinType penguinType;
        public List<Platform> platforms = new List<Platform>();

        public int pinguinVertical = 0;
        public int pinguinHorizontal = 0;

        public Penguin(Texture2D Image, Texture2D imageHorizontal, Texture2D avatar, Vector2 position, float speedValue, float gravity, PenguinType penguinType) : base(Image, position, speedValue, gravity)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = Image;
            Avatar = avatar;
            this.penguinType = penguinType;
            Equipment = new Equipment();

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

                if (Keyboard.GetState().IsKeyDown(Keys.D5) && penguinType == PenguinType.RICO)
                {
                    if(platforms.Count > 0)
                    {
                        platforms[0].ResetMoney(rectangle); //ustawia monete względem pingwina i resetuje parametry lotu monety
                        platforms[0].initJump = true;//inicjalizuje lot monety 
                        platforms[0].active = true; //aktywuje monete by mogla być rysowana
                        platforms.RemoveAt(0); //usuwa monete z pingwina
                        Equipment.Items.Clear();
                    }
                }

                position += speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Down)) speed.X = speedValue * 2;
                else
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Down)) speed.X = -speedValue * 2;
                else
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !blockDirectionRIGHT)
                {
                    speed.X = speedValue;
                    activeDirection = true;
                }
                else
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && !blockDircetionLEFT)
                {
                    speed.X = -speedValue;
                    activeDirection = false;
                }
                else speed.X = 0;

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
                speed.X = 0;
                position += speed;
                speed.Y += 0.15f;

                positionVertical = position;
                Image = imageVertical;
                rectangle = new Rectangle((int)positionVertical.X, (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); //jak stoi

            }

            if(platforms.Count > 0)
            if (platforms[0].platformType == PlatformType.MONEY )
            {
                    if (platforms[0].jump == false && platforms[0].initJump == false)
                    {
                        platforms[0].position.X = rectangle.X;
                        platforms[0].position.Y = rectangle.Y;
                    }
                    
            }

        }

        /// <summary>
        /// Wykrywa kolizje z podana platforma
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CollisionPlatform(Platform r1, PlatformType type)
        {
            if (rectangle.Intersects(r1.PlatformRectangle) && r1.platformType == type && penguinType == PenguinType.RICO)
            {
                platforms.Add(r1);
                return true;
            }
            return false;
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
                   (penguinY + penguinHeight) < (platform.PlatformRectangle.Y + platform.PlatformRectangle.Height))
                {

                        return true;
                }
            return false;
        }
       /// <summary>
       /// Wykrywa kolizje po prawej i lewej stronie pingwina
       /// </summary>
       /// <param name="r1"></param>
       /// <returns></returns>
        private bool CollisionRL(Rectangle r1)
        {

            if ((rectangle.X + rectangle.Width) >= r1.X && 
                (rectangle.X + rectangle.Width) <= (r1.X + r1.Width) &&
                (rectangle.Y + rectangle.Height) >= r1.Y && 
                (rectangle.Y + rectangle.Height) <= (r1.Y + r1.Height)) return true;

            if (rectangle.X <= (r1.X + r1.Width) && 
                rectangle.X >= r1.X &&
               (rectangle.Y + rectangle.Height) >= r1.Y && 
               (rectangle.Y + rectangle.Height) <= (r1.Y + r1.Height)) return true;

            return false;
        }

        /// <summary>
        /// Wykrywa kolizcje z dołu pingwina (w momencie kiedy jeden na drugiego wskoczy)
        /// </summary>
        /// <param name="r1"></param>
        /// <returns></returns>
        private bool CollisionUD(Rectangle r1)
        {
            if (CollisionRL(r1) && (rectangle.Y + rectangle.Height) >= r1.Y && (rectangle.Y + rectangle.Height) <= r1.Y + 10) return true;
            
            return false;
        }
        ///
        /// <summary>
        /// Wykrywa kolizje z innym pingwinem i blokuje w przypadku wykrycia
        /// </summary>
        /// <param name="r1">Obszar w którym znajduje się pingwin</param>
        /// <returns>True - gdy pingwin zderzył się z pingwinem
        ///          False - gdy pingwin nie zderzył się z pingwinem</returns>
        public bool CollisionPenguin(Rectangle r1)
        {
            bool colRL, colUD;

            
            colRL = CollisionRL(r1); //wykrywa kolizje po lewej i prawej stronie pingwina
            colUD = CollisionUD(r1); //wykrywa kolizje pod pingwinem 

            if(colUD) colRL = false; //jeśli pingwin lata to nie ma kolizji na lewo i prawo
            if(colRL && block == false) 
            {
                tmpPosition.X = rectangle.X; //zapamietaj aktualna pozycje 
                block = true;

                if (activeDirection == true) //prawo
                {
                    blockDirectionRIGHT = true;
                    blockDircetionLEFT = false;
                }
                if (activeDirection == false) //lewo
                {
                    blockDircetionLEFT = true;
                    blockDirectionRIGHT = false;
                }

            }

            if (colUD) //jak lata i ma kolizje to zatrzymaj
            {
                JumpStop(0);
                PutMeOn(r1.Y - 1);
            }
            
            if (activeDirection == true && block)//jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie 
            {
                if((rectangle.X) > tmpPosition.X + speedValue)
                {
                    block = false;
                }
            }
            if (activeDirection == false && block)//jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie 
            {
                if ((rectangle.X) < tmpPosition.X - speedValue)
                {
                    block = false;
                }
            }
            if(!block)//jak nie zablokowane to odblokuj oba kierunki 
            {
                blockDircetionLEFT = blockDirectionRIGHT = false;
            }

            if (colRL || colUD) return true;

            return false;
        }

        public void PutMeOn(Rectangle platform)
        {
            position.Y = platform.Y;
        }
        public void PutMeOn(float newPosition)
        {
            position.Y = newPosition;
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

        public override string ToString()
        {
            return penguinType.ToString();
        }
    }
}