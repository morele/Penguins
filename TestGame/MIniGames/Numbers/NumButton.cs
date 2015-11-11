using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.MIniGames.Numbers
{
    public class NumButton
    {
        private readonly Texture2D _texture;
        private Vector2 _position;

        /// <summary>
        /// only get
        /// </summary>
        public Rectangle Rectangle
        {
            get;
            private set;
        }

        public string _number { get; set; }

        private readonly SpriteFont _font;
        public bool visibility { get; set; }

        private bool _mark = false;
        public bool mark
        {
            get { return _mark; }
            set { _mark = value; }
        }

        public Color color { get; set; } 

        public float duration { get; set; } 

        public float delay { get; set; } 


        public NumButton(Texture2D texture, SpriteFont font, Vector2 position, string number)
        {
            _texture = texture;
            _position = position;
            Rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            _number = number;
            _font = font;
            color=Color.White;
            duration=0;
            delay=200;
            visibility=false;
        }

        public void Update(GameTime gameTime)
        {
            if (visibility)
            {
                if (duration >= delay)
                {
                    duration = 0;
                    color = Color.White;
                    visibility = false;
                }
                else
                {
                    duration += gameTime.ElapsedGameTime.Milliseconds;
                    color = Color.DarkGray;
                    mark = true;
                }
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, Rectangle, color);
            spriteBatch.DrawString(_font, _number, new Vector2((int)_position.X + 25, (int)_position.Y + 25), Color.White);
        }

    }
}