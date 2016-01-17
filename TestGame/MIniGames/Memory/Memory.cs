using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TestGame.MIniGames.Memory
{

    class Memory
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;

        private Plansza plansza;
        private Przeciwnik Julian;
        bool blokada;
        int liczba_kart;
        int punkty_gracza;
        float Naliczanie;
        Texture2D Wygrana;
        Texture2D Przegrana;
        Rectangle PolozenieTextury;
        Rectangle PolozenieKlatki;
        Texture2D JulianTex;
        Texture2D SkipperTex;
        SpriteFont Czcionka;
        private GameTime _gameTime;
        public bool EndOfGame
        {
            get;
            private set;
        }

        int krok;
        int rozm_postaci = 150;

        public Memory(SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphicsDevice)
        {
            if (spriteBatch == null) throw new NullReferenceException("spriteBatch in Memory");
            if (content == null) throw new NullReferenceException("content in Memory");
            if (graphicsDevice == null) throw new NullReferenceException("graphicsDevice in Memory");

            _spriteBatch = spriteBatch;
            _content = content;
            _graphicsDevice = graphicsDevice;
        }
        public void LoadContent()
        {
            //     spriteBatch = new SpriteBatch(GraphicsDevice);
            Wygrana = _content.Load<Texture2D>("Minigry/Memory/Wygrana");
            Przegrana = _content.Load<Texture2D>("Minigry/Memory/JulianSprite");
            JulianTex = _content.Load<Texture2D>("Minigry/Memory/Julian");
            SkipperTex = _content.Load<Texture2D>("Minigry/Memory/Skipper");
            Czcionka = _content.Load<SpriteFont>("Minigry/Memory/Liczby");
            plansza = new Plansza(_graphicsDevice, _spriteBatch, _content);
            Julian = new Przeciwnik(plansza.Karty);
            punkty_gracza = 0;
            Naliczanie = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                _gameTime = gameTime;
            }
            liczba_kart = plansza.liczbakart;
            plansza.Zakryj(gameTime);


            if (liczba_kart > 0)
            {
                if (plansza.blokuj == false)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        plansza.OkryjKarte(Mouse.GetState().X, Mouse.GetState().Y);

                        if (plansza.liczbakart < liczba_kart)
                        {
                            punkty_gracza++;
                        }
                    }
                    Naliczanie = 0;
                }
                else
                {
                    if (Naliczanie < 5f)
                    {
                        Naliczanie = 5;
                        Karta[] wybor = Julian.Losuj();
                        plansza.OkryjKarte(wybor[0].Pozycja.X + 1, wybor[0].Pozycja.Y + 1);
                        plansza.OkryjKarte(wybor[1].Pozycja.X + 1, wybor[1].Pozycja.Y + 1);
                    }

                    if (plansza.liczbakart < liczba_kart)
                    {
                        Julian.DodajPunkty();
                        Naliczanie = 0;
                        plansza.blokuj = true;
                    }

                }
            }


            Julian.UpdateKarty(plansza.Karty);


        }

        public void Draw()
        {
            _graphicsDevice.Clear(Color.Black);

            if (liczba_kart == 0)
            {
                if (Julian.Punkty < punkty_gracza)
                {
                    PolozenieTextury = new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
                    _spriteBatch.Draw(Wygrana, PolozenieTextury, Color.White);
                }
                else
                {
                    PolozenieTextury = new Rectangle(_graphicsDevice.Viewport.Width - (int)(Przegrana.Width / 4), 0, Przegrana.Width / 4, Przegrana.Height / 3);
                    Naliczanie += (float)_gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (Naliczanie > 70f)
                    {
                        krok++;
                        if (krok == 4)
                            krok = 0;
                        Naliczanie = 0;
                    }
                    PolozenieKlatki = new Rectangle((int)(krok * Przegrana.Width / 4), (int)(Przegrana.Height / 3), Przegrana.Width / 4, Przegrana.Height / 3);
                    _spriteBatch.Draw(Przegrana, PolozenieTextury, PolozenieKlatki, Color.White);
                }
            }
            else
            {


                _spriteBatch.Draw(SkipperTex, new Rectangle(20, 20, rozm_postaci, rozm_postaci), Color.White);
                _spriteBatch.DrawString(Czcionka, punkty_gracza.ToString(), new Vector2(_graphicsDevice.Viewport.Width / 2 - 200, 50), Color.White);

                _spriteBatch.Draw(JulianTex, new Rectangle(_graphicsDevice.Viewport.Width - rozm_postaci - 20, 20, rozm_postaci, rozm_postaci), Color.White);
                _spriteBatch.DrawString(Czcionka, Julian.Punkty.ToString(), new Vector2(_graphicsDevice.Viewport.Width / 2 + 200, 50), Color.White);

                plansza.Rysuj();
            }

        }
    }
}
