﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TestGame
{
    abstract public class TextureManager : GameObject
    {
        //public Vector2 position; // aktualna pozycja textury
        protected Texture2D Image; //tekstrua 
        public Animation Animation; //animacja
        protected float speedValue; //szybkość poruszania się 
        protected float gravitation; //wysokość wybicia przy skoku
        protected List<Rectangle> dimensionsPenguin;
        protected List<Rectangle> dimensionsPenguinSlide;




        /// <summary>
        /// Uniwersalny konstuktor dla tekstur którymi nie mamy w zamiarze się ruszać
        /// </summary>
        /// <param name="Image">Przyjmuje content textury</param>
        /// <param name="position">Początkowa pozycja textury</param>
        /// <param name="speedValue">Szybkość poruszania się textury(optymalna 3-5)</param>
        public TextureManager(Texture2D Image, Vector2 position)
            : base(Image, position.ToPoint(), new Point(Image.Width, Image.Height))
        {
            if (Image == null || position == null) throw new ArgumentNullException("Null exception, TextureManager");
            this.Image = Image;
            //this.position = position;

            Texture = Image;
            Position = position.ToPoint();
            Size = new Point(Image.Width, Image.Height);
        }

        public TextureManager()
        {

        }
        /// <summary>
        /// Konstruktor używany tylko dla textur którymi mamy zamiar ruszać się/skakać
        /// </summary>
        /// <param name="Image">Przyjmuje content textury</param>
        /// <param name="position">Początkowa pozycja textury</param>
        /// <param name="speedValue">Szybkość poruszania się textury(optymalna 3-5)</param>
        /// <param name="gravitation">wysokość wybicia przy skoku(optymalna 5)</param>
        public TextureManager(Texture2D Image, Vector2 position, float speedValue, float gravitation)
            : base(Image, position.ToPoint(), new Point(Image.Width, Image.Height))
        {
            if (Image == null || position == null || speedValue == null || gravitation == null) throw new ArgumentNullException("Null exception, TextureManager");
            this.Image = Image;
            // this.position = position;
            this.speedValue = speedValue;
            this.gravitation = gravitation;

            Texture = Image;
            Position = position.ToPoint();
            Size = new Point(Image.Width, Image.Height);

        }
        /// <summary>
        /// Konstruktor używany tylko dla textur którymi mamy zamiar ruszać się/skakać paramert do animacji
        /// </summary>
        /// <param name="Image">Przyjmuje content textury</param>
        /// <param name="position">Początkowa pozycja textury</param>
        /// <param name="speedValue">Szybkość poruszania się textury(optymalna 3-5)</param>
        /// <param name="gravitation">wysokość wybicia przy skoku(optymalna 5)</param>
        /// <param name="frameSize">ustawienie rozmiarow do animacji</param>
        public TextureManager(Texture2D Image, Vector2 position, float speedValue, float gravitation, Point frameSize)
            : base(Image, position.ToPoint(), new Point(Image.Width, Image.Height))
        {
            if (Image == null || position == null || speedValue == null || gravitation == null) throw new ArgumentNullException("Null exception, TextureManager");
            this.Image = Image;
            // this.position = position;
            this.speedValue = speedValue;
            this.gravitation = gravitation;

            Texture = Image;
            Position = position.ToPoint();
            Size = new Point(Image.Width, Image.Height);
            this.FrameSize = frameSize;
        }
        /// <summary>
        /// w razie animacji rzeczy tworze ten konstuktor
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="position"></param>
        public TextureManager(Animation animation, Vector2 position)
        {
            if (animation == null || position == null) throw new ArgumentNullException("Null exception, TextureManager");
            Animation = animation;
            //this.position = position;

            Position = position.ToPoint();
            Size = new Point(animation.PositionStaticItems.Width, animation.PositionStaticItems.Height);
        }
        /// <summary>
        /// Metoda zmieniająca pozycje tekstury względem osi X o zadaną jednostke speedValue
        /// </summary>
        public virtual void UpdatePosition(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position.X = (int)speedValue;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position.X = (int)-speedValue;
            }
            else
            {
                Position.X = 0;
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public Rectangle ReScale(Rectangle rectangle, int scale)
        {
            rectangle.X /= scale;
            rectangle.Y /= scale;
            rectangle.Width /= scale;
            rectangle.Height /= scale;
            return rectangle;
        }
        public List<Rectangle> UpdateDimensions(Rectangle r1, bool shoe, int speed = 0)
        {
            List<Rectangle> rectangle = new List<Rectangle>();

            if (shoe == false)
                for (int i = 0; i < dimensionsPenguin.Count; i++)
                    rectangle.Add(new Rectangle(r1.X + dimensionsPenguin[i].X,
                                                r1.Y + dimensionsPenguin[i].Y + speed,
                                                dimensionsPenguin[i].Width,
                                                dimensionsPenguin[i].Height));
            if (shoe == true)

                for (int i = 0; i < dimensionsPenguinSlide.Count; i++)
                    rectangle.Add(new Rectangle(r1.X + dimensionsPenguinSlide[i].X,
                                                r1.Y + dimensionsPenguinSlide[i].Y + speed,
                                                dimensionsPenguinSlide[i].Width,
                                                dimensionsPenguinSlide[i].Height));



            return rectangle;
        }
    }
}
