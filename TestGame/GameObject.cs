using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    /// <summary>
    /// Główna klasa gry
    /// </summary>
    public class GameObject
    {
        public Texture2D Texture;
        public Point Position;
        public Point Size;

        public GameObject(Texture2D texture, Point position, Point size)
        {
            Texture = texture;
            Position = position;
            Size = size;
        }
    }
}
