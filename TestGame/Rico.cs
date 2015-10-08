using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public class Rico
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private GraphicsDeviceManager _graphics;
        public Texture2D Image;
        private Vector2 _position = new Vector2(20, 20);
        
        public Vector2 position
        {
            get
            {
                return _position;

            }
            set
            {
                if (value != null)
                {
                    _position = value;
                }
                throw new ArgumentNullException("Rico`s positon is null!");
            }
        }

        public Rico(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("Null spriteBatch in Rico!");
            if (content == null)
                throw new ArgumentNullException("content is null in Rico!");
            if (graphics == null)
                throw new ArgumentNullException("graphics is null in Rico!");
            _spriteBatch = spriteBatch;
            _content = content;
            _graphics = graphics;
            Image = _content.Load<Texture2D>("RICO_2");
        }

        public void UpdatePositionLeft()
        {

            if ((_position.X) != _graphics.GraphicsDevice.Viewport.Bounds.Left)
            {
                _position.X -= 2;
            }

        }

        public void UpdatePositionRight()
        {

            if (_position.X + Image.Width != _graphics.GraphicsDevice.Viewport.Bounds.Right)
            {
                _position.X += 2;
            }

        }

        public void UpdatePositionUP()
        {
            if (_position.Y != _graphics.GraphicsDevice.Viewport.Bounds.Top)
            {
                _position.Y -= 2;
            }

        }

        public void UpdatePositionDown()
        {
            if ((_position.Y + Image.Height) < _graphics.GraphicsDevice.Viewport.Bounds.Bottom)
            {
                _position.Y += 2;
            }
            else
            {
                int i = 0;
            }
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Image, _position, Color.White);
            _spriteBatch.End();

        }
    }
}