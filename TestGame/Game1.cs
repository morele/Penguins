using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TestGame.Menu;
using TestGame.Scene;

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
        private float penguinSpeed;
        private float gravitation;
        private TextLabel _textLabel;
        // panel gracza
        private PlayerPanel _playerPanel;
        Camera camera;
        private bool firstStart = true;

        Scene1 scene1;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1200;

            graphics.ApplyChanges();
           // TargetElapsedTime  = new TimeSpan(0, 0, 0, 0, 1);

        }

        protected override void Initialize()
        {
            penguinSpeed = 5; //szybkość poruszania się pingwinów
            gravitation = 7f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            camera = new Camera();

            scene1 = new Scene1(Content, camera);


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

            skipper = new Penguin(Content.Load<Texture2D>("Postacie/Skipper"), 
                                  Content.Load<Texture2D>("Slizg/skipper"),
                                  Content.Load<Texture2D>("WyborPostaci/Skipper"),
                                  new Vector2(-550, 400), penguinSpeed, 
                                  gravitation, PenguinType.SKIPPER, Const.SKIPPER_MASS);

            kowalski = new Penguin(Content.Load<Texture2D>("Postacie/Kowalski"), 
                                   Content.Load<Texture2D>("Slizg/Kowalski"),
                                   Content.Load<Texture2D>("WyborPostaci/Kowalski"),
                                   new Vector2(-450, 400), penguinSpeed, 
                                   gravitation, PenguinType.KOWALSKI, Const.KOWALSKI_MASS);

            rico = new Penguin(Content.Load<Texture2D>("Postacie/Rico"), 
                               Content.Load<Texture2D>("Slizg/Rico"),
                               Content.Load<Texture2D>("WyborPostaci/Rico"),
                               new Vector2(-350, 400), penguinSpeed, 
                               gravitation, PenguinType.RICO, Const.RICO_MASS);

            szeregowy = new Penguin(Content.Load<Texture2D>("Postacie/Szeregowy"), 
                                    Content.Load<Texture2D>("Slizg/Szeregowy"),
                                    Content.Load<Texture2D>("WyborPostaci/Szeregowy"),
                                    new Vector2(-250, 400), penguinSpeed,
                                    gravitation, PenguinType.SZEREGOWY, Const.SZEREGOWY_MASS);

            penguins.Add(skipper);
            penguins.Add(kowalski);
            penguins.Add(rico);
            penguins.Add(szeregowy);

            //Podstawowy gracz - skipper
            player = ActiveAndDeactivationPlayer(true, false, false, false);

            scene1.LoadContent(penguins, _playerPanel, player);


            // załadowanie i ustawienie platform
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
            var deltaTime = 1/gameTime.ElapsedGameTime.TotalSeconds;

            // metoda ustawia wszystkich graczy na pozycji początkowej
            /*  if (firstStart) FirstStart();

              if (Keyboard.GetState().IsKeyDown(Keys.D1)) player = ActiveAndDeactivationPlayer(true, false, false, false);
              if (Keyboard.GetState().IsKeyDown(Keys.D2)) player = ActiveAndDeactivationPlayer(false, true, false, false);
              if (Keyboard.GetState().IsKeyDown(Keys.D3)) player = ActiveAndDeactivationPlayer(false, false, true, false);
              if (Keyboard.GetState().IsKeyDown(Keys.D4)) player = ActiveAndDeactivationPlayer(false, false, false, true);


              // odświeżenie paska gracza
              _playerPanel.Update(player);



              int i;
              foreach (Platform platform in platforms)
              {
                  foreach (Penguin penguin in penguins)
                  {

                      for(i = 0; i < penguins.Count; i++)//sprawdza kolizje z innymi pingwinami i blokuje w przypadku wykrycia
                          if (penguins[i].penguinType != penguin.penguinType) penguins[i].CollisionPenguin(penguin.rectangle);


                      if (penguin.IsOnTopOf(platform))// sprawdzenie czy na platformie są pingwiny
                      {
                          penguin.JumpStop((int)platform.PlatformSpeed); //zatrzymuje spadek pingwina jak wykryje kolizje z platforma 

                          if (platform.IsMotion) // jak platforma sie porusza to pingwin razem z nią musi
                          {
                              penguin.PutMeOn(platform.PlatformRectangle);

                              if (penguin.active) platform.Slowdown();
                              penguin.platformSpeed = (int)platform.PlatformSpeed;
                          }
                      }
                      else
                      {
                          if (penguin.active) platform.SpeedUp();
                      }

                  }
                  // aktualizacja pozycji jeśli platforma ma sie poruszać
                  platform.UpdatePosition();
              }



                foreach (Penguin penguin in penguins)
                    penguin.UpdatePosition();


              camera.Update(player);*/

            scene1.UpdatePosition(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);


            /* foreach (Platform platform in platforms)
                 platform.Draw(spriteBatch);

             rico.Draw(spriteBatch);
             szeregowy.Draw(spriteBatch);
             skipper.Draw(spriteBatch);
             kowalski.Draw(spriteBatch);*/

            scene1.Draw(spriteBatch);

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
                    firstStart = false;
                }
            }
        }
    }

}
