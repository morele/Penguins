using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using TestGame.Interfaces;

namespace TestGame
{
    public class Penguin : TextureManager, IGravitable
    {
        public Rectangle rectangle;
        public Vector2 speed;

        public int Mass { get; private set; }

        public Equipment Equipment { get; }
        public bool CanMove { get; set; }
        public Texture2D Avatar { get; private set; }
        private Texture2D imageHorizontal;
        private Texture2D imageVertical;

        private Rectangle _positionOnSheet;
        private int _PositionOnSheetX = 0;

        public List<Rectangle> currentdimensionsPenguin = new List<Rectangle>();
        private Vector2 positionHorizontal;
        private Vector2 positionVertical;
        private Vector2 tmpPosition = new Vector2();
        private Platform blockPlatform = new Platform();

        public bool jump = true;
        public bool firstStart = true;
        public bool active = true;
        private bool activeDirection = true; // true = prawo, false = lewo
        private bool blockDircetionLEFT = false;
        private bool blockDirectionRIGHT = false;
        private bool block = false;
        private bool activeSpace = false;
        public int scale = 8;
        public int platformSpeed = 0;
        public PenguinType penguinType;
        public List<Platform> platforms = new List<Platform>();

        public int pinguinVertical = 0;
        public int pinguinHorizontal = 0;

        private double _frameDuration;
        private double _frameDelay;


        public Penguin(Texture2D image, Texture2D imageHorizontal, Texture2D avatar, Vector2 position, float speedValue, float gravity, PenguinType penguinType, int mass) : base(image, position, speedValue, gravity)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = image;
            Avatar = avatar;
            Mass = mass;
            this.penguinType = penguinType;
            Equipment = new Equipment();



            Texture = image;
            Position = position.ToPoint();
            Size = new Point(image.Width / scale, image.Height / scale);
            CanMove = true;

            //każdy typ pingwina ma róźną wysokość, wartości odpowiednio przeskalowane 
            switch (penguinType)
            {
                case PenguinType.KOWALSKI:
                    pinguinVertical = Const.PINGUIN_KOWALSKI_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_KOWALSKI_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.KOWALSKI);
                    break;
                case PenguinType.RICO:
                    pinguinVertical = Const.PINGUIN_RICO_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_RICO_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.RICO);
                    break;
                case PenguinType.SZEREGOWY:
                    pinguinVertical = Const.PINGUIN_SZEREGOWY_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_SZEREGOWY_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SZEREGOWY);
                    break;
                case PenguinType.SKIPPER:
                    pinguinHorizontal = Const.PINGUIN_SKIPPER_HORIZONTAL;
                    pinguinVertical = Const.PINGUIN_SKIPPER_VERTICAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SKIPPER);
                    break;

            }
            // przeskalowanie wymiarów
            for (int i = 0; i < dimensionsPenguin.Count; i++)
            {
                dimensionsPenguin[i] = ReScale(dimensionsPenguin[i], scale);
            }
            currentdimensionsPenguin = dimensionsPenguin;
        }

        /// <summary>
        /// Animacja
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageHorizontal"></param>
        /// <param name="avatar"></param>
        /// <param name="position"></param>
        /// <param name="speedValue"></param>
        /// <param name="gravity"></param>
        /// <param name="penguinType"></param>
        /// <param name="mass"></param>
        /// <param name="frameSize">Argument potrzebny do Animacji</param>
        public Penguin(Texture2D image, Texture2D imageHorizontal, Texture2D avatar, Vector2 position, float speedValue, float gravity, PenguinType penguinType, int mass, Point frameSize) :
            base(image, position, speedValue, gravity, frameSize)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = image;
            Avatar = avatar;
            Mass = mass;
            this.penguinType = penguinType;
            Equipment = new Equipment();

            Texture = image;
            Position = position.ToPoint();

            _positionOnSheet = new Rectangle(1, 1, frameSize.X, frameSize.Y);//Ł.G;
            _frameDuration = 0;
            _frameDelay = 300;
        

            Size = new Point(image.Width / 16, image.Width / scale);

            CanMove = true;

            //każdy typ pingwina ma róźną wysokość, wartości odpowiednio przeskalowane 
            switch (penguinType)
            {
                case PenguinType.KOWALSKI:
                    pinguinVertical = Const.PINGUIN_KOWALSKI_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_KOWALSKI_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.KOWALSKI);
                    break;
                case PenguinType.RICO:
                    pinguinVertical = Const.PINGUIN_RICO_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_RICO_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.RICO);
                    break;
                case PenguinType.SZEREGOWY:
                    pinguinVertical = Const.PINGUIN_SZEREGOWY_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_SZEREGOWY_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SZEREGOWY);
                    break;
                case PenguinType.SKIPPER:
                    pinguinHorizontal = Const.PINGUIN_SKIPPER_HORIZONTAL;
                    pinguinVertical = Const.PINGUIN_SKIPPER_VERTICAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SKIPPER);
                    break;

            }
            // przeskalowanie wymiarów
            for (int i = 0; i < dimensionsPenguin.Count; i++)
            {
                dimensionsPenguin[i] = ReScale(dimensionsPenguin[i], scale);
            }
            currentdimensionsPenguin = dimensionsPenguin;
        }
        override public void UpdatePosition()
        {

            if (active)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.D5) && penguinType == PenguinType.RICO)
                {
                    if (Equipment.Items.Count > 0)
                    {
                        var lastItem = Equipment.Items.Last();
                        lastItem.Item.IsActive = true;
                        lastItem.Item.Position = new Point(Position.X + Size.X, Position.Y - Size.Y - 30);
                        Equipment.RemoveItem(lastItem);
                    }
                }
                Position += speed.ToPoint();

                if (penguinType == PenguinType.RICO)
                {
                    if (_frameDuration >= _frameDelay)
                    {
                        
                    }
                    if (_PositionOnSheetX >= 8)
                    {
                        _PositionOnSheetX = 0;
                    }
                    _positionOnSheet = new Rectangle(new Point(480 * _PositionOnSheetX, 0), this.FrameSize);
             
                }
              

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    speed.X = speedValue * 2;

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    speed.X = -speedValue * 2;

                }
                else
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !blockDirectionRIGHT)
                {
                    speed.X = speedValue;
                    activeDirection = true;
                    if (penguinType == PenguinType.RICO)
                    {
                        _PositionOnSheetX++;
                    }


                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !blockDircetionLEFT)
                {
                    speed.X = -speedValue;
                    activeDirection = false;

                }
                else
                {
                    speed.X = 0;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    activeSpace = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false && !activeSpace)
                {
                    Jump();
                    activeSpace = true;
                }


                FallDown();


                positionHorizontal = positionVertical = Position.ToVector2();
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
                Position += speed.ToPoint();
                speed.Y += 0.15f;

                positionVertical = Position.ToVector2();
                Image = imageVertical;
                rectangle = new Rectangle((int)positionVertical.X, (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); //jak stoi

            }

            //currentdimensionsPenguin.Clear();//czyszczenie listy
            //currentdimensionsPenguin.TrimExcess(); //zwalnianie pamieci
            currentdimensionsPenguin = UpdateDimensions(rectangle);
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

            if (penguinType == PenguinType.SZEREGOWY && (currentdimensionsPenguin[1].Intersects(platform.PlatformRectangle) ||
                currentdimensionsPenguin[4].Intersects(platform.PlatformRectangle)))//jak kolizja po prawej stronie
            {
                Position.X -= 5;
            }




            if (currentdimensionsPenguin[3].Intersects(platform.PlatformRectangle))//jak kolizja po lewej stronie
                blockDircetionLEFT = true;
            else blockDircetionLEFT = false;

            if (currentdimensionsPenguin[2].Intersects(platform.PlatformRectangle))//jak dotknie nogami
                return true;

            if (currentdimensionsPenguin[0].Intersects(platform.PlatformRectangle)) //jak wyskoczy 
                Position.Y = platform.PlatformRectangle.Y + platform.PlatformRectangle.Height;

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
                (rectangle.Y + rectangle.Height) <= (r1.Y + r1.Height))
                return true;

            if (rectangle.X <= (r1.X + r1.Width) &&
                rectangle.X >= r1.X &&
               (rectangle.Y + rectangle.Height) >= r1.Y &&
               (rectangle.Y + rectangle.Height) <= (r1.Y + r1.Height))
                return true;

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

            if (colUD) colRL = false; //jeśli pingwin lata to nie ma kolizji na lewo i prawo
            if (colRL && block == false)
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
                blockDircetionLEFT = blockDirectionRIGHT = false;
            }

            if (activeDirection == true && block)//jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie 
            {
                if ((rectangle.X) > tmpPosition.X + speedValue)
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
            if (!block)//jak nie zablokowane to odblokuj oba kierunki 
            {
                blockDircetionLEFT = blockDirectionRIGHT = false;
            }

            if (colRL || colUD) return true;

            return false;
        }

        public void PutMeOn(Rectangle platform)
        {
            Position.Y = platform.Y;
        }
        public void PutMeOn(float newPosition)
        {
            Position.Y = (int)newPosition;
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
        /// <summary>
        /// Odmiana Draw do Animacji
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            rectangle.Width /= 8;
            spriteBatch.Draw(Image, rectangle, _positionOnSheet, Color.White);

        }
        public override string ToString()
        {
            return penguinType.ToString();
        }

        public void FallDown()
        {
            float a = Mass / Const.GRAVITY;
            speed.Y += 0.25f;
        }

        public void Jump()
        {
            speed.Y = -Const.GRAVITY;
            jump = true;
            Position.Y -= (int)Math.Pow(Const.GRAVITY * 3, 2) / Mass;
        }
    }
}