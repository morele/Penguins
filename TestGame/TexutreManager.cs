using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace TestGame
{
    abstract public class TextureManager
    {
        protected Vector2 position; // aktualna pozycja textury
        protected Texture2D Image; //tekstrua 
        protected float speedValue; //szybkość poruszania się 
        protected float gravitation; //wysokość wybicia przy skoku

        /// <summary>
        /// Uniwersalny konstuktor dla tekstur którymi nie mamy w zamiarze się ruszać
        /// </summary>
        /// <param name="Image">Przyjmuje content textury</param>
        /// <param name="position">Początkowa pozycja textury</param>
        /// <param name="speedValue">Szybkość poruszania się textury(optymalna 3-5)</param>
        public TextureManager(Texture2D Image, Vector2 position, float speedValue)
        {
            if(Image == null || position == null || speedValue == null) throw new ArgumentNullException("Null exception, TextureManager");
            this.Image = Image;
            this.position = position;
            this.speedValue = speedValue;
        }

        /// <summary>
        /// Konstruktor używany tylko dla textur którymi mamy zamiar ruszać się/skakać
        /// </summary>
        /// <param name="Image">Przyjmuje content textury</param>
        /// <param name="position">Początkowa pozycja textury</param>
        /// <param name="speedValue">Szybkość poruszania się textury(optymalna 3-5)</param>
        /// <param name="gravitation">wysokość wybicia przy skoku(optymalna 5)</param>
        public TextureManager(Texture2D Image, Vector2 position, float speedValue, float gravitation)
        {
            if (Image == null || position == null || speedValue == null || gravitation == null) throw new ArgumentNullException("Null exception, TextureManager");
            this.Image = Image;
            this.position = position;
            this.speedValue = speedValue;
            this.gravitation = gravitation;
        }

        /// <summary>
        /// Metoda zmieniająca pozycje tekstury względem osi X o zadaną jednostke speedValue
        /// </summary>
        public virtual void UpdatePosition()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) position.X = speedValue;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) position.X = -speedValue; else position.X = 0;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
