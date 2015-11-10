using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MiniGameSwiming
{
    public class Pinguin
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _velocityX;
        private float _velocityY;
        private float _scale;

        public Rectangle Position
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_texture.Width * _scale),
                    (int)(_texture.Width * _scale));
            }
        }
        /// <summary>
        /// podajemy w procentach 
        /// </summary>

        public float Scale
        {
            get
            {
                return _scale;

            }
            set
            {
                _scale = value / 100;
            }

        }

        public Pinguin(Texture2D texture, Vector2 position, float scale = 100)
        {
            _texture = texture;
            _position = position;
            _scale = scale / 100;
            _velocityY = 2;
            _velocityX = 3;

        }

        public void Update(GameTime gametdTime, GraphicsDevice device)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {

                _position.Y -= 4;
            }
          //  if (Keyboard.GetState().IsKeyDown(Keys.Down))
         //   {
                _position.Y += _velocityY;
         //   }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _position.X -= _velocityX;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _position.X += _velocityX;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}