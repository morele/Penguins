using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TestGame.Menu;

namespace TestGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // stworzenie elementów menu
            List<MenuItem> menuItems = new List<MenuItem>()
            {
                new MenuItem("New Game", new Game1()),
                new MenuItem("How to play?", null),
                new MenuItem("Authors", null),
                new MenuItem("Exit", null)
            };

            // stworzenie instancji menu
            GameMenu menu = new GameMenu(menuItems);

            GameFlow.Run(menu);
        }
    }
#endif
}
