using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace NumberMiniGame.Minigame
{
    public class Panel
    {

        private Texture2D _texture;
        private Vector2 _position;
        private NumButton[] _arrayOfNumButtons;
        private Random _random = new Random();
        private int _couter = 0;
        private float duration = 0;
        private float delay = 1000;

        private MouseState _oldstate;
        private MouseState _currentState;

        private string _question = String.Empty;
        private string _answer = String.Empty;

        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        public Panel(SpriteBatch spriteBatch, ContentManager content, Texture2D panelTexture)
        {
            _position = new Vector2(20, 20);
            _arrayOfNumButtons = new NumButton[12];
            _spriteBatch = spriteBatch;
            _content = content;
            _texture = panelTexture;
            LoadContent();

        }

        private void LoadContent()
        {



            _arrayOfNumButtons[0] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64 * 4), "0");
            _arrayOfNumButtons[1] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64), "1");
            _arrayOfNumButtons[2] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64), "2");
            _arrayOfNumButtons[3] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64), "3");
            _arrayOfNumButtons[4] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 * 2), "4");
            _arrayOfNumButtons[5] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64 * 2), "5");
            _arrayOfNumButtons[6] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 * 2), "6");
            _arrayOfNumButtons[7] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 * 3), "7");
            _arrayOfNumButtons[8] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64 * 3), "8");
            _arrayOfNumButtons[9] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 * 3), "9");

            _arrayOfNumButtons[10] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 * 4), "#");
            _arrayOfNumButtons[11] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 * 4), "*");


        }

        public void Run(GameTime gameTime)
        {
            Update(gameTime);
            Draw();
        }
        private void Update(GameTime gameTime)
        {
            _oldstate = _currentState;
            _currentState = Mouse.GetState();


            if (_couter < 4)
            {
                if (duration <= delay)
                {
                    duration += gameTime.ElapsedGameTime.Milliseconds;

                }
                else
                {
                    int buff = _random.Next(0, 11);
                    _arrayOfNumButtons[buff].visibility = true;
                    _question += buff;
                    Debug.WriteLine(buff);
                    _couter++;
                    duration = 0;
                }
            }
            else
            {
                if (_currentState.LeftButton == ButtonState.Pressed && _oldstate.LeftButton == ButtonState.Released)
                {
                    foreach (var buttons in _arrayOfNumButtons)
                    {
                        if (buttons.Rectangle.Contains(_currentState.X, _currentState.Y))
                        {
                            _answer += buttons._number;
                        }
                    }
                }
            }


            foreach (var item in _arrayOfNumButtons)
            {
                item.Update(gameTime);
            }
            if (_answer.Length == 4)
            {
                if (Equals(_answer, _question))
                {
                    Debug.WriteLine("Wygrales!");

                }
                else
                {
                    _couter = 0;
                    _question = _answer = string.Empty;
                    duration = 0;
                    Debug.WriteLine("Blad!");
                }
            }

        }

        private void Draw()
        {
           
            _spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, 400, 640), Color.White);
            foreach (var item in _arrayOfNumButtons)
            {
                item.Draw(_spriteBatch);
            }
         
        }
    }
}