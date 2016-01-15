using System;
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

        #region region dla stalych poprawki pozycji animacji

        public static int PINGUIN_RICO_CORRECTION_VERTICAL = 100;
        public static int PINGUIN_RICO_CORRECTION_HORIZONTAL = 100;

        public static int PINGUIN_KOWALSKI_CORRECTION_VERTICAL = 100;
        public static int PINGUIN_KOWALSKI_CORRECTION_HORIZONTAL = 100;

        public static int PINGUIN_SKIPPER_CORRECTION_VERTICAL = 100;
        public static int PINGUIN_SKIPPER_CORRECTION_HORIZONTAL = 100;

        public static int PINGUIN_PRIVATE_CORRECTION_VERTICAL = 100;
        public static int PINGUIN_PRIVATE_CORRECTION_HORIZONTAL = 100;

        #endregion

        public static int PINGUIN_RICO_VERTICAL = -41;
        public static int PINGUIN_RICO_HORIZONTAL = 57;

        public static int PINGUIN_SZEREGOWY_VERTICAL = -34;
        public static int PINGUIN_SZEREGOWY_HORIZONTAL = 45;

        public static int PINGUIN_KOWALSKI_VERTICAL = -58;
        public static int PINGUIN_KOWALSKI_HORIZONTAL = 68;

        public static int PINGUIN_SKIPPER_VERTICAL = -29;
        public static int PINGUIN_SKIPPER_HORIZONTAL = 40;

        public static int PENGUIN_AVATAR_SIZE = 224;

        public static int PLAYER_PANEL_HEIGHT = 150;

        public static int SPRING_JUMP = 300;

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
                    dimensions.Add(new Rectangle(50,0,125,50));   // głowa
                    dimensions.Add(new Rectangle(20,390,125,52));    // nogi
                    dimensions.Add(new Rectangle(20,120,40,230));   // dupa
                    dimensions.Add(new Rectangle(130,120,45,230));    // brzuch

                    break;
                case PenguinType.RICO:
                    dimensions.Add(new Rectangle(60,22,110,40));//głowa
                    dimensions.Add(new Rectangle(0,350,170,58));//nogi
                    dimensions.Add(new Rectangle(0,110,50,180));// dupa
                    dimensions.Add(new Rectangle(120,110,50,180));// brzuch

                    break;
                case PenguinType.SZEREGOWY:
                    dimensions.Add(new Rectangle(15,0,145,50));//głowa
                    dimensions.Add(new Rectangle(31,268,129,40));//nogi --16 - 16 px
                    dimensions.Add(new Rectangle(15,100,50,140));// dupa
                    dimensions.Add(new Rectangle(110,100,50,140));//brzuch

                    break;
                case PenguinType.SKIPPER:

                    dimensions.Add(new Rectangle(30,0,150,50));   // głowa
                    dimensions.Add(new Rectangle(30,280,150,52));    // nogi
                    dimensions.Add(new Rectangle(30,100,50,140));  // dupa
                    dimensions.Add(new Rectangle(130,100,50,140));   //   brzuch                  

                    break;

            }
            return dimensions;
        }
        public static List<Rectangle> DimensionsPenguinSlide(PenguinType type)
        {
            List<Rectangle> dimensions = new List<Rectangle>();

            switch (type)
            {
                case PenguinType.KOWALSKI:
                    dimensions.Add(new Rectangle(50,0,390,50));   // głowa
                    dimensions.Add(new Rectangle(50,134,850,50)); // nogi
                    dimensions.Add(new Rectangle(50,20,50,90));   // dupa 
                    dimensions.Add(new Rectangle(390,0,50,110));    // pyszczek
                    break;
                case PenguinType.RICO:
                    dimensions.Add(new Rectangle(30,12,395,50));//głowa
                    dimensions.Add(new Rectangle(30,132,395,50));//nogi
                    dimensions.Add(new Rectangle(30,40,50,70));//dupa
                    dimensions.Add(new Rectangle(375,20,50,90));//pyszczek
                    break;
                case PenguinType.SZEREGOWY: 
                    dimensions.Add(new Rectangle(50,0,280,50));//głowa
                    dimensions.Add(new Rectangle(50,89,280,50));//nogi
                    dimensions.Add(new Rectangle(50,0,50,60));//dupa
                    dimensions.Add(new Rectangle(280,0,50,60));//pyszczek
                    break;
                case PenguinType.SKIPPER:
                    dimensions.Add(new Rectangle(50,0,310,50));   // głowa 
                    dimensions.Add(new Rectangle(50,123,310,50));    // nogi
                    dimensions.Add(new Rectangle(50,8,50,92));  // dupa
                    dimensions.Add(new Rectangle(310,0,50,100));   // pyszczek                    
                    break;
            }
            return dimensions;
        }
    }
}