using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame;

namespace Testgame.MIniGames.Swiming
{
    public class Pinguin
    {
        private Texture2D _texture;
        private Texture2D _textureEating;
        private Vector2 _position;
        private float _velocityX;
        private float _velocityY;
        private float _scale;

        private float _greenDuration = 0;
        private float _greenDelay = 1500;

        private Rectangle _positionOnSheet;

        private float _viewPortHeigth;

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
                return new Rectangle((int)_position.X, (int)_position.Y, ((int)(_widthOfShoot * Scale)),
                    (int)(_texture.Height * Scale));
            }
        }
        private const int _widthOfShoot = 991;
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


        private int _numberOfFrame = 0;

        private Animation _ricoSwiming;
        private Animation _ricoEating;

        public bool Eating
        {
            get;
            set;
        }

        public Pinguin(Texture2D texture, Texture2D textureEating, Vector2 position, float scale = 100)
        {
            _texture = texture;
            _textureEating = textureEating;
            _position = position;
            _scale = scale / 100;
            _velocityY = 3;
            _velocityX = 3;
            NumberOfLife = 4;
            color = Color.White;

            _ricoSwiming = new Animation(_texture, 14, 50, _position);


            _ricoEating = new Animation(_textureEating, 6, 50, _position);
            _ricoEating.FinishAnimation += _ricoEating_FinishAnimation;


        }

        private void _ricoEating_FinishAnimation(object sender, System.EventArgs e)
        {
            Eating = !Eating;
        }
        public void Update(GraphicsDevice device)
        {
            _ricoSwiming.UpdateInStay(Position);
            if (!Run)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Run = true;
                    color = Color.White;
                }
            }
        }
        public void Update(GameTime gameTime, GraphicsDevice device)
        {
            if (!Run)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Run = true;
                    color = Color.White;
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

                if (_position.X <= -1200)
                {
                    _position.X = -1198;
                }
                if (_position.X > device.Viewport.X + _texture.Width - 100)
                {
                    _position.X = device.Viewport.X + _texture.Width - 100;
                }
                if (_position.Y <= 200)
                {
                    _position.Y = 201;
                }
                if (_position.Y > 800)
                {

                    NumberOfLife --;
                   
                    System.Diagnostics.Debug.WriteLine("Life: {0}", NumberOfLife);
                    SetStartPosition();
                    Run = false;
                }
                _position.Y += _velocityY;


                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !(color == Color.GreenYellow))
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
            if (_numberOfFrame >= 14)
            {
                _numberOfFrame = 0;
            }

            if (Eating)
            {
                //  _ricoEating.Position = _position;
                _ricoEating.Update(gameTime, Position);
            }
            else
            {

                _ricoSwiming.Update(gameTime, Position);
            }

            _ricoSwiming.color = color;
            _ricoEating.color = color;
        }

        public void SetStartPosition()
        {
            _position.X = -1150;
            _position.Y = 200;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Eating)
            {
                _ricoEating.Draw(spriteBatch);
            }
            else
            {
                _ricoSwiming.Draw(spriteBatch);
            }


        }
    }
}