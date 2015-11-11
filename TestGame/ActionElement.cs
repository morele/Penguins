using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Interfaces;

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
            if(collisionObject.Position.X + collisionObject.Size.X > Position.X &&
               collisionObject.Position.X + collisionObject.Size.X < Position.X + Size.X)
                if(collisionObject.Position.Y > Position.Y)
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
