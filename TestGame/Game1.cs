using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        private Penguin kowalski;
        List<Platform> platforms = new List<Platform>();
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
            rico = new Penguin(Content.Load<Texture2D>("RICO_2"), Content.Load<Texture2D>("RICO_2_poziomo"), new Vector2(20, 400), penguinSpeed, gravitation);


            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(30, 600)));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(250, 600), true, 2, 100));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(500, 600), true, 5, 200));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(750, 600)));

            Font = Content.Load<SpriteFont>("JingJing");
            _textLabel.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            // todo: zwolnienie zasobów gry
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Platform platform in platforms)
            {
                // sprawdzenie czy na platformie są pingwiny
                if (rico.IsOnTopOf(platform))
                {
                    rico.speed.Y = 0f;
                    rico.jump = false;

                    // jak platforma sie porusza to pingwin razem z nią musi
                    if (platform.IsMotion) 
                    {
                        rico.PutMeOn(platform);
                        
                        platform.Slowdown();
                    }
                     
                }
                else
                {
                    platform.SpeedUp();
                }
                
                // aktualizacja pozycji jeśli platforma ma sie poruszać
                platform.UpdatePosition(); 
            }

            rico.UpdatePosition();

            camera.Update(rico);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);


            foreach (Platform platform in platforms)
                platform.Draw(spriteBatch);

            rico.Draw(spriteBatch);

            /* var origin = new Vector2()
             {
                 X = image.Width / 2,
                 Y = image.Height / 2
             };
             Vector2 vec = new Vector2(100, 100);
            // spriteBatch.Draw(image, vec, null, Color.White, 90, origin, 1f, SpriteEffects.None, 0f);*/
            _textLabel.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
