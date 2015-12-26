using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame
{
    public class Animation
    {
        /// <summary>
        /// event na zakonczenie animacji
        /// </summary>
        public event EventHandler<EventArgs> FinishAnimation;

        protected void OnFinishAnimation()
        {
            var tempHandler = FinishAnimation;
            if (tempHandler != null)
            {
                tempHandler(this, EventArgs.Empty);
            }
        }


        private readonly Texture2D _texture;
        private int _numberOfFrame;
        private readonly float _timeRefresh;

        private float _timeElapsed;
        private Vector2 _position;

        private float _widthOfFrame;
        private int _currentFrame = 0;

        private Rectangle _positionOnSheet;

        public Color color
        {
            get;
            set;
        }

        public Rectangle Position
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_widthOfFrame / 8),
                    (int)(_texture.Height / 8));
            }
            set
            {
                Position = value;
            }

        }

        /// <summary>
        /// color default value white
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="numberOfFrame"></param>
        /// <param name="timeRefresh"></param>
        /// <param name="startPosition"></param>
        public Animation(Texture2D texture, int numberOfFrame, float timeRefresh, Vector2 startPosition)
        {
            _texture = texture;
            _numberOfFrame = numberOfFrame;
            _timeRefresh = timeRefresh;
            _widthOfFrame = _texture.Width / _numberOfFrame;

            color = Color.White;

            _position = startPosition;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="newPosition"></param>
        public void Update(GameTime gametime, Rectangle newPosition)
        {

            _position.X = newPosition.X;
            _position.Y = newPosition.Y;

            _timeElapsed += gametime.ElapsedGameTime.Milliseconds;

            if (_timeElapsed >= _timeRefresh)
            {
                _currentFrame++;
                if (_currentFrame >= _numberOfFrame)
                {
                    _currentFrame = 0;
                    OnFinishAnimation();
                }
                _positionOnSheet = new Rectangle((int)_widthOfFrame * _currentFrame, 0, (int)_widthOfFrame, _texture.Height);
                _timeElapsed = 0;
            }
        }
    
        public void UpdateInStay(Vector2 newPosition)
        {
            _position = newPosition;


            _positionOnSheet = new Rectangle((int)_widthOfFrame * 0, 0, (int)_widthOfFrame, _texture.Height);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="flip">default fasle</param>
        public void Draw(SpriteBatch spriteBatch, bool flip = false)
        {
            if (flip)
            {
                spriteBatch.Draw(_texture, Position, _positionOnSheet, color, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, _positionOnSheet, color);
            }
        }


    }
}
