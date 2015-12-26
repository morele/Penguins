using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
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
        private GameTime gametime;

        Scene1 scene1;
        Scene2 scene2;

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
            penguinSpeed = 4; //szybkość poruszania się pingwinów
            gravitation = 7f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            camera = new Camera();

            //scene1 = new Scene1(Content, camera, gametime);
            scene2 = new Scene2(Content, camera, gametime);

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

            skipper = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/SkipperAnimacja"),
                                  Content.Load<Texture2D>("Postacie/Animacje/SkipperAnimacja"),
                                  Content.Load<Texture2D>("WyborPostaci/Skipper"),
                                  new Vector2(-1080, 400), penguinSpeed,
                                  gravitation, PenguinType.SKIPPER, Const.SKIPPER_MASS, new Point(422, 663));

            // dźwięki wydawane przez skippera
            skipper.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\skipper_start"));

            kowalski = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/KowalskiAnimacja"),
                                   Content.Load<Texture2D>("Postacie/Animacje/KowalskiPlywanie"),
                                   Content.Load<Texture2D>("WyborPostaci/Kowalski"),
                                   new Vector2(-1030, 400), penguinSpeed,
                                   gravitation, PenguinType.KOWALSKI, Const.KOWALSKI_MASS, new Point(412, 882));

            rico = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/RicoAnimacja_poprawiony"),
                               Content.Load<Texture2D>("Postacie/Animacje/RicoPlywa"),//Ł.G: tymczasowo zmienione 
                               Content.Load<Texture2D>("WyborPostaci/Rico"),
                               new Vector2(-980, 400), penguinSpeed,
                               gravitation, PenguinType.RICO, Const.RICO_MASS, new Point(480, 815));//Ł.G : dodanie rozmiaru frame do Animacji

            // dźwięki wydawane przez skippera
            rico.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\rico_start"));



            szeregowy = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/SzeregowySheet"),
                                    Content.Load<Texture2D>("Slizg/Szeregowy"),
                                    Content.Load<Texture2D>("WyborPostaci/Szeregowy"),
                                    new Vector2(-930, 400), penguinSpeed,
                                    gravitation, PenguinType.SZEREGOWY, Const.SZEREGOWY_MASS, new Point(352, 635));

            penguins.Add(skipper);
            penguins.Add(kowalski);
            penguins.Add(rico);
            penguins.Add(szeregowy);

            //Podstawowy gracz - skipper
            player = ActiveAndDeactivationPlayer(true, false, false, false);

            //scene1.LoadContent(penguins, _playerPanel, player);
            scene2.LoadContent(penguins, _playerPanel, player);

            // załadowanie i ustawienie platform
            /*   platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy500x48"), new Vector2(-600, 600)));
               platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(50, 600), true, 1, 100));
               platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(300, 600), true, 2, 200));
               platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(500, 600), true, 3, 100));
               platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(700, 600), true, 4, 400));*/
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameFlow.CurrentInstance.Exit();
            var deltaTime = 1 / gameTime.ElapsedGameTime.TotalSeconds;



            //scene1.UpdatePosition(gameTime);

            scene2.UpdatePosition(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.gametime = gameTime;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            //scene1.Draw(spriteBatch);
            scene2.Draw(spriteBatch);

            spriteBatch.End();

            #region PANEL GRACZA

            spriteBatch.Begin();

            _playerPanel.Draw(spriteBatch);


            spriteBatch.End();

            #endregion

            base.Draw(gameTime);
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

    }

}