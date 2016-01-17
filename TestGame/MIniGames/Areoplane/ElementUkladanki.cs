using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.MIniGames.Areoplane
{

    class ElementUkladanki
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private Texture2D Grafika;
        private Rectangle AktualnePolozenie;
        private Vector2 PolozenieWPrzyborniku;
        private Rectangle PolozenieNaUkladance;
        private bool Zlapany;

        public bool NaMejscu
        {
            get;
            private set;
        }
        private float czas;

        public ElementUkladanki(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content, string nazwa_sprita, Vector2 PolUklad)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _content = content;

            Grafika = content.Load<Texture2D>("Minigry/Areoplane/" + nazwa_sprita);
            PolozenieNaUkladance = new Rectangle((int)PolUklad.X, (int)PolUklad.Y, 2, 2);
            Zlapany = false;
            NaMejscu = false;
        }
        public int ZaladujDoPrzybornika(int y)
        {
            PolozenieWPrzyborniku.X = _graphicsDevice.Viewport.Width - 200 - Grafika.Width / 2 - 10;
            PolozenieWPrzyborniku.Y = y;
            AktualnePolozenie = new Rectangle((int)PolozenieWPrzyborniku.X, (int)PolozenieWPrzyborniku.Y, Grafika.Width, Grafika.Height);
            return y + Grafika.Height + 5;
        }

        public bool Zlap(int x, int y)
        {
            if (!NaMejscu)
            {
                Rectangle Mysz = new Rectangle(x, y, 1, 1);
                if (Mysz.Intersects(AktualnePolozenie))
                {
                    Zlapany = true;
                    return true;
                }
            }
            return false;
        }

        public void Scrolluj(int y)
        {
            if (!NaMejscu)
            {
                if (AktualnePolozenie.Y == PolozenieWPrzyborniku.Y)
                    AktualnePolozenie.Y += y;

                PolozenieWPrzyborniku.Y += y;

            }
        }

        public void Przesun(int x, int y)
        {
            if (Zlapany && !NaMejscu)
            {
                AktualnePolozenie.X = x - Grafika.Width / 2;
                AktualnePolozenie.Y = y - Grafika.Height / 2;
            }
        }

        public void Powrot(GameTime gt)
        {
            if (!Zlapany && !NaMejscu)
            {
                czas += (float)gt.ElapsedGameTime.TotalMilliseconds;

                if (czas >= 16f)
                {
                    if (AktualnePolozenie.X != PolozenieWPrzyborniku.X || AktualnePolozenie.Y != PolozenieWPrzyborniku.Y)
                    {
                        Vector2 aktualne = new Vector2(AktualnePolozenie.X, AktualnePolozenie.Y);
                        float distance = Vector2.Distance(aktualne, PolozenieWPrzyborniku);

                        AktualnePolozenie.X += (int)(((float)(PolozenieWPrzyborniku.X - AktualnePolozenie.X)) / (distance / 2));
                        AktualnePolozenie.Y += (int)(((float)(PolozenieWPrzyborniku.Y - AktualnePolozenie.Y)) / (distance / 2));
                    }

                    czas = 0;
                }
            }
        }

        public void Odloz()
        {
            Rectangle RogElementu = new Rectangle(AktualnePolozenie.X - 5, AktualnePolozenie.Y - 5, 10, 10);

            if (PolozenieNaUkladance.Intersects(RogElementu))
            {
                AktualnePolozenie.X = PolozenieNaUkladance.X;
                AktualnePolozenie.Y = PolozenieNaUkladance.Y;
                NaMejscu = true;
            }

            if (Zlapany)
                Zlapany = false;
        }

        public void Wyswiwietl()
        {
            _spriteBatch.Draw(Grafika, AktualnePolozenie, Color.White);

        }
    }
}
