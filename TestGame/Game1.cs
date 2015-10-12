using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Penguin rico;
        private Penguin kowalski;
        List<Platform> platforms = new List<Platform>();
        private float penguinSpeed;
        private float gravitation;
        SpriteFont Font1;
        Vector2 FontPos;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
         
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1360;
            graphics.ApplyChanges();

        }

        protected override void Initialize()
        {
            penguinSpeed = 5; //szybkość poruszania się pingwinów
            gravitation = 5f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            rico = new Penguin(Content.Load<Texture2D>("RICO_2"), new Vector2(20, 400), penguinSpeed, gravitation);

            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(30, 670), false));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(400, 600), true));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(800, 550), true));

            platforms[1].Properties(3, 100, 600);
            platforms[2].Properties(7, 300, 560);
            

        }


        protected override void UnloadContent()
        {        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Platform platform in platforms)
            {
                platform.UpdatePosition(); //aktualizacja pozycji jeśli platforma ma sie poruszać

                if (rico.isOnTopOf(platform.rectangle)) //jak pingwin wskoczy na platofrme zatrzymuje sie spadek wysokości
                {
                    rico.speed.Y = 0f;
                    rico.jump = false;

                    if (platform.motion) //jak platforma sie porusza to pingwin razem z nią musi
                    {
                        rico.UpdatePositionRelativePlatform(platform.rectangle.Y);
                    }
                }
            }
               
            rico.UpdatePosition();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            foreach (Platform platform in platforms)
                platform.Draw(spriteBatch);

            rico.Draw(spriteBatch);

           
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
