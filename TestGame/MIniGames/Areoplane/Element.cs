using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.MIniGames.Areoplane
{
    public class Element
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public int szerokosc;
        public int wysokosc;
        private int punkty;
        private Texture2D tekstura;
        private Texture2D _background;
        private Rectangle SheetPart;
        private float Odswiezania;
        private float Wyswietlono;
        private float rozmiar;
        private int krok;
        private int liczba_klatek;
        float poczatek;
        int pocz_x = 1200;
        int pocz_y = 0;
        public Element(string plik, int lk, float odswiez, float skala, Microsoft.Xna.Framework.Content.ContentManager Content, SpriteBatch sb)
        {
            spriteBatch = sb;
            tekstura = Content.Load<Texture2D>(@plik);
            _background = Content.Load<Texture2D>("Minigry/areoPlane/TloSamolot");
            liczba_klatek = lk;
            szerokosc = tekstura.Width / liczba_klatek;
            wysokosc = tekstura.Height;
            krok = 0;
            Wyswietlono = 0;
            Odswiezania = odswiez;
            rozmiar = skala;
        }

        public int Update(GameTime gameTime)
        {
            Wyswietlono += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Wyswietlono >= Odswiezania)
            {
                krok++;
                Wyswietlono = 0;

                if (krok >= liczba_klatek)
                {
                    krok = 0;
                }
                pocz_x -= 5;
                pocz_y += 1;
                SheetPart = new Rectangle(krok * szerokosc, 0, szerokosc, wysokosc);
            }

            return krok;
        }

        public void Rysuj()
        {
            rozmiar += 0.0003f;
            spriteBatch.Draw(_background, new Rectangle(0, 0, _background.Width, _background.Height), Color.White);
            spriteBatch.Draw(tekstura, new Rectangle(pocz_x, krok + pocz_y, (int)(szerokosc * rozmiar), (int)(wysokosc * rozmiar)), SheetPart, Color.White);
        }
    }
}