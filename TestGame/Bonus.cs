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
            Penguin temp = checkedObject as Penguin;

            Point newSize = new Point(Size.X, Size.Y + 25);
            Rectangle rectangle1 = new Rectangle(Position, newSize);
            Rectangle rectangle2 = new Rectangle(checkedObject.Position, checkedObject.Size);

            foreach (var rect in temp.currentdimensionsPenguin)
            {
                if (rectangle1.Intersects(rect))
                    return true;
            }

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
            // bonusy nie skaczą :P
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
