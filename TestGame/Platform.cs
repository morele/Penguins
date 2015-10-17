using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TestGame
{
    class Platform : TextureManager
    {
        public Rectangle rectangle;
        private bool kierunek = true;
        public bool motion;
        private float speed = 1;
        private float maxTop = 100;
        private float maxBottom = 500;
        public Platform(Texture2D Image, Vector2 position, bool motion) : base(Image, position)
        {
            this.motion = motion;
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                this.Image.Width, this.Image.Height);
        }

        override public void UpdatePosition()
        {
            if(motion)
            {
                if (kierunek) // w góre
                {
                    if (position.Y < maxBottom + speed && position.Y > maxTop)
                        position.Y -= speed;
                    else kierunek = false;
                }

                if (!kierunek) // W dół
                {
                    if (position.Y < maxBottom && position.Y > maxTop - speed)
                        position.Y += speed;
                    else kierunek = true;
                }

                rectangle = new Rectangle((int)position.X, (int)position.Y,
                    this.Image.Width, this.Image.Height);
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"> szybkośc poruszania się w góre i dół</param>
        /// <param name="maxTop"> maksymalny półap platformy (zależny od początkowego ustawienia)</param>
        /// <param name="maxBottom"> minimalny półap platformy(zależny od początkowego ustawienia)</param>
        public void Properties(int speed, int maxTop, int maxBottom)
        {
            this.speed = speed;
            this.maxTop = maxTop;
            this.maxBottom = maxBottom;
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, rectangle, Color.White);
            
        }

    }
}
