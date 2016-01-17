using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame.MIniGames.Areoplane
{
    class SkladanieSamolotu
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        ElementUkladanki[] elementy;
        private Texture2D Tlo;
        int zlapany;
        int Mysz;

        public bool EndOfGame
        {
            get;
            private set;
        }

        public SkladanieSamolotu(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _content = content;


            Tlo = _content.Load<Texture2D>("Minigry/Areoplane/SamolotTlo");
            zlapany = -1;
            int y = 5;

            elementy = new ElementUkladanki[14];
            elementy[0] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "0", new Vector2(93f, 118f));
            elementy[1] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "1", new Vector2(154f, 317f));
            elementy[2] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "2", new Vector2(138f, 312f));
            elementy[3] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "3", new Vector2(220f, 381f));
            elementy[4] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "4", new Vector2(201f, 304f));
            elementy[5] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "5", new Vector2(507f, 233f));
            elementy[6] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "6", new Vector2(434f, 102f));
            elementy[7] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "7", new Vector2(117f, 184f));
            elementy[8] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "8", new Vector2(276f, 277f));
            elementy[9] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "9", new Vector2(91f, 83f));
            elementy[10] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "10", new Vector2(337f, 265f));
            elementy[11] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "11", new Vector2(102f, 206f));
            elementy[12] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "12", new Vector2(94f, 164f));
            elementy[13] = new ElementUkladanki(_graphicsDevice, spriteBatch, _content, "13", new Vector2(57f, 40f));

            for (int i = 0; i < 14; i++)
            {
                y = elementy[i].ZaladujDoPrzybornika(y);
            }

        }

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < 14; i++)
                    if (elementy[i].Zlap(Mouse.GetState().X, Mouse.GetState().Y) == true)
                    {
                        zlapany = i;
                        break;
                    }
            }



            if (Mysz != Mouse.GetState().ScrollWheelValue)
            {

                if (Mouse.GetState().ScrollWheelValue > -2300 && Mouse.GetState().ScrollWheelValue <= 0)
                {
                    int y = (int)((Mouse.GetState().ScrollWheelValue - Mysz) / 3);
                    Mysz = Mouse.GetState().ScrollWheelValue;

                    for (int i = 0; i < 14; i++)
                    {
                        elementy[i].Scrolluj(y);
                    }
                }
            }



            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                if (zlapany > -1)
                    elementy[zlapany].Odloz();
                zlapany = -1;
            }

            if (zlapany > -1)
            {
                elementy[zlapany].Przesun(Mouse.GetState().X, Mouse.GetState().Y);
            }

            for (int i = 0; i < 14; i++)
            {
                elementy[i].Powrot(gameTime);
            }
            foreach (ElementUkladanki element in elementy)
            {
                EndOfGame = element.NaMejscu;
            }
        }

        public void Draw()
        {
            _graphicsDevice.Clear(Color.Silver);
            _spriteBatch.Begin();
            _spriteBatch.Draw(Tlo, new Rectangle(0, 0, Tlo.Width, Tlo.Height), Color.White);

            for (int i = 0; i < 14; i++)
            {
                elementy[i].Wyswiwietl();
            }

            _spriteBatch.End();

        }

    }
}
