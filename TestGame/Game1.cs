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
        private bool firstStart = true;
        private float penguinSpeed;
        private float gravitation;
        private TextLabel _textLabel;
        Vector2 FontPos;
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
            _textLabel = new TextLabel(new Rectangle(0, 0, 100, 50), "TextLabel", "TextLabelBackground");
            
        }

        protected override void Initialize()
        {
            penguinSpeed = 5; //szybkość poruszania się pingwinów
            gravitation = 5f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            camera = new Camera();
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

            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy500x48"), new Vector2(-600, 600)));
            
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(50, 600), true, 1, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(300, 600), true, 2, 200));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(500, 600), true, 3, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(700, 600), true, 4, 400));

          /* playersLabel.Add(new TextLabel(new Rectangle(-500, 20, 60, 60), "Rico - 1", "WyborPostaci/Rico"));
           playersLabel[0].LoadContent(Content);
           playersLabel.Add(new TextLabel(new Rectangle(-430, 20, 60, 60), "Kowalski - 2", "WyborPostaci/Kowalski"));
           playersLabel[1].LoadContent(Content);
           playersLabel.Add(new TextLabel(new Rectangle(-360, 20, 60, 60), "Skipper - 3", "WyborPostaci/Skipper"));
           playersLabel[2].LoadContent(Content);
           playersLabel.Add(new TextLabel(new Rectangle(-290, 20, 60, 60), "Szeregowy - 4", "WyborPostaci/Szeregowy"));
           playersLabel[3].LoadContent(Content);
           */

            Font = Content.Load<SpriteFont>("JingJing");
            
              _textLabel.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D1)) player = skipper;
            if (Keyboard.GetState().IsKeyDown(Keys.D2)) player = kowalski;
            if (Keyboard.GetState().IsKeyDown(Keys.D3)) player = rico;
            if (Keyboard.GetState().IsKeyDown(Keys.D4)) player = szeregowy;

            while(firstStart)//pętla ustawia wszystkich graczy na pozycji początkowej
            {
                rico.UpdatePosition();
                kowalski.UpdatePosition();
                skipper.UpdatePosition();
                szeregowy.UpdatePosition();

                foreach (Platform platform in platforms)
                {
                    if (IsOnTopOf(rico, platform)) rico.firstStart = false;
                    if(IsOnTopOf(kowalski, platform)) kowalski.firstStart = false;
                    if(IsOnTopOf(skipper, platform)) skipper.firstStart = false;
                    if( IsOnTopOf(szeregowy, platform)) szeregowy.firstStart = false;
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
            
            /*foreach (TextLabel playerLabel in playersLabel)
                playerLabel.Update(gameTime,"ja Jebie", new Rectangle(rico.rectangle.X + (int)playerLabel._positionOfText.X, 20, 60, 60));*/


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

            /* foreach (TextLabel playerLabel in playersLabel)
                 playerLabel.Draw(spriteBatch);*/

            _textLabel.Draw(spriteBatch);
            spriteBatch.End();

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
