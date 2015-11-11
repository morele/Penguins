using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lab2
{
    class Plansza
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private ContentManager Content;
        private Karta[] Karty;
        private Texture2D Obrazki;
        private Texture2D Zakrycie;
        private int odkrytych;
        private float Odczekaj = 500f;
        private float Naczekal;
        private int[] Odkryte;
        private int[] Tablica;
        public int LiczbaKrokow;
        public int liczbakart;
        private float Skala;
        int rozm_na_planszy=150;

        public Plansza(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager content)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            Content = content;
            Karty=new Karta[24];
            Obrazki = Content.Load<Texture2D>(@"Karty");
            Zakrycie = Content.Load<Texture2D>(@"Zakrywka");
            Naczekal = 0;
            odkrytych = 0;
            Odkryte = new int[2];
            liczbakart = 24;
            LiczbaKrokow = 0;
            LosujIndexy();
            StworzPlansze();
        }

        private void StworzPlansze()
        {
            int poz_x=0;
            int poz_y=0;
            int text_x = 0;
            int text_y = 0;
            int szer_wycinka = Obrazki.Width/4;
            int wys_wycinka = Obrazki.Height/3;
         


            Rectangle[] Wycinek=new Rectangle[24];
            Vector2[] Pozyja=new Vector2[24];
            

            for (int i = 0; i < 24; i++)
            {
                Wycinek[i] = new Rectangle(text_x*szer_wycinka, text_y*wys_wycinka, szer_wycinka, wys_wycinka);
                Pozyja[i] = new Vector2(poz_x*(rozm_na_planszy + 10), poz_y*(rozm_na_planszy + 10));
                poz_x++;
                text_x++;
                if (poz_x >= 6)
                {
                    poz_x = 0;
                    poz_y++;
                }

                if (text_x >= 4)
                {
                    text_x = 0;
                    text_y++;
                }

                if (text_y >= 3)
                {
                    text_y = 0;
                    text_x = 0;
                }
            }


            for (int i = 0; i < 24; i++)
            {
                Karty[i]=new Karta(Wycinek[Tablica[i]], Pozyja[i]);
            }
        }


     

        public void Rysuj()
        {
            for (int i = 0; i < 24; i++)
            {
                if (Karty[i].wyswietl)
                {
                    spriteBatch.Draw(Obrazki, Karty[i].Pozycja, Karty[i].Wycinek, Color.White);

                    if (Karty[i].zakryte)
                        spriteBatch.Draw(Zakrycie, Karty[i].Pozycja, Color.White);
                }

            }

        }

        public void OkryjKarte(int x, int y)
        {
            if (odkrytych < 2)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (Karty[i].Pozycja.Intersects(new Rectangle(x, y, 1, 1)) && Karty[i].wyswietl && Karty[i].zakryte)
                    {
                        Odkryte[odkrytych] = i;
                        Karty[i].zakryte = false;
                        odkrytych++;
                    }
                }
            }
        }

        public void Zakryj(GameTime gt)
        {
            if (odkrytych == 2)
            {
                if (!SprawdzKarty())
                {
                    
                    Naczekal += (float) gt.ElapsedGameTime.TotalMilliseconds;

                    if (Naczekal >= Odczekaj)
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            Karty[i].zakryte = true;

                        }

                        Naczekal = 0;
                        Odkryte = new int[2];
                        odkrytych = 0;
                        LiczbaKrokow++;
                    }
                }

            }
            else
            {
                Naczekal = 0;
            }
                
        }

        private bool SprawdzKarty()
        {
            if (Karty[Odkryte[0]].Wycinek.Intersects(Karty[Odkryte[1]].Wycinek))
            {
                Karty[Odkryte[0]].wyswietl = false;
                Karty[Odkryte[0]].zakryte = false;
                Karty[Odkryte[1]].wyswietl = false;
                Karty[Odkryte[1]].zakryte = false;
                odkrytych = 0;
                LiczbaKrokow++;
                liczbakart -= 2;
                return true;
            }
            return false;
        }

        
        private void LosujIndexy()
        {
            Tablica=new int[24];
            
            
            Random los=new Random();

            for (int i = 0; i < 24; i++)
            {
                int wylosowane=los.Next(24);
                
            int j = 0;
                for (j=0; j < i; j++)
                {
                    if (Tablica[j] == wylosowane)
                    {
                        j = 24;
                        i--;
                    }
                }

                if (j < 24)
                    Tablica[i] = wylosowane;
            }


        }

        public void ZmienSkale()
        {
          
        }


    }
}
