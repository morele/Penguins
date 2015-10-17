using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public class TextLabel : IDrawableTextLabel
    {
        private Rectangle _position;
        private Vector2 _positionOfText;
        private string _text;
        private Texture2D _texture;
        private string _textureName;
        private SpriteFont _spriteFont;

        public TextLabel(Rectangle position, string text, string textureName)
        {
            if (position == null || text == null || textureName == null)
                throw new ArgumentNullException("Null Argument | TextLabel");
            _position = position;
            _text = text;
            _textureName = textureName;
            _positionOfText = new Vector2(_position.X/2, _position.Y/2);

        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteFont = contentManager.Load<SpriteFont>("JingJing");
            _texture = contentManager.Load<Texture2D>(_textureName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
            spriteBatch.DrawString(_spriteFont, _text, _positionOfText, Color.Black);
        }

        public void Update(GameTime gameTime, string text)
        {
            _text = text;
        }
    }
}