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
        public event EventHandler<EventArgs> PenguinDeathByFallingHandler;

        protected void OnPenguinDeathByFalling(PenguinType penguinType)
        {
            var tempHandler = PenguinDeathByFallingHandler;
            if (tempHandler != null)
            {
                tempHandler(this, EventArgs.Empty);
            }
        }


        public Rectangle rectangle;
        public Vector2 speed;

        public int Mass { get; private set; }

        public Equipment Equipment { get; private set; }
        public EquipmentItem SelectedItem { get; set; }
        public bool CanMove { get; set; }
        public Texture2D Avatar { get; private set; }
        private Texture2D imageHorizontal;
        private Texture2D imageVertical;
        private Rectangle _positionOnSheet;
        private int _positionOnSheetX = 1;


        public List<Rectangle> currentdimensionsPenguin = new List<Rectangle>();
        public List<Rectangle> currentdimensionsPenguinShoe = new List<Rectangle>();
        private Vector2 positionHorizontal;
        private Vector2 positionVertical;
        private Vector2 tmpPosition = new Vector2();
        private Platform blockPlatform = new Platform();
        private bool shoe = false;
        public bool jump = true;
        public bool firstStart = true;
        public bool active = true;
        public bool inMove = false;
        private bool activeDirection = true; // true = prawo, false = lewo
        public bool blockDircetionLEFT = false;
        public bool blockDirectionRIGHT = false;
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


        private readonly Vector2 _startPositionOfPenguin;

        private bool _left;

        private SpriteEffects _flipEffect;


        private Animation _animationHorizontal;
        private Animation _animationVertival;

        private bool _slide;


        public Penguin(Texture2D image, Texture2D imageHorizontal, Texture2D avatar, Vector2 position, float speedValue, float gravity, PenguinType penguinType, int mass, Point frameSize) :
            base(image, position, speedValue, gravity, frameSize)
        {
            this.imageHorizontal = imageHorizontal;
            this.imageVertical = image;
            Avatar = avatar;
            Mass = mass;
            this.penguinType = penguinType;
            Equipment = new Equipment();




            _startPositionOfPenguin = position;

            _animationVertival = new Animation(this.imageVertical, 8, 50, _startPositionOfPenguin);

            _animationHorizontal = new Animation(this.imageHorizontal, 14, 50, _startPositionOfPenguin);


            Texture = image;
            //   image = new Texture2D(image.GraphicsDevice, image.Width / 4, image.Height);

            Position = position.ToPoint();



            this.PenguinDeathByFallingHandler += Penguin_PenguinDeathByFallingHandler;

            Size = new Point(image.Width / 16, image.Width / scale);

            CanMove = true;

            //każdy typ pingwina ma róźną wysokość, wartości odpowiednio przeskalowane
            switch (penguinType)
            {
                case PenguinType.KOWALSKI:
                    pinguinVertical = Const.PINGUIN_KOWALSKI_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_KOWALSKI_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.KOWALSKI);
                    dimensionsPenguinShoe = Const.DimensionsPenguinShoe(PenguinType.KOWALSKI);
                    break;
                case PenguinType.RICO:
                    pinguinVertical = Const.PINGUIN_RICO_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_RICO_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.RICO);
                    dimensionsPenguinShoe = Const.DimensionsPenguinShoe(PenguinType.RICO);
                    break;
                case PenguinType.SZEREGOWY:
                    pinguinVertical = Const.PINGUIN_SZEREGOWY_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_SZEREGOWY_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SZEREGOWY);
                    dimensionsPenguinShoe = Const.DimensionsPenguinShoe(PenguinType.SZEREGOWY);
                    break;
                case PenguinType.SKIPPER:
                    pinguinHorizontal = Const.PINGUIN_SKIPPER_HORIZONTAL;
                    pinguinVertical = Const.PINGUIN_SKIPPER_VERTICAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SKIPPER);
                    dimensionsPenguinShoe = Const.DimensionsPenguinShoe(PenguinType.SKIPPER);
                    break;

            }
            // przeskalowanie wymiarów
            for (int i = 0; i < dimensionsPenguin.Count; i++)
                dimensionsPenguin[i] = ReScale(dimensionsPenguin[i], scale);
            for (int i = 0; i < dimensionsPenguinShoe.Count; i++)
                dimensionsPenguinShoe[i] = ReScale(dimensionsPenguinShoe[i], scale);

            //na poczatek ustawiamy rectangle dla stojacego pingwina
            currentdimensionsPenguin = dimensionsPenguin;
        }

        private void Penguin_PenguinDeathByFallingHandler(object sender, EventArgs e)
        {
            Position = _startPositionOfPenguin.ToPoint();

        }



        /// <summary>
        /// w metodzie update animuje postac w zaleznosci od typu pingwina
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="penguinType"></param>
        private void UpdateAnimation(GameTime gametime, Rectangle newRectangle)
        {
            if (active && inMove)
            {
                switch (penguinType)
                {
                    case PenguinType.RICO:
                        {
                            _animationVertival.Update(gametime, newRectangle);
                        }
                        break;
                    case PenguinType.KOWALSKI:
                        {
                            _animationVertival.Update(gametime, newRectangle);
                        }
                        break;
                    case PenguinType.SKIPPER:
                        {
                            _animationVertival.Update(gametime, newRectangle);
                        }
                        break;
                    case PenguinType.SZEREGOWY:
                        {
                            _animationVertival.Update(gametime, newRectangle);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _animationVertival.UpdateInStay(newRectangle);
            }

        }
        public Rectangle RecalculateRectangle(Rectangle currentRectangle,Point correct)
        {
            return  new Rectangle(currentRectangle.X+ correct.X, currentRectangle.Y + correct.Y,
                                currentRectangle.Width, currentRectangle.Height);
        }
        private void UpdateAnimationSlide(GameTime gametime, Rectangle newRectangle)
        {
            if (active && inMove)
            {
                switch (penguinType)
                {
                    case PenguinType.RICO:
                        {
                            

                            _animationHorizontal.Update(gametime, RecalculateRectangle(newRectangle,new Point(0,52)));
                        }
                        break;
                    case PenguinType.KOWALSKI:
                        {
                            _animationHorizontal.Update(gametime, newRectangle);
                        }
                        break;
                    case PenguinType.SKIPPER:
                        {
                            _animationHorizontal.Update(gametime, newRectangle);
                        }
                        break;
                    case PenguinType.SZEREGOWY:
                        {
                            _animationHorizontal.Update(gametime, newRectangle);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _animationHorizontal.UpdateInStay(newRectangle);
            }

        }

        KeyboardState keyboard;
        public bool activeKeysDown = false;
        override public void UpdatePosition(GameTime gametime)
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

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    speed.X = speedValue * 5;
                    _slide = true;
                    _left = false;
                    inMove = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    speed.X = -speedValue * 2;
                    _slide = true;
                    _left = true;
                    inMove = true;
                }
                else
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !blockDirectionRIGHT && (!Keyboard.GetState().IsKeyDown(Keys.Down)))
                {
                    speed.X = speedValue;
                    activeDirection = true;
                    _left = false;
                    inMove = true;
                    _slide = false;

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !blockDircetionLEFT && (!Keyboard.GetState().IsKeyDown(Keys.Down)))
                {
                    speed.X = -speedValue;
                    activeDirection = false;
                    _left = true;
                    inMove = true;
                    _slide = false;
                }
                else
                {
                    speed.X = 0;
                    inMove = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Space)) activeSpace = false; //zeby nacisnięcie spacji oznaczało tylko JEDNO nacisniecie(szybko odświeża)          
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false && !activeSpace && !blockDircetionDOWN)
                {
                    Jump();
                    activeSpace = true;
                    blockDircetionDOWN = true;
                }


                FallDown();


                positionHorizontal = positionVertical = Position.ToVector2();


                if (Keyboard.GetState().IsKeyUp(Keys.Down)) activeKeysDown = false;
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !activeKeysDown)
                {
                    shoe = true;
                    activeKeysDown = true;
                    Image = imageHorizontal;
                    rectangle = new Rectangle((int)positionHorizontal.X + correctPosition,
                        ((int)positionHorizontal.Y - (this.Image.Width / scale) + (pinguinHorizontal + platformSpeed)),
                        this.Image.Width / scale, this.Image.Height / scale); // na slizgu
                }
                else
                {
                    shoe = false;
                    Image = imageVertical;
                    rectangle = new Rectangle((int)positionVertical.X + correctPosition,
                        (int)positionVertical.Y - (this.Image.Width / scale) + (pinguinVertical + platformSpeed),
                        this.Image.Width / scale, this.Image.Height / scale); //jak stoi
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

            UpdateAnimation(gametime, rectangle);
            UpdateAnimationSlide(gametime, rectangle);
            currentdimensionsPenguin = UpdateDimensions(rectangle, false);

            if (positionVertical.Y > 1500)
            {
                OnPenguinDeathByFalling(penguinType);
            }



        }

        private void BlockSystem()
        {
            if (!block) //jezeli nie jest zablokowane
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
            /*    shoe = false;
                if(shoe) //jak pingwin stoi
                {*/
            if (actualCollisionRect != null) //jak pingwin lata to musi mu sie odlokowac mozliwosc poruszania sie na lewo i prawo
                if (!currentdimensionsPenguin[0].Intersects(actualCollisionRect) && !currentdimensionsPenguin[1].Intersects(actualCollisionRect) && !currentdimensionsPenguin[2].Intersects(actualCollisionRect) && !currentdimensionsPenguin[3].Intersects(actualCollisionRect) && !currentdimensionsPenguin[4].Intersects(actualCollisionRect))
                    blockDircetionLEFT = blockDirectionRIGHT = false;

            /*  tempRect = new Rectangle(rectangle.X+2, rectangle.Y+10, rectangle.Width-4, rectangle.Height-30);
              if ((tempRect.Intersects(r1))) //jak kolizja po prawej stronie
              {
                  actualCollisionRect = r1;
                  BlockSystem();
              }
              if (tempRect.Intersects(r1)) //jak kolizja po lewej stronie
              {
                  actualCollisionRect = r1;
                  BlockSystem();
              }*/
            if ((currentdimensionsPenguin[4].Intersects(r1))) //jak kolizja po prawej stronie
            {
                actualCollisionRect = r1;
                BlockSystem();
            }
            if (currentdimensionsPenguin[3].Intersects(r1)) //jak kolizja po lewej stronie
            {
                actualCollisionRect = r1;
                BlockSystem();
            }

            if (currentdimensionsPenguin[0].Intersects(r1)) //jak dotknie głowa
            {
                if (blockDircetionDOWN) speed.Y = 0;
                blockDircetionDOWN = false;
            }

            if (activeDirection && block) //jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie
                if (rectangle.X > tmpPosition.X + speedValue)
                {
                    correctPosition = 0;
                    block = false;
                }

            if (!activeDirection && block) //jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie
                if (rectangle.X < tmpPosition.X - speedValue)
                {
                    correctPosition = 0;
                    block = false;
                }

            if (!block) //jak nie zablokowane to odblokuj oba kierunki
                blockDircetionLEFT = blockDirectionRIGHT = false;


            if (platformType == PlatformType.FLOOR) //jak podloga to powoduje zeby pingwin nie mogl sie zaczepic od boku
            {
                r1.Height = (int)speed.Y + 1;
                if (currentdimensionsPenguin[2].Intersects(r1)) //jak dotknie nogami
                {
                    blockDircetionDOWN = false;
                    return true;
                }
            }
            else if (currentdimensionsPenguin[2].Intersects(r1)) //jak dotknie nogami
            {
                blockDircetionDOWN = false;
                return true;
            }


            return false;
            /*    }
                else
                {
                    if (actualCollisionRect != null) //jak pingwin lata to musi mu sie odlokowac mozliwosc poruszania sie na lewo i prawo
                        if (!currentdimensionsPenguin[0].Intersects(actualCollisionRect) && !currentdimensionsPenguin[1].Intersects(actualCollisionRect) && !currentdimensionsPenguin[2].Intersects(actualCollisionRect) && !currentdimensionsPenguin[3].Intersects(actualCollisionRect))
                            blockDircetionLEFT = blockDirectionRIGHT = false;

                    if ((currentdimensionsPenguin[1].Intersects(r1))) //jak kolizja po prawej stronie
                    {
                        actualCollisionRect = r1;
                        BlockSystem();
                    }
                    if (currentdimensionsPenguin[3].Intersects(r1)) //jak kolizja po lewej stronie
                    {
                        actualCollisionRect = r1;
                        BlockSystem();
                    }

                    if (currentdimensionsPenguin[0].Intersects(r1)) //jak dotknie głowa
                    {
                        if (blockDircetionDOWN) speed.Y = 0;
                        blockDircetionDOWN = false;
                    }

                    if (activeDirection && block) //jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie
                        if (rectangle.X > tmpPosition.X + speedValue)
                        {
                            correctPosition = 0;
                            block = false;
                        }

                    if (!activeDirection && block) //jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie
                        if (rectangle.X < tmpPosition.X - speedValue)
                        {
                            correctPosition = 0;
                            block = false;
                        }

                    if (!block) //jak nie zablokowane to odblokuj oba kierunki
                        blockDircetionLEFT = blockDirectionRIGHT = false;


                    if (platformType == PlatformType.FLOOR) //jak podloga to powoduje zeby pingwin nie mogl sie zaczepic od boku
                    {
                        r1.Height = (int)speed.Y + 1;
                        if (currentdimensionsPenguin[2].Intersects(r1)) //jak dotknie nogami
                        {
                            blockDircetionDOWN = false;
                            return true;
                        }
                    }
                    else if (currentdimensionsPenguin[2].Intersects(r1)) //jak dotknie nogami
                    {
                        blockDircetionDOWN = false;
                        return true;
                    }


                    return false;
                }

                return false;*/
        }

        public bool Collision(Rectangle r1)
        {
            return currentdimensionsPenguin[2].Intersects(r1);
        }


        public bool Collision2(Rectangle r1)
        {

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

        public override void Draw(SpriteBatch spriteBatch)
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
            if (_slide)
            {
                _animationHorizontal.Draw(spriteBatch, _left);
            }
            else
            {
                _animationVertival.Draw(spriteBatch, _left);
            }
        }

        public override string ToString()
        {
            return penguinType.ToString();
        }

        public void FallDown()
        {
            float a = Mass / Const.GRAVITY;
            if (speed.Y < currentdimensionsPenguin[2].Height * 0.7) speed.Y += 0.7f; //ograniczenie zeby pingwin stawał na platformie a nie w jej połowie
        }

        public void Jump()
        {
            speed.Y = -Const.GRAVITY;
            jump = true;
            Position.Y -= (int)Math.Pow(Const.GRAVITY * 3, 2) / Mass;
        }
        int licz = 0;
        public void JumpSpring()
        {

            speed.Y = -Const.SPRING_JUMP;
            jump = true;
            Position.Y -= ((int)Math.Pow(Const.GRAVITY * 3, 2) / Mass) + Const.SPRING_JUMP;


        }

    }
}