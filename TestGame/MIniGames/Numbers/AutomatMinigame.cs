using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame.MIniGames.Numbers
{
    public class AutomatMinigame
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

        private TextLabel _NumPanel;

        /// <summary>
        /// jezelu gamePass jest na true to koncz gre tylko get
        /// </summary>
        public bool GamePass
        {
            get;
            private set;
        }


        public AutomatMinigame()
        {
            _position = new Vector2(20, 20);
            _arrayOfNumButtons = new NumButton[12];
            GamePass = false;

        }

        private void LoadContent(ContentManager content, Texture2D panelTexture)
        {
            _content = content;
            _texture = panelTexture;

            _NumPanel = new TextLabel(new Vector2(100, 50), 50, String.Empty, _content.Load<SpriteFont>("Digit"), _content.Load<Texture2D>("PinPanel"));
            _NumPanel.alignment = TextLabel.Alignment.Center;

            _arrayOfNumButtons[0] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, (64 * 4) + 50), "0");
            _arrayOfNumButtons[1] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 + 50), "1");
            _arrayOfNumButtons[2] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64 + 50), "2");
            _arrayOfNumButtons[3] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 + 50), "3");
            _arrayOfNumButtons[4] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 * 2 + 50), "4");
            _arrayOfNumButtons[5] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64 * 2 + 50), "5");
            _arrayOfNumButtons[6] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 * 2 + 50), "6");
            _arrayOfNumButtons[7] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 * 3 + 50), "7");
            _arrayOfNumButtons[8] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 2, 64 * 3 + 50), "8");
            _arrayOfNumButtons[9] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 * 3 + 50), "9");

            _arrayOfNumButtons[10] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64, 64 * 4 + 50), "#");
            _arrayOfNumButtons[11] = new NumButton(_content.Load<Texture2D>("Button"), _content.Load<SpriteFont>("fontNumber"), new Vector2(64 * 3, 64 * 4 + 50), "*");


        }

        public void Update(GameTime gameTime)
        {
            if (GamePass)
            {
                return;
            }
            _oldstate = _currentState;
            _currentState = Mouse.GetState();

            if (GamePass)
            {
                return;
            }
            if (_couter < 4)
            {
                if (duration <= delay)
                {
                    duration += gameTime.ElapsedGameTime.Milliseconds;

                }
                else
                {
                    int buff = _random.Next(0, 10);
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
                            _NumPanel.color = Color.Black;

                            _answer += buttons._number;
                            _NumPanel.Text = _answer;
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
                    _NumPanel.color = Color.Green;
                    GamePass = true;

                }
                else
                {
                    _couter = 0;
                    _question = _answer = string.Empty;
                    duration = 0;
                    Debug.WriteLine("Blad!");
                    _NumPanel.color = Color.Red;
                    _NumPanel.Text = "Error!";
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, 400, 640), Color.White);
            foreach (var item in _arrayOfNumButtons)
            {
                item.Draw(_spriteBatch);
            }
            _NumPanel.Draw(_spriteBatch, true);

        }
    }
}