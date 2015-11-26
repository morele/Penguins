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
        public EquipmentItem SelectedItem { get; set; }
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
        public bool blockDircetionLEFT = false;
        public  bool blockDirectionRIGHT = false;
        private bool blockDircetionDOWN = false;
        private int correctPosition = 0;
        private bool blockVomit;
        private bool block = false;
        private bool activeSpace = false;
        public int scale = 8; 
        public int platformSpeed = 0;
        public PenguinType penguinType;
        public List<Platform> platforms = new List<Platform>();
        private Rectangle actualCollisionRect;

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
            image = new Texture2D(image.GraphicsDevice,image.Width/4,image.Height);

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

                if (Keyboard.GetState().IsKeyDown(Keys.D5) && penguinType == PenguinType.RICO && !blockVomit)
                {
                    if (Equipment.Items.Count > 0)
                    {
                        var vomitItem = SelectedItem;
                        vomitItem.Item.IsActive = true;
                        vomitItem.Item.Position = new Point(Position.X + Size.X, Position.Y - Size.Y - 30);
                        Equipment.RemoveItem(vomitItem);
                        blockVomit = true;

                    }
                    }

                if (Keyboard.GetState().IsKeyUp(Keys.D5))
                {
                    blockVomit = false;
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

                if (Keyboard.GetState().IsKeyUp(Keys.Space)) activeSpace = false;    //zeby nacisnięcie spacji oznaczało tylko JEDNO nacisniecie(szybko odświeża)           
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false && !activeSpace && !blockDircetionDOWN)
                {
                    Jump();
                    activeSpace = true;
                    blockDircetionDOWN = true;
                }
                    

                FallDown();
            

                positionHorizontal = positionVertical = Position.ToVector2();
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    Image = imageHorizontal;
                    rectangle = new Rectangle((int)positionHorizontal.X + correctPosition, (int)positionHorizontal.Y - (this.Image.Width / scale) + (pinguinHorizontal + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); // na slizgu
                }
                else
                {
                    Image = imageVertical;
                    rectangle = new Rectangle((int)positionVertical.X + correctPosition, (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); //jak stoi
                }


            }
            else
            {
                speed.X = 0;
                Position += speed.ToPoint();
                speed.Y += 0.15f;

                positionVertical = Position.ToVector2();
                Image = imageVertical;
                rectangle = new Rectangle((int)positionVertical.X + correctPosition, (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed), this.Image.Width / scale, this.Image.Height / scale); //jak stoi

            }

            currentdimensionsPenguin = UpdateDimensions(rectangle);
        }

        private void blockSystem()
        {
            if (!block)//jezeli nie jest zablokowane
            {
                tmpPosition = rectangle.Location.ToVector2(); //zapamietaj aktualna pozycje 
                block = true;

                if (activeDirection) //prawo
                {
                    blockDirectionRIGHT = true;
                    blockDircetionLEFT = false;
                    correctPosition = (int)-speedValue;
            }
                if (!activeDirection) //lewo
                {
                    blockDircetionLEFT = true;
                    blockDirectionRIGHT = false;
                    correctPosition = (int)speedValue;
        }
            }
        }

        /// <summary>
        /// Wykrywa kolizje pingwina z parametrem
        /// </summary>
        /// <param name="platform">Platforma do sprawdzenia</param>
        /// <returns>True - gdy pingwin znajduje się na platformie
        ///          False - gdy pingwin nie znajduje się na platformie</returns>
        public bool Collision(Rectangle r1, PlatformType platformType = PlatformType.NN)
        {
            if (actualCollisionRect != null) //jak pingwin lata to musi mu sie odlokowac mozliwosc poruszania sie na lewo i prawo
                if (!currentdimensionsPenguin[0].Intersects(actualCollisionRect) &&
                   !currentdimensionsPenguin[1].Intersects(actualCollisionRect) &&
                   !currentdimensionsPenguin[2].Intersects(actualCollisionRect) &&
                   !currentdimensionsPenguin[3].Intersects(actualCollisionRect) &&
                   !currentdimensionsPenguin[4].Intersects(actualCollisionRect))
                    blockDircetionLEFT = blockDirectionRIGHT = false;

            if ((currentdimensionsPenguin[1].Intersects(r1) || currentdimensionsPenguin[4].Intersects(r1)))//jak kolizja po prawej stronie
            {
                actualCollisionRect = r1;
                blockSystem();
            }


            if (currentdimensionsPenguin[3].Intersects(r1))//jak kolizja po lewej stronie
            {
                actualCollisionRect = r1;
                blockSystem();
            }

            if (currentdimensionsPenguin[0].Intersects(r1)) //jak dotknie głowa
            {
                if (blockDircetionDOWN) speed.Y = 0;
                blockDircetionDOWN = false;
            }

            if (activeDirection && block)//jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie 
                if (rectangle.X > tmpPosition.X + speedValue)
                {
                    correctPosition = 0;
                    block = false;
                }

            if (!activeDirection && block)//jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie 
                if (rectangle.X < tmpPosition.X - speedValue)
                {
                    correctPosition = 0;
                    block = false;
                }

            if (!block)//jak nie zablokowane to odblokuj oba kierunki 
                blockDircetionLEFT = blockDirectionRIGHT = false;


            if (platformType == PlatformType.FLOOR)//jak podloga to powoduje zeby pingwin nie mogl sie zaczepic od boku 
            {
                r1.Height = (int)speed.Y + 1;
                if (currentdimensionsPenguin[2].Intersects(r1))//jak dotknie nogami
                {
                    blockDircetionDOWN = false;
                    return true;
                }
            }
            else
            if (currentdimensionsPenguin[2].Intersects(r1))//jak dotknie nogami
            {
                blockDircetionDOWN = false;
                return true;
            }


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
            if (speed.Y < currentdimensionsPenguin[2].Height * 0.75) speed.Y += 0.25f; //ograniczenie zeby pingwin stawał na platformie a nie w jej połowie
        }

        public void Jump()
        {
            speed.Y = -Const.GRAVITY;
            jump = true;
            Position.Y -= (int)Math.Pow(Const.GRAVITY * 3, 2) / Mass;
        }
    }
}