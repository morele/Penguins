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
            _chooseItemMenu = new ChooseItemMenu();
        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            int YpositionFloor =700;
            
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/platforma2"), new Vector2(-1100, YpositionFloor)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/platforma2"), new Vector2(-100, YpositionFloor)));

            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/mur"), new Vector2(-1100, YpositionFloor + content.Load<Texture2D>("Scena2/platforma2").Height - 2)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/woda"), new Vector2(-1100 + content.Load<Texture2D>("Scena2/mur").Width, YpositionFloor + content.Load<Texture2D>("Scena2/platforma2").Height + 30)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/mur"), new Vector2(-1100 + content.Load<Texture2D>("Scena2/mur").Width + content.Load<Texture2D>("Scena2/woda").Width, YpositionFloor + content.Load<Texture2D>("Scena2/platforma2").Height - 2)));

            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Rura"), new Vector2(-700, YpositionFloor - content.Load<Texture2D>("Scena2/Rura").Height), false,0,0,PlatformType.MAGICPIPE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/autko/Autko1"), new Vector2(-600, YpositionFloor - content.Load<Texture2D>("Scena2/autko/Autko1").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kaluza"), new Vector2(0, YpositionFloor)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kolec"), new Vector2(400, YpositionFloor - content.Load<Texture2D>("Scena2/Kolec").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kolec"), new Vector2(440, YpositionFloor - content.Load<Texture2D>("Scena2/Kolec").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kolec"), new Vector2(480, YpositionFloor - content.Load<Texture2D>("Scena2/Kolec").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kolec"), new Vector2(520, YpositionFloor - content.Load<Texture2D>("Scena2/Kolec").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Rura"), new Vector2(800, YpositionFloor - content.Load<Texture2D>("Scena2/Rura").Height), false, 0, 0, PlatformType.MAGICPIPE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/wciecie"), new Vector2(981, YpositionFloor)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/waga"), new Vector2(1035, YpositionFloor + 46 - content.Load<Texture2D>("Scena2/waga").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Skrzynia"), new Vector2(1045, YpositionFloor + 46 - content.Load<Texture2D>("Scena2/waga").Height - content.Load<Texture2D>("Scena2/Skrzynia").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/platforma1"), new Vector2(1221, YpositionFloor)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Sprezyna"), new Vector2(1400, YpositionFloor - content.Load<Texture2D>("Scena2/Sprezyna").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/SprezynaPlatforma"), new Vector2(1400, YpositionFloor - content.Load<Texture2D>("Scena2/Sprezyna").Height - content.Load<Texture2D>("Scena2/SprezynaPlatforma").Height),false,0,0,PlatformType.SPRING));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/mur"), new Vector2(1580, YpositionFloor - content.Load<Texture2D>("Scena2/mur").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Zapadka"), new Vector2(1580, YpositionFloor - content.Load<Texture2D>("Scena2/Zapadka").Height - content.Load<Texture2D>("Scena2/mur").Height), false, 0,0,PlatformType.PAWL));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Sciana"), new Vector2(1580, YpositionFloor - content.Load<Texture2D>("Scena2/Zapadka").Height - content.Load<Texture2D>("Scena2/mur").Height - content.Load<Texture2D>("Scena2/Sciana").Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Sciana"), new Vector2(1580 + 89, YpositionFloor - content.Load<Texture2D>("Scena2/Zapadka").Height - content.Load<Texture2D>("Scena2/mur").Height - content.Load<Texture2D>("Scena2/Sciana").Height)));
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
