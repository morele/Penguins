using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TestGame
{
    class Platform : TextureManager
    {
        public Rectangle rectangle;
        public Platform(Texture2D Image, Vector2 position) : base(Image, position)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                this.Image.Width, this.Image.Height);
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
        }

    }
}
