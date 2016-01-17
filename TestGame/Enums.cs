using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame
{
    public enum CurrentScene
    {
        Scene1 = 1,
        Scene2,
        Scene3
    }

    public enum SelectedOptionMenu
    {
        None,
        NewGame,
        Control,
        Authors,
        Exit
    }

    public enum SelectedOptionPauseMenu
    {
        None,
        Resume,
        TryAgain,
        BackToMenu
    }


    // kierunek poruszania się obiektów
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    public enum PenguinType
    {
        RICO = 1,
        SKIPPER = 2,
        KOWALSKI = 3,
        SZEREGOWY = 4,
        NN = 5
    }
    public enum PlatformType
    {
        FLOOR = 1,
        MONEY = 2,
        NN = 3,
        MAGICPIPE = 4,
        SPRING = 5,
        PAWL = 6,
        CAR = 7,
        SPIKE=8,
        JULIAN=9

    }
}
