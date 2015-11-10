﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniGameSwiming
{
    public class Fish
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Color _color;

        public bool Eaten
        {
            get; set;
        }

        public Rectangle Position
        {
            get { return new Rectangle((int) _position.X, (int) _position.Y, _texture.Width,_texture.Height); }
        }
        public bool Poision
        {
            get; set;
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private float _velocityX;
        private float _velocityY;

        public Fish(Texture2D texture, Vector2 position,bool poision=false)
        {
            _texture = texture;
            _position = position;
            _color=Color.White;
            _velocityX = 2;
            _velocityY = 1;
            Poision = poision;
            Eaten = false;
        }

        public void Update(GameTime gameTime)
        {
            _position.X -= _velocityX;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _color);
        }
    }
}