using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TestGame.Menu;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Penguin rico;
        public Penguin kowalski;
        public Penguin skipper;
        public Penguin szeregowy;
        public Penguin player;
        private List<Platform> platforms = new List<Platform>();
        private List<TextLabel> playersLabel = new List<TextLabel>();
        private TextLabel textLabel;
        private bool firstStart = true;
        private float penguinSpeed;
        private float gravitation;
        private TextLabel _textLabel;

        // panel gracza
        private PlayerPanel _playerPanel;

        Vector2 FontPos;
        Vector2 labelPosition = new Vector2();
        SpriteFont Font;
        Camera camera;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1200;

            graphics.ApplyChanges();
            // todo: poprawić ostatni parametr
            //   _textLabel = new TextLabel(new Vector2(0, 0), "TextLabel", "TextLabelBackground");

        }

        protected override void Initialize()
        {
            penguinSpeed = 5; //szybkość poruszania się pingwinów
            gravitation = 5f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            camera = new Camera();

            // inicjalizacja panelu gracza
            
            _playerPanel = new PlayerPanel(Content.Load<Texture2D>("panel_background"), 
                                           new Vector2(0, 0), 
                                           new Vector2(GraphicsDevice.Viewport.Width, 150),
                                           Content.Load<SpriteFont>("JingJing"));

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            skipper = new Penguin(Content.Load<Texture2D>("Postacie/Skipper"), Content.Load<Texture2D>("Slizg/skipper"), new Vector2(-550, 400), penguinSpeed, gravitation, PenguinType.SKIPPER);
            kowalski = new Penguin(Content.Load<Texture2D>("Postacie/Kowalski"), Content.Load<Texture2D>("Slizg/Kowalski"), new Vector2(-450, 400), penguinSpeed, gravitation, PenguinType.KOWALSKI);
            rico = new Penguin(Content.Load<Texture2D>("Postacie/Rico"), Content.Load<Texture2D>("Slizg/Rico"), new Vector2(-350, 400), penguinSpeed, gravitation, PenguinType.RICO);
            szeregowy = new Penguin(Content.Load<Texture2D>("Postacie/Szeregowy"), Content.Load<Texture2D>("Slizg/Szeregowy"), new Vector2(-250, 400), penguinSpeed, gravitation, PenguinType.SZEREGOWY);


            //Podstawowy gracz - skipper
            player = skipper;
            _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Skipper"), player);

            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy500x48"), new Vector2(-600, 600)));

            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(50, 600), true, 1, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(300, 600), true, 2, 200));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(500, 600), true, 3, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(700, 600), true, 4, 400));


            
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

 

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                player = skipper;
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Skipper"), player);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                player = kowalski;
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Kowalski"), player);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                player = rico;
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Rico"), player);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                player = szeregowy;
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Szeregowy"), player);
            }

            while (firstStart)//pętla ustawia wszystkich graczy na pozycji początkowej
            {
                rico.UpdatePosition();
                kowalski.UpdatePosition();
                skipper.UpdatePosition();
                szeregowy.UpdatePosition();

                foreach (Platform platform in platforms)
                {
                    if (IsOnTopOf(rico, platform)) rico.firstStart = false;
                    if (IsOnTopOf(kowalski, platform)) kowalski.firstStart = false;
                    if (IsOnTopOf(skipper, platform)) skipper.firstStart = false;
                    if (IsOnTopOf(szeregowy, platform)) szeregowy.firstStart = false;
                }
                if (!rico.firstStart && !kowalski.firstStart && !skipper.firstStart && !szeregowy.firstStart) firstStart = false;
            }

            foreach (Platform platform in platforms)
            {
                // sprawdzenie czy na platformie są pingwiny
                if (player.IsOnTopOf(platform))
                {
                    player.speed.Y = 0f;
                    player.jump = false;
                    player.platformSpeed = (int)platform.PlatformSpeed;
                    // jak platforma sie porusza to pingwin razem z nią musi
                    if (platform.IsMotion)
                    {
                        player.PutMeOn(platform);

                        platform.Slowdown();
                        player.platformSpeed = (int)platform.PlatformSpeed;
                    }
                }
                else
                {
                    platform.SpeedUp();
                }

                // aktualizacja pozycji jeśli platforma ma sie poruszać
                platform.UpdatePosition();

            }

            player.UpdatePosition();

            camera.Update(player);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);


            foreach (Platform platform in platforms)
                platform.Draw(spriteBatch);

            rico.Draw(spriteBatch);
            szeregowy.Draw(spriteBatch);
            skipper.Draw(spriteBatch);
            kowalski.Draw(spriteBatch);
            player.Draw(spriteBatch);

            #region PANEL GRACZA

            spriteBatch.End();

            spriteBatch.Begin();

            _playerPanel.Draw(spriteBatch);

            spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
        private bool IsOnTopOf(Penguin player, Platform platform)
        {
            if (player.IsOnTopOf(platform))
            {
                player.speed.Y = 0f;
                player.jump = false;
                return true;
            }
            return false;
        }
    }

}
