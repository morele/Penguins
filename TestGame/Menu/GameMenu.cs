using System.Collections.Generic;
using System.Media;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace TestGame.Menu
{
    public class GameMenu : Game
    {
        private const float VOLUME_STEP = 19.2f;
        private readonly Vector2 VOLUME_POINTER_START_POSITION = new Vector2(90, 15);
        private readonly Vector2 VOLUME_POINTER_END_POSITION = new Vector2(287, 15);

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<MenuItem> _menuItems;
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

        // dla dźwięku
        private Rectangle _soundIconRectangle;
        private Texture2D _soundOnTexture;
        private Rectangle _soundOffRectangle;
        private Texture2D _soundOffTexture;
        private Rectangle _soundProgressBarRectangle;
        private Texture2D _soundProgressBarTexture;
        private Rectangle _soundProgressBarPointerRectangle;
        private Texture2D _soundProgressBarPointerTexture;
        private Vector2 _soundValuePointer;
        private SoundEffect _chooseMenuItemEffect;
        private SoundEffectInstance _chooseMenuItemEffectInstance;
        private Song _themeSong;
        
        public GameMenu(List<MenuItem> menuItems)
        {
            _menuItems = menuItems;
            _selectedMenuItemIndex = 0;
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1200;

            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            _backgroundRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // grafiki menu
            _backgroundTexture = Content.Load<Texture2D>("menu_background_new");
            _newGameOptionTexture = Content.Load<Texture2D>("NowaGra");
            _optionOptionTexture = Content.Load<Texture2D>("Opcje");
            _authorsOptionTexture = Content.Load<Texture2D>("Autorzy");
            _exitOptionTexture = Content.Load<Texture2D>("Wyjscie");
            _cursorTexture = Content.Load<Texture2D>("Wskaznik");

            // grafiki obsługi dźwięku
            _soundOnTexture = Content.Load<Texture2D>(@"Audio\Graph\soundOn");
            _soundOffTexture = Content.Load<Texture2D>(@"Audio\Graph\soundOff");
            _soundIconRectangle = new Rectangle(0, 0, _soundOnTexture.Width, _soundOnTexture.Height);

            _soundProgressBarPointerTexture = Content.Load<Texture2D>(@"Audio\Graph\barPointer");
            _soundProgressBarPointerRectangle = new Rectangle(0, 0,
                _soundProgressBarPointerTexture.Width,
                _soundProgressBarPointerTexture.Height);

            _soundProgressBarTexture = Content.Load<Texture2D>(@"Audio\Graph\progressBar");
            _soundProgressBarRectangle = new Rectangle(0, 0, 
                _soundProgressBarTexture.Width, 
                _soundProgressBarTexture.Height);

            _soundValuePointer = VOLUME_POINTER_END_POSITION;


            // dźwięki
            _chooseMenuItemEffect = Content.Load<SoundEffect>(@"Audio\Waves\click");
            _chooseMenuItemEffectInstance = _chooseMenuItemEffect.CreateInstance();
            _chooseMenuItemEffectInstance.Volume = SoundManager.Volume;
            _themeSong = Content.Load<Song>("Audio/Waves/menu_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_themeSong);
            SoundManager.SoundOn = true;
            SoundManager.Volume = 1.0f;

            //rectangle
            _newGameOptionRectangle = new Rectangle(0,0,_newGameOptionTexture.Width, _newGameOptionTexture.Height);
            _optionOptionRectangle = new Rectangle(0, 0, _optionOptionTexture.Width, _optionOptionTexture.Height);
            _authorsOptionRectangle = new Rectangle(0, 0, _authorsOptionTexture.Width, _authorsOptionTexture.Height);
            _exitOptionRectangle = new Rectangle(0, 0, _exitOptionTexture.Width, _exitOptionTexture.Height);
            _cursorRectangle = new Rectangle(0, 0, _cursorTexture.Width, _cursorTexture.Height);
            

            float x = graphics.GraphicsDevice.Viewport.Width / 2 - (_newGameOptionTexture.Width / 2) - 10 - _cursorTexture.Width;
            _cursorPosition = new Point((int)x, 300);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        private void ChangeSelectedItemColor(int selectedItem)
        {

        }

        protected override void Update(GameTime gameTime)
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
                if (_selectedMenuItemIndex < _menuItems.Count - 1)
                {
                    if (SoundManager.SoundOn) _chooseMenuItemEffectInstance.Play();
                    _selectedMenuItemIndex++;
                    _cursorPosition.Y += 100;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (SoundManager.SoundOn)
                {
                    _chooseMenuItemEffectInstance.Play();
                    MediaPlayer.Stop();
                }

                _blockUpKey = true;
                // jeśli wybrano wyście
                if (_menuItems[_selectedMenuItemIndex].Link == null)
                {
                    // todo wywala tu exception!!
                    Exit();
                }
                else
                {
                    GameFlow.Run(_menuItems[_selectedMenuItemIndex].Link);
                }
            }

            // odblokowanie klawiszy
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            // narysowanie tła
            spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), _backgroundRectangle, Color.White);

            #region RYSOWANIE OPCJI DŹWIĘKU

            // narysowanie ikonki dźwięku
            if(SoundManager.SoundOn)
                spriteBatch.Draw(_soundOnTexture, Vector2.Zero, _soundIconRectangle, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(_soundOffTexture, Vector2.Zero, _soundIconRectangle, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            // naryswoanie progress bara z dźwiękiem
            spriteBatch.Draw(_soundProgressBarTexture, new Vector2(70, 5),
                _soundProgressBarRectangle, Color.White, 0.0f,
                Vector2.Zero, 0.3f, SpriteEffects.None, 0);

            // narysowanie wskaźnika dźwięku
            spriteBatch.Draw(_soundProgressBarPointerTexture, _soundValuePointer,
                _soundProgressBarPointerRectangle, Color.White, 0.0f,
                Vector2.Zero, 0.5f, SpriteEffects.None, 0);


            #endregion


            float xAxis = graphics.GraphicsDevice.Viewport.Width/2 - (_newGameOptionTexture.Width/2);
            float yStart = 300;
            spriteBatch.Draw(_newGameOptionTexture, new Vector2(xAxis, yStart), _newGameOptionRectangle, Color.White);
            spriteBatch.Draw(_optionOptionTexture, new Vector2(xAxis, yStart+100), _optionOptionRectangle, Color.White);
            spriteBatch.Draw(_authorsOptionTexture, new Vector2(xAxis, yStart+200), _authorsOptionRectangle, Color.White);
            spriteBatch.Draw(_exitOptionTexture, new Vector2(xAxis, yStart+300), _exitOptionRectangle, Color.White);

            // wskaźnik 
            spriteBatch.Draw(_cursorTexture, _cursorPosition.ToVector2(), _cursorRectangle, Color.White);



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}