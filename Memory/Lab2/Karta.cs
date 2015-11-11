using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Lab2
{
    class Karta
    {
        public Rectangle Wycinek;
        public Rectangle Pozycja;
        public bool zakryte;
        public bool wyswietl;
        


        public Karta(Rectangle wycinek, Vector2 pozycja)
        {
            Wycinek = wycinek;
            Pozycja = new Rectangle((int)pozycja.X, (int)pozycja.Y, 150, 150);
            zakryte = true;
            wyswietl = true;
           
        }


    }
}
