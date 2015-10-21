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
        private Vector2 _textPosition;
        private Rectangle _rectangle;
        private float _scale = 1;
        private string _text;
        private SpriteFont _font;
        private Texture2D _background;
        private Color _color = Color.Black;
        private Alignment _alignment=Alignment.Bottom;
        public Alignment alignment
        {
            get
            {
                return _alignment;
                
            }
            set
            {
                _alignment = value; 
                
            }
        }

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

        public float Scale
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
        public TextLabel(Vector2 position, float scaleInProcent, string text, SpriteFont font, Texture2D background)
        {
            _position = position;
            _text = text;
            _font = font;
            _background = background;
            _scale = (scaleInProcent / 100);
            _textPosition = SetPosition();
            
            
            
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
            _textPosition = SetPosition();
            
            

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
              
                spriteBatch.Draw(_background, new Rectangle((int)(_position.X), (int)(_position.Y ),(int)(_background.Width * _scale), (int)(_background.Height * _scale)), Color.White);
                spriteBatch.DrawString(_font, _text, _textPosition, _color);
            }
            else
            {
                spriteBatch.DrawString(_font, _text, _textPosition, _color);
               
            }

        }
        [Flags]
        public enum Alignment
        {
            Center = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8
        };

        private Vector2 SetPosition()
        {
            Vector2 textSize = _font.MeasureString(_text);

            if (alignment.HasFlag(Alignment.Left))
            {
                _textPosition.X = (_position.X - textSize.X - 2);
                _textPosition.Y = ((_position.Y + (_background.Height * _scale)) - textSize.Y) / 2;
            }

            if (alignment.HasFlag(Alignment.Right))
            {
                _textPosition.X = (_position.X + textSize.X + 2);
                _textPosition.Y = ((_position.Y + (_background.Height * _scale)) - textSize.Y) / 2;
            }

            if (alignment.HasFlag(Alignment.Top))
            {
                _textPosition.X = ((_position.X + (_background.Width * _scale)));
                _textPosition.Y = (_position.Y - (_background.Height * _scale) - 2);
            }


            if (alignment.HasFlag(Alignment.Bottom))
            {
                _textPosition.X = ((_position.X + (_background.Width * _scale)));
                _textPosition.Y = (_position.Y + (_background.Height * _scale) + 2);

            }
            return new Vector2(_textPosition.X, _textPosition.Y);
               
        }
        /// <summary>
        /// update metoda na zmiane pozycji
        /// </summary>
        /// <param name="position"></param>
        public void Update(Vector2 position)
        {
            _position = position;
            _textPosition = SetPosition();
        }
        /// <summary>
        /// zmiana pozycji i tekstu
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        public void Update(Vector2 position, string text)
        {
            _position = position;
            _textPosition = SetPosition();
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
            _textPosition = SetPosition();
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
            _textPosition = SetPosition();
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
            _textPosition = SetPosition();
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
            _textPosition = SetPosition();
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
            _textPosition = SetPosition();
            _font = font;
            _text = text;
            _color = color;
        }

    }
}