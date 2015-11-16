﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public static class Const
    {
        // wysokość pingwina
        public static int PENGUIN_HEIGHT = 197;

        // grawitacja - czyli prędkość spadania pingwina
        public static int GRAVITY = 10;
        public static Color SELECTED_MENU_ITEM_COLOR = Color.Orange;

        public static Color DEFAULT_MENU_ITEM_COLOR = Color.Black;
        public static int PINGUIN_RICO_VERTICAL = -41;
        public static int PINGUIN_RICO_HORIZONTAL = 82;

        public static int PINGUIN_SZEREGOWY_VERTICAL = -34;
        public static int PINGUIN_SZEREGOWY_HORIZONTAL = 61;

        public static int PINGUIN_KOWALSKI_VERTICAL = -58;
        public static int PINGUIN_KOWALSKI_HORIZONTAL = 81;

        public static int PINGUIN_SKIPPER_VERTICAL = -29;
        public static int PINGUIN_SKIPPER_HORIZONTAL = 62;

        public static int PENGUIN_AVATAR_SIZE = 224;

        public static int PLAYER_PANEL_HEIGHT = 150;

        public static int RICO_MASS = 16;
        public static int KOWALSKI_MASS = 13;
        public static int SKIPPER_MASS = 12;
        public static int SZEREGOWY_MASS = 10;

        public static List<Rectangle> DimensionsPenguin(PenguinType type)
        {
            List<Rectangle> dimensions = new List<Rectangle>();

            switch (type)
            {
                case PenguinType.KOWALSKI:
                    dimensions.Add(new Rectangle(150, 17, 129, 45));    // czubek głowy
                    dimensions.Add(new Rectangle(104, 62, 199, 735));   // głowa i tułów
                    dimensions.Add(new Rectangle(303, 144, 97, 36));    // dziób
                    dimensions.Add(new Rectangle(65, 275, 39, 329));    // plecy
                    dimensions.Add(new Rectangle(17, 604, 89, 216));    // dupa
                    dimensions.Add(new Rectangle(121,797, 240, 83));    // nogi
                    dimensions.Add(new Rectangle(303, 317, 37, 424));   // brzuch
                    break;
                case PenguinType.RICO:
                    dimensions.Add(new Rectangle(135,45,185,595));//głowa
                    dimensions.Add(new Rectangle(320, 140, 160, 60));//dziob
                    dimensions.Add(new Rectangle(115, 760, 230, 56));//nogi
                    dimensions.Add(new Rectangle(10, 200, 70, 540));//dupa
                    dimensions.Add(new Rectangle(300, 380, 35, 290));//brzuch
                    break;
                case PenguinType.SZEREGOWY:
                    dimensions.Add(new Rectangle(135, 45, 185, 595));//głowa
                    dimensions.Add(new Rectangle(320, 140, 160, 60));//dziob
                    dimensions.Add(new Rectangle(115, 760, 230, 56));//nogi
                    dimensions.Add(new Rectangle(10, 200, 70, 540));//dupa
                    break;
                case PenguinType.SKIPPER:

                    dimensions.Add(new Rectangle(89, 10, 239, 586));   // głowa i tułów
                    dimensions.Add(new Rectangle(39, 281, 54,244));    // plecy
                    dimensions.Add(new Rectangle(15, 523, 78, 103));   // dupa
                    dimensions.Add(new Rectangle(129, 600, 215, 61));  // nogi
                    dimensions.Add(new Rectangle(331, 212, 42, 283));  // brzuch
                    dimensions.Add(new Rectangle(328, 97, 90, 42));    // dziób

                    break;

            }
            return dimensions;
        }

    }
}
