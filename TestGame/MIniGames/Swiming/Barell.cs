using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testgame.MIniGames.Swiming
{
    public class Barell
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _velocityX;
        private float _velocityY;
        private float _scale;

        private GraphicsDevice _graphicDevice;

        private Random _random = new Random();
        private float _angle;

        public Rectangle Position
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_texture.Width * _scale),
                    (int)(_texture.Width * _scale));
            }
        }
        public Barell(Texture2D texture, Vector2 startPositon, GraphicsDevice graphicDevice, float scale = 100)
        {
            _texture = texture;
            _position = startPositon;
            _graphicDevice = graphicDevice;
            _scale = (scale / 100);
            _angle = _random.Next(0, 180);

        }
        public void Update(GameTime gameTime)
        {
            _position.Y += 0.8f;
            _position.X -= 2f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, _angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
