using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Plansza plansza;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1300;
            graphics.PreferredBackBufferHeight = 750;

            IsMouseVisible = true;

        }

      
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            plansza=new Plansza(graphics, spriteBatch, this.Content);
        }

     
        protected override void UnloadContent()
        {
            
        }

     
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Mouse.GetState().LeftButton==ButtonState.Pressed)
                plansza.OkryjKarte(Mouse.GetState().X, Mouse.GetState().Y);

            plansza.Zakryj(gameTime);

            this.Window.Title = "Krokow: " + plansza.LiczbaKrokow + " Kart: " + plansza.liczbakart;

            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            plansza.Rysuj();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
