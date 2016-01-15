using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using TestGame.Interfaces;

namespace TestGame
{
    public class Penguin : TextureManager, IGravitable, ISpeakable
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

        public List<SoundEffect> Voices { get; set; }

        public List<Rectangle> currentdimensionsPenguin = new List<Rectangle>();
        public List<Rectangle> currentdimensionsPenguinShoe = new List<Rectangle>();
       // private Vector2 positionHorizontal;
       // private Vector2 positionVertical;
        private Vector2 tmpPosition = new Vector2();
        private Platform blockPlatform = new Platform();
        // private bool shoe = false;
        public bool jump = true;
        public bool firstStart = true;
        public bool active = true;
        public bool inMove = false;
        private bool activeDirection = true; // true = prawo, false = lewo
        public bool blockDircetionLEFT = false;
        public bool blockDirectionRIGHT = false;
        private bool blockDircetionDOWN = false;
        private int correctPositionX = 0;
        private int correctPositionY = 0;
        private bool blockVomit;
        private bool block = false;
        private bool activeSpace = false;
        public int scale = 4;
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

            Position = position.ToPoint();

            Voices = new List<SoundEffect>();

            _positionOnSheet = new Rectangle(1, 1, frameSize.X, frameSize.Y);//Ł.G;
            _frameDuration = 0;
            _frameDelay = 100;


            this.PenguinDeathByFallingHandler += Penguin_PenguinDeathByFallingHandler;

            Size = new Point(image.Width / 8, image.Width / scale);

            CanMove = true;

            //każdy typ pingwina ma róźną wysokość, wartości odpowiednio przeskalowane
            switch (penguinType)
            {
                case PenguinType.KOWALSKI:
                    pinguinVertical = 0;//= Const.PINGUIN_KOWALSKI_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_KOWALSKI_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.KOWALSKI);
                    dimensionsPenguinSlide = Const.DimensionsPenguinSlide(PenguinType.KOWALSKI);
                    break;
                case PenguinType.RICO:
                    pinguinVertical = 0;//= Const.PINGUIN_RICO_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_RICO_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.RICO);
                    dimensionsPenguinSlide = Const.DimensionsPenguinSlide(PenguinType.RICO);
                    break;
                case PenguinType.SZEREGOWY:
                    pinguinVertical = 0;//= Const.PINGUIN_SZEREGOWY_VERTICAL;
                    pinguinHorizontal = Const.PINGUIN_SZEREGOWY_HORIZONTAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SZEREGOWY);
                    dimensionsPenguinSlide = Const.DimensionsPenguinSlide(PenguinType.SZEREGOWY);
                    break;
                case PenguinType.SKIPPER:
                    pinguinHorizontal =0;//= Const.PINGUIN_SKIPPER_HORIZONTAL;
                    pinguinVertical = Const.PINGUIN_SKIPPER_VERTICAL;
                    dimensionsPenguin = Const.DimensionsPenguin(PenguinType.SKIPPER);
                    dimensionsPenguinSlide = Const.DimensionsPenguinSlide(PenguinType.SKIPPER);
                    break;

            }
            // przeskalowanie wymiarów
            for (int i = 0; i < dimensionsPenguin.Count; i++)
                dimensionsPenguin[i] = ReScale(dimensionsPenguin[i], scale);
            for (int i = 0; i < dimensionsPenguinSlide.Count; i++)
                dimensionsPenguinSlide[i] = ReScale(dimensionsPenguinSlide[i], scale);

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
               _animationVertival.Update(gametime, newRectangle);
            else
                _animationVertival.UpdateInStay(newRectangle);

        }
        private void UpdateAnimationSlide(GameTime gametime, Rectangle newRectangle)
        {
            if (active && inMove)//52r,58k,38s,38s
                _animationHorizontal.Update(gametime, newRectangle);
            else
            {
                _animationVertival.UpdateInStay(newRectangle);
                _slide = false;
            }

        }

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
                    speed.X = speedValue * 2;
                    activeDirection = true;
                    _slide = true;
                    _left = false;
                    inMove = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    speed.X = -speedValue * 2;
                    activeDirection = false;
                    _slide = true;
                    _left = true;
                    inMove = true;
                }
                else
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !blockDirectionRIGHT)
                {
                    speed.X = speedValue;
                    activeDirection = true;
                    _left = false;
                    inMove = true;
                    _slide = false;

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !blockDircetionLEFT)
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

              //  if (Keyboard.GetState().IsKeyUp(Keys.Down)) activeKeysDown = false;
                if (_slide)// na slizgu
                {
                    activeKeysDown = true;
                    rectangle = new Rectangle((int)Position.ToVector2().X + correctPositionX,
                                             ((int)Position.ToVector2().Y  + (pinguinHorizontal + platformSpeed/* + correctPositionY*/)),
                                              _animationHorizontal.Position.Width,
                                              _animationHorizontal.Position.Height); 

                }
                else //jak stoi         
                {
                    activeKeysDown = true;
                    rectangle = new Rectangle((int)Position.ToVector2().X + correctPositionX,
                                              (int)Position.ToVector2().Y + (pinguinVertical + platformSpeed + correctPositionY),
                                             _animationVertival.Position.Width,
                                              _animationVertival.Position.Height);    
                }
            }
            else//gdy nieaktywny
            {
                speed.X = 0;
                Position += speed.ToPoint();
                speed.Y += 0.15f;

                rectangle = new Rectangle((int)Position.ToVector2().X + correctPositionX,
                                          (int)Position.ToVector2().Y + (pinguinVertical + platformSpeed + correctPositionY),
                                           _animationVertival.Position.Width,
                                           _animationVertival.Position.Height); //jak stoi

            }

            UpdateAnimation(gametime, rectangle);//update pozycji textury dla stojacego
            UpdateAnimationSlide(gametime, rectangle);//update pozycji textury dla slizgu

            currentdimensionsPenguin = UpdateDimensions(rectangle, _slide); //update wymiarow/pozycji poszczegolnych czesci pingwina



            if (Position.ToVector2().Y > 1500)//gdy pingwin spadnie
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
                    correctPositionX = (int)-speedValue;
                }
                if (!activeDirection) //lewo
                {
                    blockDircetionLEFT = true;
                    blockDirectionRIGHT = false;
                    correctPositionX = (int)speedValue;
                }
            }
        }


        Vector2 posTemp;
        /// <summary>
        /// Wykrywa kolizje pingwina z parametrem
        /// </summary>
        /// <param name="platform">Platforma do sprawdzenia</param>
        /// <returns>True - gdy pingwin znajduje się na platformie
        ///          False - gdy pingwin nie znajduje się na platformie</returns>
        public bool Collision(Rectangle r1, PenguinType penguinType = PenguinType.NN, PlatformType platformType = PlatformType.NN)
        {
            if(currentdimensionsPenguin[3].Intersects(r1) && platformType == PlatformType.CAR)
            {
                int x = 0;
            }

            if (actualCollisionRect != null) //jak pingwin lata to musi mu sie odlokowac mozliwosc poruszania sie na lewo i prawo
                if (!currentdimensionsPenguin[0].Intersects(actualCollisionRect) &&
                    !currentdimensionsPenguin[1].Intersects(actualCollisionRect) &&
                    !currentdimensionsPenguin[2].Intersects(actualCollisionRect) &&
                    !currentdimensionsPenguin[3].Intersects(actualCollisionRect))

                    blockDircetionLEFT = blockDirectionRIGHT = false;

            if ((currentdimensionsPenguin[3].Intersects(r1))) //jak kolizja po prawej stronie
            {
                actualCollisionRect = r1;
                BlockSystem();
            }

            if (currentdimensionsPenguin[2].Intersects(r1)) //jak kolizja po lewej stronie
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
                    correctPositionX = 0;
                    block = false;
                }

            if (!activeDirection && block) //jak pingwin zmienił pozycje w przeciwną strone to odblokuj blokowanie
                if (rectangle.X < tmpPosition.X - speedValue)
                {
                    correctPositionX = 0;
                    block = false;
                }

            if (!block) //jak nie zablokowane to odblokuj oba kierunki
                blockDircetionLEFT = blockDirectionRIGHT = false;


            if (platformType == PlatformType.FLOOR) //jak podloga to powoduje zeby pingwin nie mogl sie zaczepic od boku
            { 
                 r1.Height = (int)speed.Y + 1;
                if (currentdimensionsPenguin[1].Intersects(r1)) //jak dotknie nogami
                {
                    //correctPositionY = (r1.Y - (rectangle.Y + rectangle.Height)) * -1;
                    blockDircetionDOWN = false;
                    return true;
                }
            }
            else if (currentdimensionsPenguin[1].Intersects(r1)) //jak dotknie nogami
            {
                r1.Height = (int)speed.Y + 1;
                // correctPositionY = (r1.Y - (rectangle.Y + rectangle.Height)) * -1;
                blockDircetionDOWN = false;
                return true;
            }


            return false;
        }


        public bool CollisionRectangle(Rectangle r1)
        {
            return currentdimensionsPenguin[1].Intersects(r1);
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
            //   rectangle.Width /= scale;
            if (penguinType == PenguinType.RICO)
            {
                if (_slide)
                {
                    _animationHorizontal.Draw(spriteBatch, _left);
                }
                else
                {
                    _animationVertival.Draw(spriteBatch, _left, true);
                }
            }
            else
            {
            if (_slide)
            {
                _animationHorizontal.Draw(spriteBatch, _left);
            }
            else
            {
                _animationVertival.Draw(spriteBatch, _left);
            }
            }

        }

        public override string ToString()
        {
            return penguinType.ToString();
        }

        public void StartSpeaking()
        {
            if (Voices.Any())
            {
                Random rand = new Random();
                int index = rand.Next(0, Voices.Count);
                Voices[index].Play(SoundManager.Volume, 0.0f, 0.0f);
            }
        }

        public void FallDown()
        {
            float a = Mass / Const.GRAVITY;
            if (speed.Y < currentdimensionsPenguin[1].Height * 0.7) speed.Y += 0.7f; //ograniczenie zeby pingwin stawał na platformie a nie w jej połowie
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