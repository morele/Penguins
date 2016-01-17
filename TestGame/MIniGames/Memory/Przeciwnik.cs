using System;

namespace TestGame.MIniGames.Memory
{
    public class Przeciwnik
    {
        public int Punkty;
        private Karta[] Karty;
        private short[] odkrywane;
        private short odkrywanych;

        public Przeciwnik(Karta[] karty)
        {
            Karty = karty;
            Punkty = 0;
            odkrywane = new short[24];
            odkrywanych = 0;
        }

        public Karta[] Losuj()
        {
            Karta[] losowane = new Karta[2];
            int x = 0;
            int p = 0;
            bool powtorz = false;

            Random random = new Random();

            if (odkrywanych > 0)
            {
                do
                {

                    x = random.Next(odkrywanych);
                    x = odkrywane[x];

                    if (p > odkrywanych)
                        x++;

                    if (Karty[x].wyswietl == false)
                    {
                        p++;
                        if (p >= odkrywanych)
                            x++;
                        powtorz = true;
                    }
                    else
                    {
                        powtorz = false;
                    }

                } while (powtorz);

            }
            else
            {
                do
                {
                    x = random.Next(24);
                    odkrywane[odkrywanych] = (short)x;

                    if (Karty[x].wyswietl == false)
                    {
                        powtorz = true;
                    }
                    else
                    {
                        powtorz = false;
                    }

                } while (powtorz);

            }

            int y;

            do
            {
                y = random.Next(24);

                if (Karty[y].wyswietl == false || x == y)
                {
                    powtorz = true;
                }
                else
                {
                    powtorz = false;
                }

            } while (powtorz);


            for (int i = 0; i < odkrywanych; i++)
            {
                if (odkrywane[i] == y)
                {
                    losowane[0] = Karty[x];
                    losowane[1] = Karty[y];

                    return losowane;
                }
            }

            odkrywane[odkrywanych] = (short)y;
            odkrywanych++;

            losowane[0] = Karty[x];
            losowane[1] = Karty[y];

            return losowane;
        }

        public void UpdateKarty(Karta[] karty)
        {
            Karty = karty;
        }

        public void DodajPunkty()
        {
            Punkty++;
        }
    }
}