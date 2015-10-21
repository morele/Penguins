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
        private List<Penguin> penguins = new List<Penguin>();
        private TextLabel textLabel;
        private float penguinSpeed;
        private float gravitation;
        private TextLabel _textLabel;

        // panel gracza
        private PlayerPanel _playerPanel;

        Vector2 FontPos;
        Vector2 labelPosition = new Vector2();
        SpriteFont Font;
        Camera camera;
        private bool firstStart = true;
        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1200;

            graphics.ApplyChanges();

        }

        protected override void Initialize()
        {
            penguinSpeed = 5; //szybkość poruszania się pingwinów
            gravitation = 5f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            camera = new Camera();

            // inicjalizacja panelu gracza - podstawowy gracz - skipper
            
            _playerPanel = new PlayerPanel(Content.Load<Texture2D>("panel_background"), 
                                           new Vector2(0, 0), 
                                           new Vector2(GraphicsDevice.Viewport.Width, 150),
                                           Content.Load<SpriteFont>("JingJing"),
                                           Content.Load<Texture2D>("WyborPostaci/Skipper"));


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

            penguins.Add(skipper);
            penguins.Add(kowalski);
            penguins.Add(rico);
            penguins.Add(szeregowy);
            //penguins.Add(player);

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
                GameFlow.CurrentInstance.Exit();

 

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                player = ActiveAndDeactivationPlayer(true, false, false, false);
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Skipper"), player);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                player = ActiveAndDeactivationPlayer(false, true, false, false);
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Kowalski"), player);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                player = ActiveAndDeactivationPlayer(false, false, true, false);
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Rico"), player);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                player = ActiveAndDeactivationPlayer(false, false, false, true);
                _playerPanel.Update(Content.Load<Texture2D>("WyborPostaci/Szeregowy"), player);
            }

            if (firstStart) FirstStart(); //metoda ustawia wszystkich graczy na pozycji początkowej


                

             foreach (Platform platform in platforms)
                /// foreach (Penguin penguin in penguins)
            {
                if (player.IsOnTopOf(platform))// sprawdzenie czy na platformie są pingwiny
                {
                     player.JumpStop((int)platform.PlatformSpeed); //zatrzymuje spadek pingwina jak wykryje kolizje z platforma 

                    
                    if (platform.IsMotion) // jak platforma sie porusza to pingwin razem z nią musi
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

            
             foreach (Platform platform in platforms)
             {
                foreach (Penguin penguin in penguins)
                {
                    if (penguin.active == false)
                    {
                        penguin.UpdatePosition();
                        if (penguin.IsOnTopOf(platform))
                        {
                            penguin.JumpStop((int)platform.PlatformSpeed);

                            if (platform.IsMotion) // jak platforma sie porusza to pingwin razem z nią musi
                            {
                                penguin.PutMeOn(platform);

                                platform.Slowdown();
                                penguin.platformSpeed = (int)platform.PlatformSpeed;
                            }
                        }
                    }
                    
                }
               
            }


            foreach (Penguin penguin in penguins)
                if (penguin.active == false)

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

            spriteBatch.End();

            #region PANEL GRACZA

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
        /// <summary>
        /// Metoda aktywuje i dezaktuwuje graczy
        /// </summary>
        /// <returns>Zwracam referencje do aktywnego gracza</returns>
        private Penguin ActiveAndDeactivationPlayer(bool ConSkipper, bool ConKowalski, bool ConRico, bool ConSzeregowy)
        {
            skipper.active = ConSkipper;
            kowalski.active = ConKowalski;
            rico.active = ConRico;
            szeregowy.active = ConSzeregowy;

            if (ConSkipper) return skipper;
            if (ConKowalski) return kowalski;
            if (ConRico) return rico;
            if (ConSzeregowy) return szeregowy;

            return skipper;
        }

        private void FirstStart()
        {
            while (firstStart)
            {
                foreach (Penguin penguin in penguins)
                    penguin.UpdatePosition();


                foreach (Platform platform in platforms)
                    foreach (Penguin penguin in penguins)
                        if (IsOnTopOf(penguin, platform)) penguin.firstStart = false;

                if (!rico.firstStart && !kowalski.firstStart && !skipper.firstStart && !szeregowy.firstStart)
                {
                    ActiveAndDeactivationPlayer(true, false, false, false);
                    firstStart = false;
                }
            }
        }
    }

}
