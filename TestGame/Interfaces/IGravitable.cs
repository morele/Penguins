namespace TestGame.Interfaces
{
    /// <summary>
    /// Interfejs definiuje zachowanie obiektów w polu grawitacyjnym
    /// </summary>
    public interface IGravitable
    {
        void FallDown();
        void Jump();
    }
}
