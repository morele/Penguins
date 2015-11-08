using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MiniGameSwiming
{
    public class Swiming
    {
        private List<Fish> _listOfFish;
        private Pinguin _pinguin;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphics;
        private ContentManager _content;

        private float duration = 0f;
        private float delay = 2000f;

        private Random _random;

        public Swiming(SpriteBatch spriteBatch, ContentManager content,GraphicsDevice graphicsDevice)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            _graphics = graphicsDevice;

            _random=new Random();

            _pinguin=new Pinguin(_content.Load<Texture2D>("Rico"),new Vector2(80,81),10);
            _listOfFish = new List<Fish>();
        }



        public void Update(GameTime gameTime)
        {
            if (duration > delay)
            {
                duration = 0;
                int randNumber = _random.Next(1, 10);
                //for (int i = 0; i < randNumber; i++)
                //{
                    _listOfFish.Add(new Fish(_content.Load<Texture2D>("dorsz"), new Vector2(_graphics.Viewport.Width - 80, (randNumber * 100) - _random.Next(0, (int)_graphics.Viewport.Height))));
              //  }

              
                
            }
            else
            {
                duration += gameTime.ElapsedGameTime.Milliseconds;
             
            }


            foreach (Fish fish in _listOfFish)
            {
                fish.Update(gameTime);
            }

            _pinguin.Update(gameTime,_graphics);
        }


        public void Draw()
        {
            foreach (var item in _listOfFish)
            {
                item.Draw(_spriteBatch);
            }
            _pinguin.Draw(_spriteBatch);
        }
    }
}