using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame.Scene
{
    public class Scene1 : Scene
    {
        public ActionElement SlotMachine { get; private set; }

        public Scene1(ContentManager content, Camera camera) : base(content, camera) { }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(-1000, 600)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(500, 600)));
           // platforms.Add(new Platform(content.Load<Texture2D>("Scena1/automat"), new Vector2(1000, 242)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/blat"), new Vector2(-50, 450)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/Moneta"), new Vector2(50, 300), false, 0, 0, PlatformType.MONEY));
            
            Point slotMachineSize = new Point(content.Load<Texture2D>("Scena1/automat").Width, content.Load<Texture2D>("Scena1/automat").Height);
            SlotMachine = new ActionElement(content.Load<Texture2D>("Scena1/automat"), new Point(1000, 242), slotMachineSize);
        }

        public override void UpdatePosition()
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
                if(platform.active)
                {
                    foreach (Penguin penguin in penguins)
                    {

                        for (i = 0; i < penguins.Count; i++)//sprawdza kolizje z innymi pingwinami i blokuje w przypadku wykrycia
                            if (penguins[i].penguinType != penguin.penguinType) penguins[i].CollisionPenguin(penguin.rectangle);

                        for (i = 0; i < platforms.Count; i++)
                        {              
                            if (platform.platformType != platforms[i].platformType && platform.CollisionPlatform(platforms[i].PlatformRectangle))
                            {
                                platform.jump = false;
                            }
                        }

                        // sprawdzenie kolizji między pingwinem a automatem
                        if (SlotMachine.IsCollisionDetect(penguin))
                        {
                            penguin.Position.X -= 1;
                            //penguin.CanMove = false;
                        }  

                        if (penguin.CollisionPlatform(platform, PlatformType.MONEY))
                        {
                            penguin.Equipment.AddItem(new EquipmentItem(content.Load<Texture2D>("Scena1/Moneta")));
                            platform.active = false;
                        }


                        if (penguin.IsOnTopOf(platform))// sprawdzenie czy na platformie są pingwiny
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
                    platform.UpdatePosition();
                }
                
            }



            foreach (Penguin penguin in penguins)
                penguin.UpdatePosition();


            camera.Update(player);
        }
 
    }

}
