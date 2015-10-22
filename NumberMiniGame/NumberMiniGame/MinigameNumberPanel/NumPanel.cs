using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
namespace NumberMiniGame.MinigameNumberPanel
{
    public class NumPanel
    {
        private NumButton[] _numButtonArray;
        private Texture2D _texture;
        private readonly Rectangle _position;
        private readonly Color _color;
        private float delay = 1000;
        private float delayOf = 500;
        private float duration = 0;
        private string _number;
        private Random _rand = new Random();
        private int _couter = 0;
        public NumPanel(Rectangle position, Color color)
        {

            _position = position;
            _color = color;

            int number = _rand.Next(999, 9999);
            _number = number.ToString();

            _numButtonArray = new NumButton[12];
            _numButtonArray[0] = new NumButton(new Vector2(196, 412), Color.White, "0");
            _numButtonArray[1] = new NumButton(new Vector2(65, 75), Color.White, "1");
            _numButtonArray[2] = new NumButton(new Vector2(196, 75), Color.White, "2");
            _numButtonArray[3] = new NumButton(new Vector2(325, 75), Color.White, "3");
            _numButtonArray[4] = new NumButton(new Vector2(65, 186), Color.White, "4");
            _numButtonArray[5] = new NumButton(new Vector2(196, 186), Color.White, "5");
            _numButtonArray[6] = new NumButton(new Vector2(325, 186), Color.White, "6");
            _numButtonArray[7] = new NumButton(new Vector2(65, 300), Color.White, "7");
            _numButtonArray[8] = new NumButton(new Vector2(196, 300), Color.White, "8");
            _numButtonArray[9] = new NumButton(new Vector2(325, 300), Color.White, "9");
            _numButtonArray[10] = new NumButton(new Vector2(65, 412), Color.White, "*");

            _numButtonArray[11] = new NumButton(new Vector2(325, 412), Color.White, "#");

        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Panel");
            foreach (NumButton numButton in _numButtonArray)
            {
                numButton.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_couter < _number.Length)
            {
                var index = Convert.ToInt32(_number[_couter].ToString());

                if (delay <= duration)
                {
                    duration = 0;
                    _numButtonArray[index].Update(gameTime);
                    Debug.WriteLine("zmieniono kolor " + index);
                    _couter++;
                }
                else
                {
                    _couter = 0;
                    duration += gameTime.ElapsedGameTime.Milliseconds;

                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _color);
            foreach (NumButton numButton in _numButtonArray)
            {
                numButton.Draw(spriteBatch);
            }
        }

    }
}