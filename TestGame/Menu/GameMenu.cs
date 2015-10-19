using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public GameMenu(List<MenuItem> menuItems)
        {
            _menuItems = menuItems;
            _selectedMenuItemIndex = 0;

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1200;

            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            _backgroundRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // inicjalizacja czcionek 
            int rightBorder = GraphicsDevice.Viewport.Width-200 ;
            int verticalCenter = GraphicsDevice.Viewport.Height/2;

            for (int i = 0; i < _menuItems.Count; i++)
            {
                _menuItems[i].SetLabel(new TextLabel(new Rectangle(rightBorder, verticalCenter + ((i + 1) * 50), 100, 50), _menuItems[i].Title, "TextLabelBackgroundTransparent"));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTexture = Content.Load<Texture2D>("menu_background");

            foreach (var menuItem in _menuItems)
            {
                menuItem.Label.LoadContent(Content);
            }

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Window.Title = string.Format("Selected: {0}", _menuItems[_selectedMenuItemIndex].Title);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_blockUpKey)
            {
                _blockUpKey = true;
                // zabezpieczenie przed przekroczeniem zakresu listy
                if (_selectedMenuItemIndex > 0)
                {
                    _selectedMenuItemIndex--;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_blockDownKey)
            {
                _blockDownKey = true;
                // zabezpieczenie przed przekroczeniem zakresu listy
                if (_selectedMenuItemIndex < _menuItems.Count-1)
                    _selectedMenuItemIndex++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _blockUpKey = true;
                // jeśli wybrano wyście
                if (_menuItems[_selectedMenuItemIndex].Link == null)
                {
                    Exit();
                }
                else
                {
                    _menuItems[_selectedMenuItemIndex].Link.Run();
                    Exit();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                _blockDownKey = false;

            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                _blockUpKey = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, new Vector2(0,0), _backgroundRectangle, Color.White);

            foreach (var menuItem in _menuItems)
            {
                menuItem.Label.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
