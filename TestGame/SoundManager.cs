namespace TestGame
{
    public static class SoundManager
    {
        public static bool SoundOn { get; set; }
        public static float Volume { get; set; }

        static SoundManager()
        {
            SoundOn = false;
            Volume = 0.2f;
        }
    }
}
