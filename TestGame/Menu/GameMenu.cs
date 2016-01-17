using System.Collections.Generic;
using System.Media;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Content;

namespace TestGame.Menu
{
    public class GameMenu
    {
        private const float VOLUME_STEP = 19.2f;
        private  Vector2 VOLUME_POINTER_START_POSITION = new Vector2(90, 15);
        private  Vector2 VOLUME_POINTER_END_POSITION = new Vector2(287, 15);

        private GraphicsDevice _graphicsDevice;

        private int _menuElementsCount = 3;
        private int _selectedMenuItemIndex;

        // flaga blokująca trzymanie klawisza
        private bool _blockUpKey = false;
        private bool _blockDownKey = false;
        private bool _blockSoundOn;
        private bool _blockSoundUp;
        private bool _blockSoundDown;

        
        private Rectangle _backgroundRectangle;

        private Texture2D _backgroundTexture;

        private Point _cursorPosition;
        private Rectangle _cursorRectangle;
        private Texture2D _cursorTexture;

        private Rectangle _newGameOptionRectangle;
        private Texture2D _newGameOptionTexture;

        private Rectangle _exitOptionRectangle;
        private Texture2D _exitOptionTexture;

        private Rectangle _optionOptionRectangle;
        private Texture2D _optionOptionTexture;

        private Rectangle _authorsOptionRectangle;
        private Texture2D _authorsOptionTexture;

        private bool _blockEnterKey;

        // dla dźwięku
        private Rectangle _soundIconRectangle;
        private Texture2D _soundOnTexture;
        private Vector2 _soundIconPosition = Vector2.Zero;
        private Vector2 _soundProgressBarPosition = Vector2.Zero;
        private Texture2D _soundOffTexture;
        private Rectangle _soundProgressBarRectangle;
        private Texture2D _soundProgressBarTexture;
        private Rectangle _soundProgressBarPointerRectangle;
        private Texture2D _soundProgressBarPointerTexture;
        private Vector2 _soundValuePointer;
        private SoundEffect _chooseMenuItemEffect;
        private SoundEffectInstance _chooseMenuItemEffectInstance;
        private Song _themeSong;

        public bool IsShowing { get; set; }


        public SelectedOptionMenu SelectedOptionMenu { get; set; }

        public GameMenu(ContentManager content, GraphicsDevice device)
        {
            IsShowing = true;
            _graphicsDevice = device;
            _selectedMenuItemIndex = 0;

            // grafiki menu
            _backgroundTexture = content.Load<Texture2D>("menu_background_new");
            _newGameOptionTexture = content.Load<Texture2D>("NowaGra");
            _optionOptionTexture = content.Load<Texture2D>("Opcje");
            _authorsOptionTexture = content.Load<Texture2D>("Autorzy");
            _exitOptionTexture = content.Load<Texture2D>("Wyjscie");
            _cursorTexture = content.Load<Texture2D>("Wskaznik");

            // grafiki obsługi dźwięku
            _soundOnTexture = content.Load<Texture2D>(@"Audio\Graph\soundOn");
            _soundOffTexture = content.Load<Texture2D>(@"Audio\Graph\soundOff");
            _soundIconRectangle = new Rectangle(0, 0, _soundOnTexture.Width, _soundOnTexture.Height);

            _soundProgressBarPointerTexture = content.Load<Texture2D>(@"Audio\Graph\barPointer");
            _soundProgressBarPointerRectangle = new Rectangle(0, 0,
                _soundProgressBarPointerTexture.Width,
                _soundProgressBarPointerTexture.Height);

            _soundProgressBarTexture = content.Load<Texture2D>(@"Audio\Graph\progressBar");
            _soundProgressBarRectangle = new Rectangle(0, 0,
                _soundProgressBarTexture.Width,
                _soundProgressBarTexture.Height);

            // dźwięki
            _chooseMenuItemEffect = content.Load<SoundEffect>(@"Audio\Waves\click");
            _chooseMenuItemEffectInstance = _chooseMenuItemEffect.CreateInstance();
            _chooseMenuItemEffectInstance.Volume = SoundManager.Volume;
            _themeSong = content.Load<Song>("Audio/Waves/menu_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_themeSong);
            SoundManager.SoundOn = true;
            SoundManager.Volume = 1.0f;

            //rectangle
            _newGameOptionRectangle = new Rectangle(0, 0, _newGameOptionTexture.Width, _newGameOptionTexture.Height);
            _optionOptionRectangle = new Rectangle(0, 0, _optionOptionTexture.Width, _optionOptionTexture.Height);
            _authorsOptionRectangle = new Rectangle(0, 0, _authorsOptionTexture.Width, _authorsOptionTexture.Height);
            _exitOptionRectangle = new Rectangle(0, 0, _exitOptionTexture.Width, _exitOptionTexture.Height);
            _cursorRectangle = new Rectangle(0, 0, _cursorTexture.Width, _cursorTexture.Height);


            float x = device.Viewport.Width / 2 - (_newGameOptionTexture.Width / 2) - 10 - _cursorTexture.Width;
            _cursorPosition = new Point((int)x, 300);

            _soundIconPosition.X = device.Viewport.Width - _soundIconRectangle.Width / 2 - 10;
            _soundIconPosition.Y = 10;

            _soundProgressBarPosition.X = device.Viewport.Width - _soundProgressBarRectangle.Width / 3 - 50;
            _soundProgressBarPosition.Y = 12;

            VOLUME_POINTER_START_POSITION.X = _soundProgressBarPosition.X + 25;
            VOLUME_POINTER_START_POSITION.Y = 22;

            VOLUME_POINTER_END_POSITION.X = (VOLUME_POINTER_START_POSITION.X + 10) + 178;
            VOLUME_POINTER_END_POSITION.Y = 22;

            _soundValuePointer = VOLUME_POINTER_END_POSITION;

        }

        public void Update()
        {
            #region DŹWIĘK

            MediaPlayer.Volume = SoundManager.Volume;
    
            // podgłośnij klawisz '+' 
            if (Keyboard.GetState().IsKeyDown(Keys.Add) && !_blockSoundUp)
            {
                _blockSoundUp = true;
                if (SoundManager.Volume < 1.0f)
                {
                    _soundValuePointer.X += VOLUME_STEP;
                    SoundManager.Volume += 0.1f;
                }
                else
                {
                    _soundValuePointer = VOLUME_POINTER_END_POSITION;
                }
                MediaPlayer.Resume();
                SoundManager.SoundOn = true;
            }

            // ścisz klawisz '-'
            if (Keyboard.GetState().IsKeyDown(Keys.Subtract) && !_blockSoundDown)
            {
                _blockSoundDown = true;
                if (SoundManager.Volume >= 0.0f)
                {
                    _soundValuePointer.X -= VOLUME_STEP;
                    SoundManager.Volume -= 0.1f;
                }
                if (SoundManager.Volume <= 0.1f)
                {
                    SoundManager.Volume = 0.0f;
                    _soundValuePointer = VOLUME_POINTER_START_POSITION;
                    SoundManager.SoundOn = false;
                }
            }

            // włącz/wyłącz dźwięk
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && !_blockSoundOn)
            {
                SoundManager.SoundOn = !SoundManager.SoundOn;
                
                _blockSoundOn = true;

                // odtworzenie dźwięku tła jeśli jest to włączone
                if (SoundManager.SoundOn)
                {
                    _soundValuePointer = VOLUME_POINTER_END_POSITION;
                    SoundManager.Volume = 1.0f;
                    MediaPlayer.Play(_themeSong);
                }
                else
                {
                    _soundValuePointer = VOLUME_POINTER_START_POSITION;
                    SoundManager.Volume = 0.0f;
                    MediaPlayer.Pause();
                }
            }

            #endregion

            #region STEROWANIE

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_blockUpKey)
            {
                _blockUpKey = true;

                // zabezpieczenie przed przekroczeniem zakresu listy
                if (_selectedMenuItemIndex > 0)
                {
                    if (SoundManager.SoundOn) _chooseMenuItemEffectInstance.Play();
                    _selectedMenuItemIndex--;
                    _cursorPosition.Y -= 100;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_blockDownKey)
            {
                _blockDownKey = true;
                // zabezpieczenie przed przekroczeniem zakresu listy
                if (_selectedMenuItemIndex < _menuElementsCount)
                {
                    if (SoundManager.SoundOn) _chooseMenuItemEffectInstance.Play();
                    _selectedMenuItemIndex++;
                    _cursorPosition.Y += 100;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_blockEnterKey)
            {
                _blockEnterKey = true;
                if (SoundManager.SoundOn)
                {
                    _chooseMenuItemEffectInstance.Play();
                    MediaPlayer.Stop();
                }
    
                // sprawdzenie który element został wybrany
                switch (_selectedMenuItemIndex)
                {
                    case 0:
                        IsShowing = false;
                        SelectedOptionMenu = SelectedOptionMenu.NewGame;
                        break;
                    case 1:
                        SelectedOptionMenu = SelectedOptionMenu.Control;
                        break;
                    case 2:
                        SelectedOptionMenu = SelectedOptionMenu.Authors;
                        break;
                    case 3:
                        SelectedOptionMenu = SelectedOptionMenu.Exit;
                        break;
                }

            }

            // odblokowanie klawiszy
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                _blockEnterKey = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                _blockDownKey = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                _blockUpKey = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Q))
                _blockSoundOn = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Add))
                _blockSoundUp = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Subtract))
                _blockSoundDown = false;

            #endregion
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            // narysowanie tła
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);

            #region RYSOWANIE OPCJI DŹWIĘKU

            // narysowanie ikonki dźwięku
            if(SoundManager.SoundOn)
                _spriteBatch.Draw(_soundOnTexture, _soundIconPosition, _soundIconRectangle, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            else
                _spriteBatch.Draw(_soundOffTexture, _soundIconPosition, _soundIconRectangle, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            // naryswoanie progress bara z dźwiękiem
            _spriteBatch.Draw(_soundProgressBarTexture, _soundProgressBarPosition,
                _soundProgressBarRectangle, Color.White, 0.0f,
                Vector2.Zero, 0.3f, SpriteEffects.None, 0);

            // narysowanie wskaźnika dźwięku
            _spriteBatch.Draw(_soundProgressBarPointerTexture, _soundValuePointer,
                _soundProgressBarPointerRectangle, Color.White, 0.0f,
                Vector2.Zero, 0.5f, SpriteEffects.None, 0);


            #endregion


            float xAxis = _graphicsDevice.Viewport.Width/2 - (_newGameOptionTexture.Width/2);
            float yStart = 300;
            _spriteBatch.Draw(_newGameOptionTexture, new Vector2(xAxis, yStart), _newGameOptionRectangle, Color.White);
            _spriteBatch.Draw(_optionOptionTexture, new Vector2(xAxis, yStart+100), _optionOptionRectangle, Color.White);
            _spriteBatch.Draw(_authorsOptionTexture, new Vector2(xAxis, yStart+200), _authorsOptionRectangle, Color.White);
            _spriteBatch.Draw(_exitOptionTexture, new Vector2(xAxis, yStart+300), _exitOptionRectangle, Color.White);

            // wskaźnik 
            _spriteBatch.Draw(_cursorTexture, _cursorPosition.ToVector2(), _cursorRectangle, Color.White);
        }
    }
}