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
            penguinSpeed = 3; //szybkość poruszania się pingwinów
            gravitation = 10f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            rico = new Penguin(Content.Load<Texture2D>("RICO_2"), new Vector2(20, 400), penguinSpeed, gravitation);

            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(30, 670)));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(400, 600)));
            platforms.Add(new Platform(Content.Load<Texture2D>("trawa"), new Vector2(800, 550)));

            //Font1 = Content.Load<SpriteFont>("Courier New");
            

        }


        protected override void UnloadContent()
        {        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Platform platform in platforms)
                if (rico.isOnTopOf(platform.rectangle))
                {
                    rico.speed.Y = 0f;
                    rico.jump = false;
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
