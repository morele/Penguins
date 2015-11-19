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
        public Rectangle ActionSector { get; set; }

        public ActionElement(Texture2D texture, Point position, Point size, int actionSector) 
            :base(texture, position, size)
        {
            Point newPosition = new Point(position.X - actionSector, position.Y - actionSector);
            Point newSize = new Point(size.X + actionSector*2, size.Y + actionSector*2);

            ActionSector = new Rectangle(newPosition, newSize);
        }

        public bool IsInActionSector(GameObject obj)
        {
            Rectangle objRectangle = new Rectangle(obj.Position, obj.Size);

            if(obj.Position.X + obj.Size.X > ActionSector.X)
                return true;
            return false;
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
        }
    }
}
