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
        public bool CanFallDown { get; set; }

        public Bonus(Texture2D texture, Point position, Point size) 
            : base(texture, position, size)
        {
            CanFallDown = true;
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
            CanFallDown = true;
        }

        public void FallDown()
        {
            if (CanFallDown)
            {
                Position.X += 1;
                Position.Y += 2;
            }
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public bool IsCollisionDetect(GameObject collisionObject)
        {
            Rectangle bonusRectangle = new Rectangle(Position, Size);
            Rectangle collisionObjectRectangle = new Rectangle(collisionObject.Position, collisionObject.Size);

            if (collisionObjectRectangle.Intersects(bonusRectangle))
                return true;
            return false;

        }

        public void OnCollisionDetect(GameObject collisionObject)
        {
            // nic nie rób
        }
    }
}
