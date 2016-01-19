using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame.Menu
{
    public class PauseMenu
    {
        private Texture2D _levelName;
        private Texture2D _levelNumber;
        private Texture2D _menuBackground;
        private Texture2D _menuResume;
        private Texture2D _menuExit;
        private Texture2D _menuTryAgain;
        private Texture2D _menuPointer;

        private Vector2 _menuPointerPosition;
        private Vector2 _menuPosition;
        private Vector2 _menuItemPosition;
        private GraphicsDevice _graphicsDevice;

        private bool _blockDownKey;
        private bool _blockUpKey;
        
        private int _selectedMenuItem = 0;
        private bool _blockEnterKey;

        public bool ShowMenu { get; set; }

        public SelectedOptionPauseMenu SelectedOptionPauseMenu { get; set; }

        public PauseMenu(
            GraphicsDevice graphicsDevice,
            Texture2D levelName, 
            Texture2D levelNumber, 
            Texture2D menuPointer, 
            Texture2D menuBackground, 
            Texture2D menuResume, 
            Texture2D menuExit, 
            Texture2D menuTryAgain)
        {
            _graphicsDevice = graphicsDevice;
            _levelName = levelName;
            _levelNumber = levelNumber;
            _menuBackground = menuBackground;
            _menuResume = menuResume;
            _menuExit = menuExit;
            _menuTryAgain = menuTryAgain;
            _menuPointer = menuPointer;
            _menuPosition = Vector2.Zero;
            _menuItemPosition = Vector2.Zero;

            _menuPosition.Y = graphicsDevice.Viewport.Height / 2 - _menuBackground.Height / 2;
            _menuPosition.X = graphicsDevice.Viewport.Width / 2 - _menuBackground.Width / 2;

            // wszystkie elementy menu pauzy mają tą samą wysokość, więc wziąłem sobie _menuExit
            _menuItemPosition.Y = graphicsDevice.Viewport.Height / 2 - _menuExit.Height / 2;
            _menuItemPosition.X = graphicsDevice.Viewport.Width / 2 - _menuExit.Width / 2;
            _menuPointerPosition.X = _menuItemPosition.X - 120;
            _menuPointerPosition.Y = 310;

            
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_blockDownKey)
            {
                _blockDownKey = true;
                if (_selectedMenuItem < 2)
                {
                    _menuPointerPosition.Y += 100;
                    _selectedMenuItem++;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_blockUpKey)
            {
                _blockUpKey = true;
                if (_selectedMenuItem > 0)
                {
                    _menuPointerPosition.Y -= 100;
                    _selectedMenuItem--;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_blockEnterKey)
            {
                _blockEnterKey = true;
                switch (_selectedMenuItem)
                {
                    case 0:
                        SelectedOptionPauseMenu = SelectedOptionPauseMenu.Resume;
                        break;
                    case 1:
                        SelectedOptionPauseMenu = SelectedOptionPauseMenu.TryAgain;
                        break;
                    case 2:
                        SelectedOptionPauseMenu = SelectedOptionPauseMenu.BackToMenu;
                        break;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                _blockEnterKey = false;

            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                _blockUpKey = false;

            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                _blockDownKey = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(_menuBackground, _menuPosition, Color.White);

            float xAxis = _menuItemPosition.X+2;
            float yStart = 310;
            spriteBatch.Draw(_levelNumber, new Vector2(xAxis - 5, yStart - 250), Color.White);
            spriteBatch.Draw(_levelName, new Vector2(xAxis+20, yStart - 150), Color.White);
            spriteBatch.Draw(_menuResume, new Vector2(xAxis, yStart), Color.White);
            spriteBatch.Draw(_menuTryAgain, new Vector2(xAxis, yStart + 100), Color.White);
            spriteBatch.Draw(_menuExit, new Vector2(xAxis, yStart + 200), Color.White);
            spriteBatch.Draw(_menuPointer, _menuPointerPosition, Color.White);
            spriteBatch.End();
        }
    }
}
