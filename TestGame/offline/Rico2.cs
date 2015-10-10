using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace TestGame
{
    public class Rico2
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private GraphicsDeviceManager _graphics;
        public Texture2D Image;
        private Vector2 _position;
        private Vector2 speed;
        bool jump = true;


        public Rico2(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics)
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
            _position =  new Vector2(20, 400);
        }

        public void UpdatePosition()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) speed.X = 3;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) speed.X = -3; else speed.X = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && jump == false)
            {
                _position.Y -= 10;
                speed.Y = -10;
                jump = true;
            
            }

            if(jump == true) speed.Y += (float)0.15;

            if(_position.Y + Image.Height >= 650) jump = false;

            if (jump == false) speed.Y = 0;


            _position += speed;
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
            _spriteBatch.Draw(Image, _position, Color.White);
        }
    }
}