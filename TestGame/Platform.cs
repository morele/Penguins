using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Interfaces;

namespace TestGame
{
    /// <summary>
    /// Klasa reprezentująca platformę, która porusza się w górę i w dół
    /// </summary>
    public class Platform : TextureManager, ICollisionable
    {
        public Rectangle PlatformRectangle;

        // flaga informująca o tym czy platforma jest w ruchu
        public bool IsMotion { get; private set; }

        // kierunek poruszania się platformy
        public Direction Direction { get; private set; }

        // prędkość poruszania się platformy
        public float PlatformSpeed { get; private set; }

        // maksymalna prędkość poruszania się platformy
        private readonly float _maxPlatformSpeed;

        // maksymalny zasięg platformy
        private readonly float _maxPlatformScope;

        // aktualna pozycja platformy
        private int _currentPlatformPosition;

        //typ platformy
        public PlatformType platformType;

        //LG: dla samochodu
        public bool ActiveCar
        {
            get; set;
        }

        //Czy platforma jest aktywna
        public bool active = true;
        private bool stop = true;
        private float time = 0;
        public bool initJump = false;
        public bool jump = false;
        public Vector2 speed = new Vector2(0);
        public float angleFall = 10f;
        public Platform() { }
        /// <summary>
        /// Tworzy obiekt platformy
        /// </summary>
        /// <param name="Image">Tekstura platformy</param>
        /// <param name="position">Początkowa pozycja platformy</param>
        /// <param name="isMotion">Flaga ustawiająca poruszanie się platformy</param>
        /// <param name="maxSpeed">Maksymalna prędkość platformy</param>
        /// <param name="maxScope">Maksymalny zasięg platformy (względem początkowego położenia)</param>
        public Platform(Texture2D Image, Vector2 position, bool isMotion = false, float maxSpeed = 0, float maxScope = 0, PlatformType platformType = PlatformType.FLOOR) : base(Image, position)
        {
            // domyślnie platforma będzie na dole, czyli ruch będzie w górę
            Direction = Direction.Up;

            // ustawienie maksymalnej prędkości platformy
            _maxPlatformSpeed = maxSpeed;
            PlatformSpeed = maxSpeed;

            _maxPlatformScope = maxScope;

            IsMotion = isMotion;

            this.platformType = platformType;

            PlatformRectangle = new Rectangle((int)position.X, (int)position.Y, this.Image.Width, this.Image.Height);
        }
        public Platform(Animation animation, bool isMotion = false, float maxSpeed = 0, float maxScope = 0, PlatformType platformType = PlatformType.FLOOR) : base(animation, new Vector2(animation.PositionStaticItems.X, animation.PositionStaticItems.Y))
        {
            // domyślnie platforma będzie na dole, czyli ruch będzie w górę
            Direction = Direction.Up;



            // ustawienie maksymalnej prędkości platformy
            _maxPlatformSpeed = maxSpeed;
            PlatformSpeed = maxSpeed;

            _maxPlatformScope = maxScope;

            IsMotion = isMotion;


            this.platformType = platformType;

            PlatformRectangle = Animation.PositionStaticItems;


        }

        public void UpdateCar(GameTime gametime)
        {
            if (platformType == PlatformType.CAR)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Position.X += 2;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Position.X -= 2;
                }

                Animation.UpdatePosition(Position.X, Position.Y);
            }

        }
        override public void UpdatePosition(GameTime gametime)
        {
            if (platformType == PlatformType.CAR && !ActiveCar && Animation != null)
            {
                Animation.UpdateInStay();
            }
            else if (platformType == PlatformType.CAR && ActiveCar)
            {
                Animation?.Update(gametime);
            }
            // jeśli platforma się porusza
            if (IsMotion)
            {
                if (stop)//jak platforma ma sie podczas ruchu zatrzymac na jakis czas
                {
                    // sprawdź kierunek ruchu
                    switch (Direction)
                    {
                        // ruch w górę
                        case Direction.Up:

                            time += gametime.ElapsedGameTime.Milliseconds;

                            // ustaw pozycję o ile nie została przekroczona maksymalna wysokość platformy
                            if (_currentPlatformPosition <= _maxPlatformScope)
                            {
                                _currentPlatformPosition += (int)PlatformSpeed;
                                Position = new Point(Position.X, Position.Y - (int)PlatformSpeed);
                            }

                            // maksymalna wysokość została przekroczona - platforma zawraca
                            else
                            if (time > 4000)
                            {
                                Direction = Direction.Down;
                                time = 0;
                            }

                            break;

                        // ruch w dół
                        case Direction.Down:

                            time += gametime.ElapsedGameTime.Milliseconds;



                            // ustaw pozycję platformy o ile nie znajduje się na dole
                            if (_currentPlatformPosition >= 0)
                            {
                                _currentPlatformPosition -= (int)PlatformSpeed;
                                Position = new Point(Position.X, Position.Y + (int)PlatformSpeed);
                            }

                            // platforma na dole - teraz się w górę
                            else
                            if (time > 4000)
                            {
                                Direction = Direction.Up;
                                time = 0;
                            }

                            break;
                    }
                }
                else
                {
                    // sprawdź kierunek ruchu
                    switch (Direction)
                    {
                        // ruch w górę
                        case Direction.Up:

                            _currentPlatformPosition += (int)PlatformSpeed;

                            // ustaw pozycję o ile nie została przekroczona maksymalna wysokość platformy
                            if (_currentPlatformPosition <= _maxPlatformScope)
                                Position = new Point(Position.X, Position.Y - (int)PlatformSpeed);

                            // maksymalna wysokość została przekroczona - platforma zawraca
                            else
                                Direction = Direction.Down;

                            break;

                        // ruch w dół
                        case Direction.Down:

                            _currentPlatformPosition -= (int)PlatformSpeed;

                            // ustaw pozycję platformy o ile nie znajduje się na dole
                            if (_currentPlatformPosition >= 0)
                                Position = new Point(Position.X, Position.Y + (int)PlatformSpeed);

                            // platforma na dole - teraz się w górę
                            else
                                Direction = Direction.Up;

                            break;
                    }
                }


                // aktualizacja sprite'a
                PlatformRectangle = new Rectangle((int)Position.X, (int)Position.Y, PlatformRectangle.Width, PlatformRectangle.Height);
            }
            if (platformType == PlatformType.MONEY)
            {
                if (jump)
                {

                    speed.Y += 0.15f;
                    speed.X += angleFall;
                    if (angleFall > 0.01) angleFall -= 0.2f;
                    Position += speed.ToPoint();

                }
                if (initJump)
                {
                    Position.Y -= 30;
                    Position.X += 60;
                    initJump = false;
                    jump = true;
                }

                PlatformRectangle = new Rectangle(Position, Size);
            }
            if (gametime != null && Animation != null && platformType != PlatformType.CAR)
            {
                this.Animation.Update(gametime, PlatformRectangle);
            }

        }

        public bool CollisionPlatform(Rectangle r1)
        {
            return PlatformRectangle.Intersects(r1);
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            if (Animation != null)
            {
                Animation.DrawStaticItems(spriteBatch);
            }
            else
            {
                if (active) spriteBatch.Draw(Texture, PlatformRectangle, Color.White);
            }

        }


        /// <summary>
        /// Metoda powoduje spowolnienie poruszania się platformy
        /// </summary>
        public void Slowdown()
        {
            if (PlatformSpeed > 1)
                PlatformSpeed--;
        }

        /// <summary>
        /// Metoda powoduje zwiększenie prędkości poruszania się platformy
        /// </summary>
        public void SpeedUp()
        {
            if (PlatformSpeed <= _maxPlatformSpeed)
                PlatformSpeed++;
        }
        public void ResetMoney(Rectangle rectangle)
        {
            Position.X = rectangle.X;
            Position.Y = rectangle.Y;
            speed.X = 0;
            speed.Y = 0;
            angleFall = 1f;
        }

        public bool IsCollisionDetect(GameObject collisionObject)
        {
            Point newSize = new Point(Size.X, Size.Y + 100);
            Rectangle rectangle1 = new Rectangle(Position, newSize);
            Rectangle rectangle2 = new Rectangle(collisionObject.Position, collisionObject.Size);

            // sprawdzenie czy nastąpiła kolizja
            if (rectangle1.Intersects(rectangle2))
                return true;
            return false;
        }

        public void OnCollisionDetect(GameObject collisionObject)
        {
            if (collisionObject is Penguin)
            {

            }
        }


    }
}
