using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public static class CONST
    {
        public static int PLATFORM_MAX_HEIGHT = 100;
    }

    /// <summary>
    /// Klasa reprezentująca platformę, która porusza się w górę i w dół
    /// </summary>
    public class Platform : TextureManager
    {
        public Rectangle PlatformRectangle { get; private set; }

        // flaga informująca o tym czy platforma jest w ruchu
        public bool IsMotion { get; private set; }

        // kierunek poruszania się platformy
        public Direction Direction { get; private set; }

        // prędkość poruszania się platformy
        public float PlatformSpeed { get; private set; }

        private float _maxPlatformSpeed;

        private float _maxPlatformScope;

        // aktualna pozycja platformy
        private int _currentPlatformPosition;

        /// <summary>
        /// Tworzy obiekt platformy
        /// </summary>
        /// <param name="Image">Tekstura platformy</param>
        /// <param name="position">Początkowa pozycja platformy</param>
        /// <param name="isMotion">Flaga ustawiająca poruszanie się platformy</param>
        /// <param name="maxSpeed">Maksymalna prędkość platformy</param>
        /// <param name="maxScope">Maksymalny zasięg platformy (względem początkowego położenia)</param>
        public Platform(Texture2D Image, Vector2 position, bool isMotion = false, float maxSpeed = 0, float maxScope = 0) : base(Image, position)
        {
            // domyślnie platforma będzie na dole, czyli ruch będzie w górę
            Direction = Direction.Up;

            // ustawienie maksymalnej prędkości platformy
            _maxPlatformSpeed = maxSpeed;
            PlatformSpeed = maxSpeed;

            _maxPlatformScope = maxScope;

            IsMotion = isMotion;
  
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
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, PlatformRectangle, Color.White);
        }


        public void Slowdown()
        {
            if(PlatformSpeed > 1)
                PlatformSpeed--;
        }

        public void SpeedUp()
        {
            if(PlatformSpeed <= _maxPlatformSpeed)
                PlatformSpeed++;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"> szybkośc poruszania się w góre i dół</param>
        /// <param name="maxTop"> maksymalny półap platformy (zależny od początkowego ustawienia)</param>
        /// <param name="maxBottom"> minimalny półap platformy(zależny od początkowego ustawienia)</param>
        public void Properties(int speed, int maxTop, int maxBottom)
        {
            PlatformSpeed = speed;
        }


        
    }
}
