using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Interfaces;

namespace TestGame
{
    public class Bonus : GameObject, ICheckable, IGravitable, ICollisionable
    {
        public Bonus(Texture2D texture, Point position, Point size) 
            : base(texture, position, size)
        {
        }


        public bool IsChecked(GameObject checkedObject)
        {
            Rectangle rectangle1 = new Rectangle(Position, Size);
            Rectangle rectangle2 = new Rectangle(checkedObject.Position, checkedObject.Size);

            if (rectangle2.Intersects(rectangle1))
                return true;
            return false;
        }

        public void OnChecked()
        {
            IsActive = false;
        }

        public void FallDown()
        {
            Position.Y += 1;
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public bool IsCollisionDetect(GameObject collisionObject)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionDetect(GameObject collisionObject)
        {
            throw new NotImplementedException();
        }
    }
}
