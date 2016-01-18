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
    public class Scene1 : Scene
    {
        bool _blockD1 = false;
        bool _blockD2 = false;
        bool _blockD3 = false;
        bool _blockD4 = false;

        // muzyka w tle
        private Song _themeSong;

        // automat
        private ActionElement _slotMachine;

        // Moneta
        private Bonus _coin;


        //minigierka  - AutomatMinigame
        private AutomatMinigame automatMinigame;

        //Aktywność minigierki
        private bool activeMiniGame = false;

        // menu wyboru ekwipunku
        private ChooseItemMenu _chooseItemMenu;

        private GameTime gametime;

        public Scene1(ContentManager content, Camera camera,GameTime gametime) : base(content, camera, gametime)
        {
            automatMinigame = new AutomatMinigame();

            _chooseItemMenu = new ChooseItemMenu();

        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(-1100, 700)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(400, 850))); 
            //platforms.Add(new Platform(content.Load<Texture2D>("Scena1/blat"), new Vector2(500, 450),false,0,0,PlatformType.FLOOR));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/krzeslo_siedzenie"), new Vector2(-50, 539), false, 0, 0, PlatformType.FLOOR));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/krzeslo_oparcie"), new Vector2(124, 381)));
            // stworzenie obiektu monety
            _coin = new Bonus(content.Load<Texture2D>("Scena1/Moneta"), new Point(50, 300), new Point(50));
            _coin.IsActive = true;

            // stworzenie obiektu automatu
            Point slotMachineSize = new Point(content.Load<Texture2D>("Scena1/automat").Width, content.Load<Texture2D>("Scena1/automat").Height);
            _slotMachine = new ActionElement(content.Load<Texture2D>("Scena1/automat"), new Point(1200, 150),
                slotMachineSize, 50);

            automatMinigame.LoadContent(content, content.Load<Texture2D>("Minigry/AutomatGame/Panel"));

            // muzyka tła
            _themeSong = content.Load<Song>("Audio/Waves/scene1_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = SoundManager.Volume;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
       
            if (activeMiniGame == false)
            {
                // narysowanie monety
                _coin.Draw(spriteBatch);

                // narysowanie automatu
                _slotMachine.Draw(spriteBatch);


                // narysowanie menu wyboru ekwipunku
                _chooseItemMenu.Draw(spriteBatch);


                foreach (Platform platform in platforms)
                    platform.Draw(spriteBatch);

                foreach (Penguin penguin in penguins)
                {
                   
                        penguin.DrawAnimation(spriteBatch);
                    
                 
                   
                }
                

            }else    automatMinigame.Draw(spriteBatch);
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            if (firstStart)
            {
                if (SoundManager.SoundOn)
                {
                    MediaPlayer.Play(_themeSong);
                }
            }

            // metoda ustawia wszystkich graczy na pozycji początkowej
            if (firstStart) FirstStart(gameTime);

            if (!activeMiniGame)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D1) && !_blockD1)
                {
                    _blockD1 = true;
                    player = ActiveAndDeactivationPlayer(true, false, false, false);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D2) && !_blockD2)
                {
                    _blockD2 = true;
                    player = ActiveAndDeactivationPlayer(false, true, false, false);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D3) && !_blockD3)
                {
                    _blockD3 = true;
                    player = ActiveAndDeactivationPlayer(false, false, true, false);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D4) && !_blockD4)
                {
                    _blockD4 = true;
                    player = ActiveAndDeactivationPlayer(false, false, false, true);
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
                            // moneta spada
                            if (_coin.IsActive)
                            {
                                _coin.FallDown();

                                if (_coin.IsCollisionDetect(platform))
                                {
                                    _coin.CanFallDown = false;
                                }

                            }

                            // sprawdzenie czy pingwin (RICO) jest w obrębie automatu
                            if (penguin.penguinType == PenguinType.RICO && _slotMachine.IsInActionSector(penguin))
                            {
                                // wyświetlenie menu wyboru ekwipunku
                                if (penguin.Equipment.Items.Count > 0)
                                {
                                    _chooseItemMenu.IsVisible = true;
                                    List<Texture2D> textures = penguin.Equipment.Items.Select(equipmentItem => equipmentItem.Item.Texture).ToList();
                                    _chooseItemMenu.Update(penguin, textures);
                                    penguin.SelectedItem = penguin.Equipment.Items[_chooseItemMenu.SelectedIndex];
                                }
                            }
                            else if(penguin.penguinType == PenguinType.RICO && !_slotMachine.IsInActionSector(penguin))
                            {
                                _chooseItemMenu.IsVisible = false;
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


                            // sprawdzenie kolizji wyrzuconej monety z automatem
                            if (_coin.IsCollisionDetect(_slotMachine))
                            {
                                activeMiniGame = true;
                                camera.active = true;
                                playerPanel.activeDraw = false;
                            }



                            if (penguin.Collision(platform.PlatformRectangle, PenguinType.NN, platform.platformType))// sprawdzenie czy na platformie są pingwiny
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
            else automatMinigame.Update(gameTime);

            if(automatMinigame.GamePass)
            {
                // zakończenie poziomu
                IsCompleted = true;
                activeMiniGame = false;
                camera.active = false;
                playerPanel.activeDraw = true;
            }

           
        }

        public override void ResetScene()
        {
            activeMiniGame = false;
            IsCompleted = false;
            activeMiniGame = false;
            camera.active = false;
        }
    }

}
