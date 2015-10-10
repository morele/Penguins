using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private float penguinSpeed;
        private float gravitation;

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
            gravitation = 5f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            rico = new Penguin(Content.Load<Texture2D>("RICO_2"), new Vector2(20, 400), penguinSpeed, gravitation);
            //kowalski = new Penguin(Content.Load<Texture2D>("RICO_2"), new Vector2(100, 400), penguinSpeed, gravitation);
            
        }


        protected override void UnloadContent()
        {        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            rico.UpdatePosition();
            //kowalski.UpdatePosition();
 
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            

            rico.Draw(spriteBatch);
            //kowalski.Draw(spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
