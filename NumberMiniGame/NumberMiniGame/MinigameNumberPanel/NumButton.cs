using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NumberMiniGame.MinigameNumberPanel
{
    public class NumButton
    {
        private Rectangle _position;
        private Texture2D _texture;
        private Color _color;
        private Color _colorOfText;
        private string _text;
        private SpriteFont _spriteFont;
        private readonly Vector2 _positionOfText;
        private Vector2 _buffPosition ;

        public float duration { get; set; } = 0;

        public float delay
        {
            get; set;
        } = 200;
        public NumButton(Vector2 position, Color color,string text)
        {
            _buffPosition = position;

            _color = color;
            _text = text;
            _colorOfText = Color.Black;
            _positionOfText=new Vector2(_position.Width,_position.Height);
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Color ColorOfText
        {
            get { return _colorOfText; }
            set { _colorOfText = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Rectangle Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("button");
            _spriteFont = content.Load<SpriteFont>("fontNumber");
            _position=new Rectangle((int)_buffPosition.X,(int)_buffPosition.Y,_texture.Width,_texture.Height);
        }

        public void Update(GameTime gametime)
        {
            if (duration >= delay)
            {
                duration += gametime.ElapsedGameTime.Milliseconds;
                _color = Color.Yellow;
            }
            else
            {
                duration = 0;
                _color = Color.White;
            }
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position,_color);
            spriteBatch.DrawString(_spriteFont,_text,new Vector2(_position.X+25,_position.Y+25), _colorOfText);
        }


    }
}
