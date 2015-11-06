using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    /// <summary>
    /// Klasa reprezentująca platformę, która porusza się w górę i w dół
    /// </summary>
    public class Platform : TextureManager
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

        //Czy platforma jest aktywna
        public bool active = true;
        public bool initJump = false;
        public bool jump = false;
        public Vector2 speed = new Vector2(0);
        public float angleFall = 10f;
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

        override public void UpdatePosition()
        {
            // jeśli platforma się porusza
            if (IsMotion)
            {
                // sprawdź kierunek ruchu
                switch (Direction)
                {
                    // ruch w górę
                    case Direction.Up:

                        _currentPlatformPosition += (int)PlatformSpeed;

                        // ustaw pozycję o ile nie została przekroczona maksymalna wysokość platformy
                        if (_currentPlatformPosition <= _maxPlatformScope)
                            position.Y -= PlatformSpeed;

                        // maksymalna wysokość została przekroczona - platforma zawraca
                        else
                            Direction = Direction.Down;

                        break;

                    // ruch w dół
                    case Direction.Down:

                        _currentPlatformPosition -= (int)PlatformSpeed;

                        // ustaw pozycję platformy o ile nie znajduje się na dole
                        if (_currentPlatformPosition >= 0)
                            position.Y += PlatformSpeed;

                        // platforma na dole - teraz się w górę
                        else
                            Direction = Direction.Up;

                        break;
                }

                // aktualizacja sprite'a
                PlatformRectangle = new Rectangle((int)position.X, (int)position.Y, Image.Width, Image.Height);
            }
            if (platformType == PlatformType.MONEY)
            {
                if (jump)
                {

                    speed.Y += 0.15f;
                    speed.X += angleFall;
                    if (angleFall > 0.01) angleFall -= 0.2f;
                    position += speed;

                }
                if (initJump)
                {
                    position.Y -= 30;
                    position.X += 60;
                    initJump = false;
                    jump = true;
                }

                PlatformRectangle = new Rectangle((int)position.X, (int)position.Y, Image.Width, Image.Height);
            }
            

            

        }

        public bool CollisionPlatform(Rectangle r1)
        {
            return PlatformRectangle.Intersects(r1);
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            if(active) spriteBatch.Draw(Image, PlatformRectangle, Color.White);
        }


        /// <summary>
        /// Metoda powoduje spowolnienie poruszania się platformy
        /// </summary>
        public void Slowdown()
        {
            if(PlatformSpeed > 1)
                PlatformSpeed--;
        }

        /// <summary>
        /// Metoda powoduje zwiększenie prędkości poruszania się platformy
        /// </summary>
        public void SpeedUp()
        {
            if(PlatformSpeed <= _maxPlatformSpeed)
                PlatformSpeed++;
        }
        public void ResetMoney(Rectangle rectangle)
        {
            position.X = rectangle.X;
            position.Y = rectangle.Y;
            speed.X = 0;
            speed.Y = 0;
            angleFall = 1f;
        }
                
    }
}
