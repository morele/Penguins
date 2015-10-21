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
            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            skipper = new Penguin(Content.Load<Texture2D>("Postacie/Skipper"), Content.Load<Texture2D>("Slizg/skipper"), new Vector2(-550, 400), penguinSpeed, gravitation, PenguinType.SKIPPER);      
            kowalski = new Penguin(Content.Load<Texture2D>("Postacie/Kowalski"), Content.Load<Texture2D>("Slizg/Kowalski"), new Vector2(-450, 400), penguinSpeed, gravitation, PenguinType.KOWALSKI);
            rico = new Penguin(Content.Load<Texture2D>("Postacie/Rico"), Content.Load<Texture2D>("Slizg/Rico"), new Vector2(-350, 400), penguinSpeed, gravitation, PenguinType.RICO);
            szeregowy = new Penguin(Content.Load<Texture2D>("Postacie/Szeregowy"), Content.Load<Texture2D>("Slizg/Szeregowy"), new Vector2(-250, 400), penguinSpeed, gravitation, PenguinType.SZEREGOWY);

            penguins.Add(skipper);
            penguins.Add(kowalski);
            penguins.Add(rico);
            penguins.Add(szeregowy);

            //Podstawowy gracz - skipper
            player = skipper;

            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy500x48"), new Vector2(-600, 600)));
            
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(50, 600), true, 1, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(300, 600), true, 2, 200));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(500, 600), true, 3, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platformy/Trawa/Platformy100x48"), new Vector2(700, 600), true, 4, 400));          

           playersLabel.Add(new TextLabel(new Vector2(-550, 30), 25, "Skipper - 1", Content.Load<SpriteFont>("JingJing"), Content.Load<Texture2D>("WyborPostaci/Skipper")));         
           playersLabel.Add(new TextLabel(new Vector2(-480, 30), 25, "Kowalski - 2", Content.Load<SpriteFont>("JingJing"), Content.Load<Texture2D>("WyborPostaci/Kowalski")));        
           playersLabel.Add(new TextLabel(new Vector2(-410, 30), 25, "Rico - 3", Content.Load<SpriteFont>("JingJing"), Content.Load<Texture2D>("WyborPostaci/Rico")));
           playersLabel.Add(new TextLabel(new Vector2(-340, 30), 25, "Szeregowy - 4", Content.Load<SpriteFont>("JingJing"), Content.Load<Texture2D>("WyborPostaci/Szeregowy")));
         

        }

        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D1)) player = ActiveAndDeactivationPlayer(true, false, false, false);
            if (Keyboard.GetState().IsKeyDown(Keys.D2)) player = ActiveAndDeactivationPlayer(false, true, false, false);
            if (Keyboard.GetState().IsKeyDown(Keys.D3)) player = ActiveAndDeactivationPlayer(false, false, true, false);
            if (Keyboard.GetState().IsKeyDown(Keys.D4)) player = ActiveAndDeactivationPlayer(false, false, false, true);

           
            if (firstStart) FirstStart(); //metoda ustawia wszystkich graczy na pozycji początkowej

           
                foreach (Platform platform in platforms)
                   // foreach (Penguin penguin in penguins)
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

           /* foreach (Penguin penguin in penguins)
                penguin.UpdatePosition();*/

            player.UpdatePosition();

           for(int i = 0;i < 4; i++)
            {
                labelPosition.X = player.rectangle.X - 550 + (i * 90);
                labelPosition.Y = 30;
                playersLabel[i].Update(labelPosition);
            }
                

           // textLabel.Update(new Vector2(player.rectangle.X - 300, 50));
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


            foreach (TextLabel textLabel in playersLabel)
                textLabel.Draw(spriteBatch,true);
       
          
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

                if (!rico.firstStart && !kowalski.firstStart && !skipper.firstStart && !szeregowy.firstStart) firstStart = false;
            }
        }
    }

}
