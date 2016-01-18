using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Testgame.MIniGames.Swiming;
using TestGame.Menu;
using TestGame.MIniGames.Numbers;
using TestGame.Interfaces;

namespace TestGame.Scene
{
    public class Scene3 : Scene
    {
        private const int MAX_ELEMENTS = 14;
        // muzyka w tle
        private Song _themeSong;

        bool _blockD1 = false;
        bool _blockD2 = false;
        bool _blockD3 = false;
        bool _blockD4 = false;

        // menu wyboru ekwipunku
        private ChooseItemMenu _chooseItemMenu;
        private TextLabel _textLabel;

        private bool _canPlayMiniGamePuzzle;
        private bool _playMiniGamePuzzle;

        private List<Platform> partsPlane = new List<Platform>();
        private bool _firstStart;

        public Scene3(ContentManager content, Camera camera, GameTime gametime, GraphicsDevice device) : base(content, camera, gametime)
        {
            _chooseItemMenu = new ChooseItemMenu();
            _chooseItemMenu.IsVisible = false;
            _firstStart = true;

        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);


            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/0"), new Vector2(1254, 333), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/1"), new Vector2(1477, 392), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/2"), new Vector2(1287, 784), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/3"), new Vector2(431, 424), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/4"), new Vector2(2244, 732), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/5"), new Vector2(1700, 482), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/6"), new Vector2(313, 637), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/7"), new Vector2(1723, 658), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/8"), new Vector2(2548, 365), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/9"), new Vector2(877, 641), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/10"), new Vector2(877, 714), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/11"), new Vector2(2755, 494), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/12"), new Vector2(1335, 681), false, 0, 0, PlatformType.PARTSPLANE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/13"), new Vector2(2742, 385), false, 0, 0, PlatformType.PARTSPLANE));
         
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/Palmy"), new Vector2(0, 128), false, 0, 0, PlatformType.PALM));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/Krzaki"), new Vector2(0, 590), false, 0, 0, PlatformType.BUSH));

            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/PustaPlatforma"), new Vector2(0, 864), false, 0, 0, PlatformType.FLOOR));


            // muzyka tła
            _themeSong = content.Load<Song>("Audio/Waves/scene3_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = SoundManager.Volume;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            foreach (Platform platform in platforms)
                    platform.Draw(spriteBatch);

                foreach (Penguin penguin in penguins)
                    penguin.DrawAnimation(spriteBatch);

            foreach (Platform part in partsPlane)
                part.Draw(spriteBatch);

            // narysowanie menu wyboru ekwipunku
            _chooseItemMenu.Draw(spriteBatch);

        }

        private void AddItem(Platform platform)
        {
            platform.active = true;
            Rectangle tmp = platform.PlatformRectangle;
            tmp.Width /= 4;
            tmp.Height /= 4;    
            platform.PlatformRectangle = tmp;
            partsPlane.Add(platform);
            
        }
        private void UpdatePartsPlane(Penguin player)
        {
            if (_firstStart)
            {
                if (SoundManager.SoundOn)
                {
                    MediaPlayer.Play(_themeSong);
                }
                _firstStart = false;
            }

            for (int i = 0; i < partsPlane.Count; i++)
            {     
                Rectangle tmp = partsPlane[i].PlatformRectangle;
                if (i == 0)
                {
                    tmp.X = 70 + (player.rectangle.X - 400);
                    tmp.Y = 20;
                    partsPlane[i].PlatformRectangle = tmp;
                }
                else
                {
                    tmp.X = partsPlane[i - 1].PlatformRectangle.X + partsPlane[i - 1].PlatformRectangle.Width + 20;
                    tmp.Y = 20;
                    partsPlane[i].PlatformRectangle = tmp;
                    /*  if (i < 7)
                      {
                          tmp.X = partsPlane[i - 1].PlatformRectangle.X + partsPlane[i - 1].PlatformRectangle.Width + 20;
                          tmp.Y = 10;
                      }
                      else
                      {
                          if (i == 7) tmp.X = 150 + (player.rectangle.X - 400); else tmp.X = partsPlane[i - 1].PlatformRectangle.X + partsPlane[i - 1].PlatformRectangle.Width + 20;
                          tmp.Y = 70;
                      }

                      partsPlane[i].PlatformRectangle = tmp;*/
                }
               // partsPlane[i].PlatformRectangle.X = (player.rectangle.X - 400) + partsPlane[i].PlatformRectangle.X; ;
            }
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            // metoda ustawia wszystkich graczy na pozycji początkowej
            if (firstStart) FirstStart(gameTime);

            if (_playMiniGamePuzzle)
            {
                //todo: uruchomienie minigry
            }
            else
            {



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

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _canPlayMiniGamePuzzle)
                {
                    _playMiniGamePuzzle = true;
                }

                // odświeżenie paska gracza
                playerPanel.Update(player);

                int i;
                foreach (Platform platform in platforms)
                {
                    if (platform.active)
                    {
                        foreach (Penguin penguin in penguins)
                        {

                            //kolizja z innymi pingwinami
                            for (i = 0; i < penguins.Count; i++)
                            {
                                if (penguins[i].penguinType != penguin.penguinType)
                                    if (penguin.Collision(penguins[i].rectangle, penguins[i].penguinType))
                                        penguin.JumpStop(0);
                            }


                            if (platform.platformType == PlatformType.PARTSPLANE)
                            {
                                if (penguin.Collision2(platform.PlatformRectangle))
                                {
                                    platform.active = false;
                                    AddItem(platform);
                                }
                            }
                            else
                            {
                                if (platform.platformType != PlatformType.PALM &&
                                    platform.platformType != PlatformType.BUSH)
                                    if (penguin.Collision(platform.PlatformRectangle, PenguinType.NN,
                                        platform.platformType))
                                        // sprawdzenie czy na platformie są pingwiny
                                    {
                                        penguin.JumpStop((int) platform.PlatformSpeed);
                                        //zatrzymuje spadek pingwina jak wykryje kolizje z platforma 

                                        if (platform.IsMotion) // jak platforma sie porusza to pingwin razem z nią musi
                                        {
                                            penguin.PutMeOn(platform.PlatformRectangle);

                                            if (penguin.active) platform.Slowdown();
                                            penguin.platformSpeed = (int) platform.PlatformSpeed;
                                        }
                                    }
                                    else
                                    {
                                        if (penguin.active) platform.SpeedUp();
                                    }
                            }

                            // jeśli zostały zebrane wszytkie elementy 
                            // to nad głową Kowalskiego ma się pojaiwć zębatka
                            // Kowalski może złożyć samolot
                            if (penguin.penguinType == PenguinType.KOWALSKI &&
                                player.penguinType == PenguinType.KOWALSKI &&
                                partsPlane.Count == MAX_ELEMENTS)
                            {
                                _chooseItemMenu.Update(penguin, new List<Texture2D>()
                                {
                                    content.Load<Texture2D>(@"gear")
                                }, topMargin: 70);
                                _chooseItemMenu.IsVisible = true;
                                _canPlayMiniGamePuzzle = true;
                            }
                            else if (player.penguinType != PenguinType.KOWALSKI &&
                                     partsPlane.Count == MAX_ELEMENTS)
                            {
                                _chooseItemMenu.IsVisible = false;
                                _canPlayMiniGamePuzzle = false;
                            }


                        }
                        // aktualizacja pozycji jeśli platforma ma sie poruszać
                        platform.UpdatePosition(gameTime);
                    }

                }

                foreach (Penguin penguin in penguins)
                    penguin.UpdatePosition(gameTime);

                UpdatePartsPlane(player);
                camera.Update(player);
            }
        }

        public override void ResetScene()
        {
            // todo: reset ustawień
        }
    }

}
