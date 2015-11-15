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
        private List<Fish> _listOfBadFish;
        private Pinguin _pinguin;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphics;
        private ContentManager _content;

        private float duration = 0f;
        private float delay = 2000f;

        private float durationyOfBadFish = 0;
        private float delayofBadFish = 30000f;

        private Random _random;

        private SpriteFont _font;

        private int punkty = 0;
        private string _gameText = "Press space to play!";

        public Swiming(SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphicsDevice)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            _graphics = graphicsDevice;

            _random = new Random();

            _pinguin = new Pinguin(_content.Load<Texture2D>("Rico"), new Vector2(80, 81), 10);
            _font = content.Load<SpriteFont>("Fon");
            _listOfFish = new List<Fish>();
            _listOfBadFish = new List<Fish>();
        }



        public void Update(GameTime gameTime)
        {
            if (!_pinguin.Run)
            {
                _gameText = "Press space to play!";
            }
            else
            {
                _gameText =String.Format("Number of Life: {0}",_pinguin.NumberOfLife);

                if (duration > delay)
                {
                    duration = 0;
                    int randNumber = _random.Next(1, 10);

                    _listOfFish.Add(new Fish(_content.Load<Texture2D>("dorsz"), new Vector2(_graphics.Viewport.Width, (randNumber * 100) - _random.Next(80, (int)_graphics.Viewport.Height) - 80)));
                }
                else
                {
                    duration += gameTime.ElapsedGameTime.Milliseconds;

                }
                if (durationyOfBadFish > delayofBadFish)
                {
                    durationyOfBadFish = 0;
                    delayofBadFish = _random.Next(800, 6666);
                    int randNumber = _random.Next(1, 10);
                    _listOfBadFish.Add(new Fish(_content.Load<Texture2D>("zly Dorsz"), new Vector2(_graphics.Viewport.Width, (randNumber * 100) - _random.Next(80, (int)_graphics.Viewport.Height) - 80), true));
                }
                else
                {
                    durationyOfBadFish += gameTime.ElapsedGameTime.Milliseconds;
                }

                foreach (Fish fish in _listOfFish)
                {
                    fish.Update(gameTime);
                    if (fish.Position.Intersects(_pinguin.Position))
                    {
                        if (!fish.Eaten)
                        {
                            punkty++;
                            System.Diagnostics.Debug.WriteLine("Pkt: {0}", punkty);

                        }

                        fish.Eaten = true;


                    }
                }
                foreach (Fish Badfish in _listOfBadFish)
                {
                    Badfish.Update(gameTime);
                    if (Badfish.Position.Intersects(_pinguin.Position))
                    {
                        if (!Badfish.Eaten)
                        {
                            punkty--;

                            _pinguin.color = Color.GreenYellow;

                            System.Diagnostics.Debug.WriteLine("Pkt: {0}", punkty);
                        }

                        Badfish.Eaten = true;


                    }
                }
            }
 


         
            _pinguin.Update(gameTime, _graphics);
        }


        public void Draw()
        {
            foreach (var fish in _listOfFish)
            {
                if(!fish.Eaten)
                    fish.Draw(_spriteBatch);

            }
            foreach (var badFish in _listOfBadFish)
            {
                if (!badFish.Eaten)
                    badFish.Draw(_spriteBatch);

            }
            _pinguin.Draw(_spriteBatch);
            _spriteBatch.DrawString(_font, _gameText,new Vector2(300,30),Color.Black );
        }
    }
}