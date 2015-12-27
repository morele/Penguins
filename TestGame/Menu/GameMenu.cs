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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<MenuItem> _menuItems;
        private int _selectedMenuItemIndex;

        // flaga blokująca trzymanie klawisza
        private bool _blockUpKey = false;

        private bool _blockDownKey = false;

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

            _backgroundTexture = Content.Load<Texture2D>("menu_background_new");
            _newGameOptionTexture = Content.Load<Texture2D>("NowaGra");
            _optionOptionTexture = Content.Load<Texture2D>("Opcje");
            _authorsOptionTexture = Content.Load<Texture2D>("Autorzy");
            _exitOptionTexture = Content.Load<Texture2D>("Wyjscie");
            _cursorTexture = Content.Load<Texture2D>("Wskaznik");
            
            // zmiana elementu menu
            _chooseMenuItemEffect = Content.Load<SoundEffect>(@"Audio\Waves\click");
            _chooseMenuItemEffectInstance = _chooseMenuItemEffect.CreateInstance();
            _chooseMenuItemEffectInstance.Volume = 0.24f;
            _chooseMenuItemEffectInstance.Pitch = 0.5f;

            try
            {
                _themeSong = Content.Load<Song>("Audio/Waves/menu_theme");
            }catch(Exception ex)
            {
                ex.Message.ToString();
            }
            
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_themeSong);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_blockUpKey)
            {
                _blockUpKey = true;
                // zabezpieczenie przed przekroczeniem zakresu listy
                if (_selectedMenuItemIndex > 0)
                {
                    _chooseMenuItemEffectInstance.Play();
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
                    _chooseMenuItemEffectInstance.Play();
                    _selectedMenuItemIndex++;
                    _cursorPosition.Y += 100;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _chooseMenuItemEffectInstance.Play();
                MediaPlayer.Stop();

                _blockUpKey = true;
                // jeśli wybrano wyście
                if (_menuItems[_selectedMenuItemIndex].Link == null)
                {
                    Exit();
                }
                else
                {
                    GameFlow.Run(_menuItems[_selectedMenuItemIndex].Link);
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                _blockDownKey = false;

            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                _blockUpKey = false;

            // zmiana koloru wybranego elementu
            ChangeSelectedItemColor(_selectedMenuItemIndex);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), _backgroundRectangle, Color.White);

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
