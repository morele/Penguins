using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    /// <summary>
    /// Klasa reprezentuje element gry na którym użytkownik będzie mógł wykonać jakąś akcję
    /// </summary>
    public class ActionElement : GameObject, ICollisionable
    {
        public ActionElement(Texture2D texture, Point position, Point size) 
            :base(texture, position, size)
        {
            
        }

        public bool IsCollisionDetect(GameObject collisionObject)
        {
            Rectangle rectangle1 = new Rectangle(Position, Size);

            Rectangle rectangle2 = new Rectangle(collisionObject.Position, collisionObject.Size);

            // sprawdzenie czy nastąpiła kolizja
            if (rectangle1.Intersects(rectangle2))
                return true;

            return false;
        }

        public void OnCollisionDetect(GameObject collisionObject)
        {
            // czy jest to kolizja z pingwinem?
            if (collisionObject is Penguin)
            {
                // ;)
            }

        }
    }
}
