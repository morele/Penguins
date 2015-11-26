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

        private float _greenDuration = 0;
        private float _greenDelay = 10000;

        public Color color
        {
           get;
          set;
        }

        public bool Run
        {
            get;
            set;
        }

        public int NumberOfLife
        {
            get;
            set;
        }

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
            _velocityY = 3;
            _velocityX = 3;
            NumberOfLife = 3;
            color = Color.White;
        }

        public void Update(GameTime gameTime, GraphicsDevice device)
        {
            if (!Run)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Run = true;
                    color=Color.White;
                }
            }
            if (Run)
            {
                if (_greenDuration > _greenDelay)
                {
                    color = Color.White;
                    _greenDuration = 0;
                }
                else
                {
                    _greenDuration += gameTime.ElapsedGameTime.Milliseconds;
                }

                if (_position.X <= 0)
                {
                    _position.X = 1;
                }
                if (_position.X > device.Viewport.X + _texture.Width - 100)
                {
                    _position.X = device.Viewport.X + _texture.Width - 100;
                }
                if (_position.Y <= 0)
                {
                    _position.Y = 1;
                }
                if (_position.Y > device.Viewport.Y + _texture.Height - 100)
                {

                    NumberOfLife--;
                    System.Diagnostics.Debug.WriteLine("Life: {0}", NumberOfLife);
                    _position.X = 10;
                    _position.Y = 10;
                    Run = false;
                }
                _position.Y += _velocityY;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {

                    _position.Y -= 5;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    _position.X -= _velocityX;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    _position.X += _velocityX;
                }
            }
           

        }

        public void Draw(SpriteBatch spriteBatch)
        {

           
            spriteBatch.Draw(_texture, Position, color);
        }
    }
}