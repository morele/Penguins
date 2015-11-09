using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public interface ICollisionable
    {
        bool IsCollisionDetect(GameObject collisionObject);
        void OnCollisionDetect(GameObject collisionObject);
    }
}
