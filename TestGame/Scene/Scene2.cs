using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using TestGame.Menu;
using TestGame.MIniGames.Numbers;

namespace TestGame.Scene
{
    public class Scene2 : Scene
    {


        // menu wyboru ekwipunku
        private ChooseItemMenu _chooseItemMenu;

        private GameTime gametime;


        public Scene2(ContentManager content, Camera camera,GameTime gametime) : base(content, camera, gametime)
        {

        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);
            
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(-1100, 700)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(400, 850))); 
            //platforms.Add(new Platform(content.Load<Texture2D>("Scena1/blat"), new Vector2(500, 450),false,0,0,PlatformType.FLOOR));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/krzeslo_siedzenie"), new Vector2(-50, 539), false, 0, 0, PlatformType.FLOOR));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/krzeslo_oparcie"), new Vector2(124, 381)));

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
       
                // narysowanie menu wyboru ekwipunku
                _chooseItemMenu.Draw(spriteBatch);


                foreach (Platform platform in platforms)
                    platform.Draw(spriteBatch);

                foreach (Penguin penguin in penguins)
                {
                   
                        penguin.DrawAnimation(spriteBatch);
                 
                }

        }

        public override void UpdatePosition(GameTime gameTime)
        {
         
            // metoda ustawia wszystkich graczy na pozycji początkowej
            if (firstStart) FirstStart();


                if (Keyboard.GetState().IsKeyDown(Keys.D1)) player = ActiveAndDeactivationPlayer(true, false, false, false);
                if (Keyboard.GetState().IsKeyDown(Keys.D2)) player = ActiveAndDeactivationPlayer(false, true, false, false);
                if (Keyboard.GetState().IsKeyDown(Keys.D3)) player = ActiveAndDeactivationPlayer(false, false, true, false);
                if (Keyboard.GetState().IsKeyDown(Keys.D4)) player = ActiveAndDeactivationPlayer(false, false, false, true);


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
