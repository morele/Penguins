using Microsoft.Xna.Framework;

namespace TestGame
{
    /// <summary>
    /// Statyczna klasa, która służy do zarządznia przepływem gry, 
    /// tworzy nowe instancje klasy Game oraz usuwa poprzednie.
    /// Zapobiega tworzeniu się wielu okien z grą.
    /// </summary>
    public static class GameFlow
    {
        // poprzednia instancja
        private static Game _previuoslyInstance;

        // aktualna instancja 
        public static Game CurrentInstance;

        /// <summary>
        /// Metoda uruchamia ustawiony obiekt klasy Game
        /// </summary>
        /// <param name="game">Obiekt klasy Game</param>
        public static void Run(Game game)
        {
            // zapisanie referencji do poprzedniego obiektu
            _previuoslyInstance = CurrentInstance;

            // usunięcie referencji do aktualnego obiektu
            CurrentInstance = new Game();

            // ustawienie referencji na nowy obiekt
            CurrentInstance = game;

            if (_previuoslyInstance != null)
            {
                _previuoslyInstance.Exit();
            }

            CurrentInstance.Run();
        }
    }
}
