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



        private KeyboardState _oldstate;
        private KeyboardState _currentState;

        private string _question = String.Empty;
        private string _answer = String.Empty;

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

        public void LoadContent(ContentManager content, Texture2D panelTexture)
        {
            _content = content;
            _texture = panelTexture; 

            _NumPanel = new TextLabel(new Vector2(100, 50), 50, String.Empty, _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), _content.Load<Texture2D>(@"Minigry\AutomatGame\PinPanel"));
            _NumPanel.alignment = TextLabel.Alignment.Center;

            _arrayOfNumButtons[0] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 2, (64 * 4) + 50), "0");
            _arrayOfNumButtons[1] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64, 64 + 50), "1");
            _arrayOfNumButtons[2] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 2, 64 + 50), "2");
            _arrayOfNumButtons[3] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 3, 64 + 50), "3");
            _arrayOfNumButtons[4] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64, 64 * 2 + 50), "4");
            _arrayOfNumButtons[5] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 2, 64 * 2 + 50), "5");
            _arrayOfNumButtons[6] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 3, 64 * 2 + 50), "6");
            _arrayOfNumButtons[7] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64, 64 * 3 + 50), "7");
            _arrayOfNumButtons[8] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 2, 64 * 3 + 50), "8");
            _arrayOfNumButtons[9] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 3, 64 * 3 + 50), "9");

            _arrayOfNumButtons[10] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64, 64 * 4 + 50), "#");
            _arrayOfNumButtons[11] = new NumButton(_content.Load<Texture2D>(@"Minigry\AutomatGame\Button"), _content.Load<SpriteFont>(@"Minigry\AutomatGame\NumFont"), new Vector2(64 * 3, 64 * 4 + 50), "*");


        }

        public void Update(GameTime gameTime)
        {
            if (GamePass)
            {
                return;
            }
            _oldstate = _currentState;
            _currentState = Keyboard.GetState();

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


                if ((_currentState.IsKeyDown(Keys.D0) && _oldstate.IsKeyUp(Keys.D0)) || (_currentState.IsKeyDown(Keys.NumPad0) && _oldstate.IsKeyUp(Keys.NumPad0)))
                {

                    _NumPanel.color = Color.Black;

                    _answer += 0;
                    _NumPanel.Text = _answer;
                }
                if ((_currentState.IsKeyDown(Keys.D1) && _oldstate.IsKeyUp(Keys.D1)) || (_currentState.IsKeyDown(Keys.NumPad1) && _oldstate.IsKeyUp(Keys.NumPad1)))
                {

                    _NumPanel.color = Color.Black;

                    _answer += 1;
                    _NumPanel.Text = _answer;
                }

                if ((_currentState.IsKeyDown(Keys.D2) && _oldstate.IsKeyUp(Keys.D2)) || (_currentState.IsKeyDown(Keys.NumPad2) && _oldstate.IsKeyUp(Keys.NumPad2)))
                {
                    _NumPanel.color = Color.Black;

                    _answer += 2;
                    _NumPanel.Text = _answer;

                }
                if ((_currentState.IsKeyDown(Keys.D3) && _oldstate.IsKeyUp(Keys.D3)) || (_currentState.IsKeyDown(Keys.NumPad3) && _oldstate.IsKeyUp(Keys.NumPad3)))
                {
                    _NumPanel.color = Color.Black;

                    _answer += 3;
                    _NumPanel.Text = _answer;

                }
                if ((_currentState.IsKeyDown(Keys.D4) && _oldstate.IsKeyUp(Keys.D4)) || (_currentState.IsKeyDown(Keys.NumPad4) && _oldstate.IsKeyUp(Keys.NumPad4)))
                {

                    _NumPanel.color = Color.Black;

                    _answer += 4;
                    _NumPanel.Text = _answer;
                }
                if ((_currentState.IsKeyDown(Keys.D5) && _oldstate.IsKeyUp(Keys.D5)) || (_currentState.IsKeyDown(Keys.NumPad5) && _oldstate.IsKeyUp(Keys.NumPad5)))
                {

                    _NumPanel.color = Color.Black;

                    _answer += 5;
                    _NumPanel.Text = _answer;
                }
                if ((_currentState.IsKeyDown(Keys.D6) && _oldstate.IsKeyUp(Keys.D6)) || (_currentState.IsKeyDown(Keys.NumPad6) && _oldstate.IsKeyUp(Keys.NumPad6)))
                {
                    _NumPanel.color = Color.Black;

                    _answer += 6;
                    _NumPanel.Text = _answer;

                }
                if ((_currentState.IsKeyDown(Keys.D7) && _oldstate.IsKeyUp(Keys.D7)) || (_currentState.IsKeyDown(Keys.NumPad7) && _oldstate.IsKeyUp(Keys.NumPad7)))
                {
                    _NumPanel.color = Color.Black;

                    _answer += 7;
                    _NumPanel.Text = _answer;

                }
                if ((_currentState.IsKeyDown(Keys.D8) && _oldstate.IsKeyUp(Keys.D8)) || (_currentState.IsKeyDown(Keys.NumPad8) && _oldstate.IsKeyUp(Keys.NumPad8)))
                {

                    _NumPanel.color = Color.Black;

                    _answer += 8;
                    _NumPanel.Text = _answer;
                }
                if ((_currentState.IsKeyDown(Keys.D9) && _oldstate.IsKeyUp(Keys.D9)) || (_currentState.IsKeyDown(Keys.NumPad9) && _oldstate.IsKeyUp(Keys.NumPad9)))
                {

                    _NumPanel.color = Color.Black;

                    _answer += 9;
                    _NumPanel.Text = _answer;
                }

             
                //if (_currentState.LeftButton == ButtonState.Pressed && _oldstate.LeftButton == ButtonState.Released)
                //{
                //    foreach (var buttons in _arrayOfNumButtons)
                //    {
                //        if (buttons.Rectangle.Contains(_currentState.X, _currentState.Y))
                //        {
                //            _NumPanel.color = Color.Black;

                //            _answer += buttons._number;
                //            _NumPanel.Text = _answer;
                //        }
                //    }
                //}
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
                item.Draw(spriteBatch);
            }
            _NumPanel.Draw(spriteBatch, true);

        }
    }
}