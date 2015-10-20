using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public class TextLabel
    {
        #region private area
        private Vector2 _position;
        private Rectangle _rectangle;
        private int _scale = 1;
        private string _text;
        private SpriteFont _font;
        private Texture2D _background;
        private Color _color;
        #endregion

        #region public area property
        public Texture2D Background
        {
            get
            {
                return _background;

            }
            set
            {
                _background = value;

            }
        }

        public SpriteFont Font
        {
            get
            {
                return _font;

            }
            set
            {
                _font = value;

            }
        }

        public string Text
        {
            get
            {
                return _text;

            }
            set
            {
                _text = value;

            }
        }

        public int Scale
        {
            get
            {
                return _scale / 100;

            }
            set
            {
                _scale = (value / 100);

            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;

            }
            set
            {
                _position = value;

            }
        }

        public Color color
        {
            get
            {
                return _color;

            }
            set
            {
                _color = value;

            }
        }
#endregion
        /// <summary>
        /// Konsktruktor TextLabel wersja z tłem
        /// </summary>
        /// <param name="position">pozycja textlabel</param>
        /// <param name="scaleInProcent">skala backgrounda textlabel</param>
        /// <param name="text">tekst ktory bedzie wyswietlany </param>
        /// <param name="font">spriteFont który bedzie uzyty w textlabel</param>
        /// <param name="background">Textura ktora bedzie za napisem</param>
        public TextLabel(Vector2 position, int scaleInProcent, string text, SpriteFont font, Texture2D background)
        {
            _position = position;
            _scale = scaleInProcent / 100;
            _text = text;
            _font = font;
            _background = background;
        }
        /// <summary>
        /// konstruktor TextLabel wersja bez tła
        /// </summary>
        /// <param name="position">pozycja TextLabel</param>
        /// <param name="text">Tekst wyswietlany w TextLabel</param>
        /// <param name="font">Font uzyty w TextLabel</param>
        public TextLabel(Vector2 position, string text, SpriteFont font)
        {
            _position = position;
            _text = text;
            _font = font;

        }

        /// <summary>
        /// Draw z mozliwoscia wyboru z tłem lub bez
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="background"></param>
        public void Draw(SpriteBatch spriteBatch, bool background = false)
        {
            if (background)
            {
                spriteBatch.Draw(_background, new Rectangle((int)_position.X * _scale, (int)_position.Y * _scale, _background.Width, _background.Height), Color.White);
                spriteBatch.DrawString(_font, _text, _position, _color);
            }
            else
            {
                spriteBatch.DrawString(_font, _text, _position, _color);
            }

        }
        /// <summary>
        /// update metoda na zmiane pozycji
        /// </summary>
        /// <param name="position"></param>
        public void Update(Vector2 position)
        {
            _position = position;
        }
        /// <summary>
        /// zmiana pozycji i tekstu
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        public void Update(Vector2 position, string text)
        {
            _position = position;
            _text = text;
        }
        /// <summary>
        /// zmiana pozycji tekstu i tła
        /// </summary>
        /// <param name="position"></param>
        /// <param name="background"></param>
        /// <param name="text"></param>
        public void Update(Vector2 position, Texture2D background, string text)
        {
            _position = position;
            _background = background;
            _text = text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="background"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        public void Update(Vector2 position, Texture2D background, SpriteFont font, string text)
        {
            _position = position;
            _background = background;
            _font = font;
            _text = text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="background"></param>
        /// <param name="font"></param>
        public void Update(Vector2 position, Texture2D background, SpriteFont font)
        {
            _position = position;
            _background = background;
            _font = font;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        public void Update(Vector2 position, SpriteFont font, string text)
        {
            _position = position;
            _font = font;
            _text = text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public void Update(Vector2 position, SpriteFont font, string text,Color color)
        {
            _position = position;
            _font = font;
            _text = text;
            _color = color;
        }

    }
}