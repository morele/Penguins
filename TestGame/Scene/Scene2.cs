using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using TestGame.Menu;
using TestGame.MIniGames.Numbers;

namespace TestGame.Scene
{
    public class Scene2 : Scene
    {
        // muzyka w tle
        private Song _themeSong;

        bool _blockD1 = false;
        bool _blockD2 = false;
        bool _blockD3 = false;
        bool _blockD4 = false;

        // menu wyboru ekwipunku
        private ChooseItemMenu _chooseItemMenu;

        private GameTime gametime;


        public Scene2(ContentManager content, Camera camera,GameTime gametime) : base(content, camera, gametime)
        {
            _chooseItemMenu = new ChooseItemMenu();
        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            int YpositionFloor = 700;
            Texture2D mur = content.Load<Texture2D>("Scena2/mur");
            Texture2D platfroma2 = content.Load<Texture2D>("Scena2/platforma2");
            Texture2D kolec = content.Load<Texture2D>("Scena2/Kolec");
            Texture2D waga = content.Load<Texture2D>("Scena2/waga");
            Texture2D woda = content.Load<Texture2D>("Scena2/woda");
            Texture2D rura = content.Load<Texture2D>("Scena2/Rura");
            Texture2D zapadka = content.Load<Texture2D>("Scena2/Zapadka");
            Texture2D platforma1 = content.Load<Texture2D>("Scena2/platforma1");
            Texture2D sciana = content.Load<Texture2D>("Scena2/Sciana");
            Texture2D sprezynaPlatforma = content.Load<Texture2D>("Scena2/SprezynaPlatforma");
            Texture2D sprezyna = content.Load<Texture2D>("Scena2/Sprezyna");

            //platformy
            platforms.Add(new Platform(platfroma2, new Vector2(-1100, YpositionFloor)));
            platforms.Add(new Platform(platfroma2, new Vector2(-100, YpositionFloor)));
            platforms.Add(new Platform(platforma1, new Vector2(1221, YpositionFloor)));

            //woda i ograniczenia
            platforms.Add(new Platform(mur, new Vector2(-1100, YpositionFloor + platfroma2.Height - 2)));
            platforms.Add(new Platform(woda, new Vector2(-1100 + mur.Width, YpositionFloor + platfroma2.Height + 30)));
            platforms.Add(new Platform(mur, new Vector2(-1100 + mur.Width + woda.Width, YpositionFloor + platfroma2.Height - 2)));


            platforms.Add(new Platform(rura, new Vector2(-700, YpositionFloor - rura.Height), false,0,0,PlatformType.MAGICPIPE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/autko/Autko1"), new Vector2(-600, YpositionFloor - content.Load<Texture2D>("Scena2/autko/Autko1").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kaluza"), new Vector2(0, YpositionFloor)));
            platforms.Add(new Platform(kolec, new Vector2(400, YpositionFloor - kolec.Height)));
            platforms.Add(new Platform(kolec, new Vector2(440, YpositionFloor - kolec.Height)));
            platforms.Add(new Platform(kolec, new Vector2(480, YpositionFloor - kolec.Height)));
            platforms.Add(new Platform(kolec, new Vector2(520, YpositionFloor - kolec.Height)));
            platforms.Add(new Platform(rura, new Vector2(800, YpositionFloor - rura.Height), false, 0, 0, PlatformType.MAGICPIPE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/wciecie"), new Vector2(981, YpositionFloor)));
            platforms.Add(new Platform(waga, new Vector2(1035, YpositionFloor + 46 - waga.Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Skrzynia"), new Vector2(1045, YpositionFloor + 46 - waga.Height - content.Load<Texture2D>("Scena2/Skrzynia").Height)));        
            platforms.Add(new Platform(sprezyna, new Vector2(1400, YpositionFloor - sprezyna.Height)));
            platforms.Add(new Platform(sprezynaPlatforma, new Vector2(1400, YpositionFloor - sprezyna.Height - sprezynaPlatforma.Height),false,0,0,PlatformType.SPRING));
            platforms.Add(new Platform(mur, new Vector2(1580, YpositionFloor - mur.Height)));
            platforms.Add(new Platform(zapadka, new Vector2(1580, YpositionFloor - zapadka.Height - mur.Height), false, 0,0,PlatformType.PAWL));
            platforms.Add(new Platform(sciana, new Vector2(1580, YpositionFloor - zapadka.Height - mur.Height - sciana.Height)));
            platforms.Add(new Platform(sciana, new Vector2(1580 + 89, YpositionFloor - zapadka.Height - mur.Height - sciana.Height)));

            // muzyka tła
            _themeSong = content.Load<Song>("Audio/Waves/scene1_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_themeSong);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
       
                // narysowanie menu wyboru ekwipunku
                _chooseItemMenu.Draw(spriteBatch);


                foreach (Platform platform in platforms)
                    platform.Draw(spriteBatch);

                foreach (Penguin penguin in penguins)
                    penguin.DrawAnimation(spriteBatch);
                 

        }

        public override void UpdatePosition(GameTime gameTime)
        {
         
            // metoda ustawia wszystkich graczy na pozycji początkowej
            if (firstStart) FirstStart(gameTime);


            if (Keyboard.GetState().IsKeyDown(Keys.D1) && !_blockD1)
            {
                player = ActiveAndDeactivationPlayer(true, false, false, false);
                _blockD1 = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2) && !_blockD2)
            {
                player = ActiveAndDeactivationPlayer(false, true, false, false);
                _blockD2 = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3) && !_blockD3)
            {
                player = ActiveAndDeactivationPlayer(false, false, true, false);
                _blockD3 = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4) && !_blockD4)
            {
                player = ActiveAndDeactivationPlayer(false, false, false, true);
                _blockD4 = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.D1)) _blockD1 = false;
            if (Keyboard.GetState().IsKeyUp(Keys.D2)) _blockD2 = false;
            if (Keyboard.GetState().IsKeyUp(Keys.D3)) _blockD3 = false;
            if (Keyboard.GetState().IsKeyUp(Keys.D4)) _blockD4 = false;

            // odświeżenie paska gracza
            playerPanel.Update(player);

                int i;
                foreach (Platform platform in platforms)
                {
                    if (platform.active)
                    {
                        foreach (Penguin penguin in penguins)
                        {

                         /*   for (i = 0; i < penguins.Count; i++)//sprawdza kolizje z innymi pingwinami i blokuje w przypadku wykrycia
                                if (penguins[i].penguinType != penguin.penguinType) penguins[i].CollisionPenguin(penguin.rectangle);*/

                            for (i = 0; i < penguins.Count; i++)
                            {
                                if (penguins[i].penguinType != penguin.penguinType)
                                       if(penguins[i].Collision(penguin.rectangle))
                                        penguins[i].JumpStop(0);
                            }

                            //Jak kolizja ze sprezyna to wysoki jump
                            if(platform.platformType == PlatformType.SPRING)
                                if(penguin.Collision(platform.PlatformRectangle))
                                {
                                penguin.JumpSpring();
                                 }



                            if (penguin.Collision(platform.PlatformRectangle, platform.platformType))// sprawdzenie czy na platformie są pingwiny
                            {
                                penguin.JumpStop((int)platform.PlatformSpeed); //zatrzymuje spadek pingwina jak wykryje kolizje z platforma 

                                if (platform.IsMotion) // jak platforma sie porusza to pingwin razem z nią musi
                                {
                                    penguin.PutMeOn(platform.PlatformRectangle);

                                    if (penguin.active) platform.Slowdown();
                                    penguin.platformSpeed = (int)platform.PlatformSpeed;
                                }
                            }
                            else
                            {
                                if (penguin.active) platform.SpeedUp();
                            }

                        }
                        // aktualizacja pozycji jeśli platforma ma sie poruszać
                        platform.UpdatePosition(gameTime);
                    }

                }



                foreach (Penguin penguin in penguins)
                    penguin.UpdatePosition(gameTime);
            

            camera.Update(player);
        }
 
    }

}
