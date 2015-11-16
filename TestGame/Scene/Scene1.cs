﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TestGame.MIniGames.Numbers;

namespace TestGame.Scene
{
    public class Scene1 : Scene
    {
        // automat
        private ActionElement _slotMachine;

        // Moneta
        private Bonus _coin;

        //minigierka  - AutomatMinigame
        private AutomatMinigame automatMinigame;

        //Aktywność minigierki
        private bool activeMiniGame = false;


        public Scene1(ContentManager content, Camera camera) : base(content, camera)
        {
            automatMinigame = new AutomatMinigame();
        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(-1100, 700)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(500, 850))); 
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/blat"), new Vector2(-50, 450)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/krzeslo"), new Vector2(1000, 500)));

            // stworzenie obiektu monety
            _coin = new Bonus(content.Load<Texture2D>("Scena1/Moneta"), new Point(50, 300), new Point(50));

            // stworzenie obiektu automatu
            Point slotMachineSize = new Point(content.Load<Texture2D>("Scena1/automat").Width, content.Load<Texture2D>("Scena1/automat").Height);
            _slotMachine = new ActionElement(content.Load<Texture2D>("Scena1/automat"), new Point(1200,150), slotMachineSize, 20);

            automatMinigame.LoadContent(content, content.Load<Texture2D>("Minigry/AutomatGame/Panel"));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(activeMiniGame == false)
            {
                // narysowanie monety
                _coin.Draw(spriteBatch);

                // narysowanie automatu
                _slotMachine.Draw(spriteBatch);



                foreach (Platform platform in platforms)
                    platform.Draw(spriteBatch);

                foreach (Penguin penguin in penguins)
                    penguin.Draw(spriteBatch);

            }else    automatMinigame.Draw(spriteBatch);
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            
            // metoda ustawia wszystkich graczy na pozycji początkowej
            if (firstStart) FirstStart();

            if (!activeMiniGame)
            {
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

                            for (i = 0; i < penguins.Count; i++)//sprawdza kolizje z innymi pingwinami i blokuje w przypadku wykrycia
                                if (penguins[i].penguinType != penguin.penguinType) penguins[i].CollisionPenguin(penguin.rectangle);


                            // moneta spada
                            if (_coin.IsActive)
                            {
                                _coin.FallDown();

                                if (_coin.IsCollisionDetect(platform))
                                {
                                    _coin.CanFallDown = false;
                                }

                            }
                            // sprawdzenie kolizji między pingwinem a automatem
                            if (_slotMachine.IsCollisionDetect(penguin))
                            {
                                penguin.Position.X -= 2;
                                //penguin.CanMove = false;
                            }

                            // sprawdzenie czy pingwin (RICO) nie zabrał monety
                            if (penguin.penguinType == PenguinType.RICO && _coin.IsChecked(penguin))
                            {
                                penguin.Equipment.AddItem(new EquipmentItem(_coin));
                                _coin.OnChecked();
                            }

                            if (_coin.IsCollisionDetect(_slotMachine))
                            {
                                activeMiniGame = true;
                                camera.active = true;
                                playerPanel.activeDraw = false;
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
            else automatMinigame.Update(gameTime);

            if(automatMinigame.GamePass)
            {
                activeMiniGame = false;
                camera.active = true;
                playerPanel.activeDraw = true;
            }

           
        }
 
    }

}
