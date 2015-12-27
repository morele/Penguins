using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Testgame.MIniGames.Swiming
{
    public class Swiming
    {
        private List<Fish> _listOfFish;
        private List<Fish> _listOfBadFish;
        private List<Barell> _listOfBarell;
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



            //   _barell = new Barell(content.Load<Texture2D>("Beczka"), new Vector2(80, 80), graphicsDevice,20);

            _pinguin = new Pinguin(_content.Load<Texture2D>("Minigry/SwimingGame/RicoPlywa"), _content.Load<Texture2D>("Minigry/SwimingGame/dziob"), new Vector2(80, 81), 10);
            _font = content.Load<SpriteFont>("JingJing");
            _listOfFish = new List<Fish>();
            _listOfBadFish = new List<Fish>();
            _listOfBarell = new List<Barell>();
        }



        public void Update(GameTime gameTime)
        {
            if (!_pinguin.Run)
            {
                _gameText = "Press space to play!";
                _pinguin.Update(_graphics);
            }
            else
            {
                _gameText = String.Format("Number of Life: {0}", _pinguin.NumberOfLife);

                if (duration > delay)
                {
                    duration = 0;
                    int randNumber = _random.Next(1, 10);

                    _listOfFish.Add(new Fish(_content.Load<Texture2D>("Minigry/SwimingGame/ryba"), new Vector2(_graphics.Viewport.Width, (randNumber * 100) - _random.Next(80, (int)_graphics.Viewport.Height) - 90)));
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
                    _listOfBadFish.Add(new Fish(_content.Load<Texture2D>("Minigry/SwimingGame/ZepsutaRyba"), new Vector2(_graphics.Viewport.Width, (randNumber * 100) - _random.Next(80, (int)_graphics.Viewport.Height) - 90), true));
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
                            _pinguin.Eating = true;
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
                            _pinguin.Eating = true;
                            punkty--;

                            _pinguin.color = Color.GreenYellow;

                            System.Diagnostics.Debug.WriteLine("Pkt: {0}", punkty);
                        }

                        Badfish.Eaten = true;


                    }
                }
                if (_random.Next(0, 100) == 7)
                {
                    int randNumber = _random.Next(1, 10);
                    _listOfBarell.Add(new Barell(_content.Load<Texture2D>("Minigry/SwimingGame/Beczka"), new Vector2(_graphics.Viewport.Width, (randNumber * 100) - _random.Next(80, (int)_graphics.Viewport.Height) - 90), _graphics, 20));
                }


                foreach (var barell in _listOfBarell)
                {
                    barell.Update(gameTime);
                }

                foreach (Barell barell in _listOfBarell)
                {
                    if (_pinguin.Position.Contains(barell.Position))
                    {
                        _pinguin.Run = false;
                        _listOfBarell.Clear();
                        _listOfBadFish.Clear();
                        _listOfFish.Clear();
                        break;
                    }
                }
                _pinguin.Update(gameTime, _graphics);
            }





        }


        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            foreach (var fish in _listOfFish)
            {
                if (!fish.Eaten)
                    fish.Draw(_spriteBatch);

            }
            foreach (var badFish in _listOfBadFish)
            {
                if (!badFish.Eaten)
                    badFish.Draw(_spriteBatch);

            }
            foreach (var barell in _listOfBarell)
            {
                barell.Draw(_spriteBatch);
            }




            _pinguin.Draw(_spriteBatch);
            _spriteBatch.DrawString(_font, _gameText, new Vector2(300, 30), Color.Black);
        }
    }
}