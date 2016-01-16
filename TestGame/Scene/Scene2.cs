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
        private TextLabel _textLabel;

        // minigra
        private bool _playMiniGame;
        private Swiming _miniGame;
        private bool _canPlayMiniGame;

        // rybka zebrana przez Rico
        private Bonus _fishItem;

        // skrzynka do której Rico ma wrzucić rybki
        private ActionElement _fishBox;

        private GameTime gametime;

        public Scene2(ContentManager content, Camera camera, GameTime gametime, GraphicsDevice device) : base(content, camera, gametime)
        {
            _chooseItemMenu = new ChooseItemMenu();
            _chooseItemMenu.IsVisible = false;
            
            _miniGame = new Swiming(new SpriteBatch(device), content, device);

        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            int YpositionFloor = 700;
            Texture2D mur = content.Load<Texture2D>("Scena2/mur");
            Texture2D platfroma2 = content.Load<Texture2D>("Scena2/platforma2");



            Texture2D kolec = content.Load<Texture2D>("Scena2/Kolec");


            Texture2D waga = content.Load<Texture2D>("Scena2/waga");

            Texture2D woda = content.Load<Texture2D>("Scena2/woda/WodaAnimacja");

            // Animation Woda=new Animation(content.Load<Texture2D>("Scena2/woda/WodaAnimacja"),3,120, new Vector2(-1100 + mur.Width, YpositionFloor + platfroma2.Height + 30));

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

            for (int i = 1; i <= 10; i++)
            {
                platforms.Add(new Platform(new Animation(content.Load<Texture2D>("Scena2/woda/WodaAnimacja"), 3, 120,
                        new Vector2(-1100 + mur.Width * i, YpositionFloor + platfroma2.Height + 30))));
            }
            // platforms.Add(new Platform(Woda));
            platforms.Add(new Platform(mur, new Vector2(-1100 + mur.Width + woda.Width, YpositionFloor + platfroma2.Height - 2)));


            platforms.Add(new Platform(rura, new Vector2(-700, YpositionFloor - rura.Height), false, 0, 0, PlatformType.MAGICPIPE));

            platforms.Add(new Platform(new Animation(content.Load<Texture2D>("Scena2/AnimacjaTla/AutkoAnimacja"), 6, 50,
                new Vector2(-600, YpositionFloor - content.Load<Texture2D>("Scena2/autko/Autko1").Height))));

            //   platforms.Add(new Platform(content.Load<Texture2D>("Scena2/autko/Autko1"), new Vector2(-600, YpositionFloor - content.Load<Texture2D>("Scena2/autko/Autko1").Height), false, 0, 0, PlatformType.CAR));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kaluza"), new Vector2(0, YpositionFloor - 150), false, 0, 0, PlatformType.CAR));



            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(400, YpositionFloor - kolec.Height)), false, 0, 0, PlatformType.SPIKE));
            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(440, YpositionFloor - kolec.Height)), false, 0, 0, PlatformType.SPIKE));
            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(480, YpositionFloor - kolec.Height)), false, 0, 0, PlatformType.SPIKE));
            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(520, YpositionFloor - kolec.Height)), false, 0, 0, PlatformType.SPIKE));




            platforms.Add(new Platform(rura, new Vector2(800, YpositionFloor - rura.Height), false, 0, 0, PlatformType.MAGICPIPE));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/wciecie"), new Vector2(981, YpositionFloor)));
            platforms.Add(new Platform(waga, new Vector2(1035, YpositionFloor + 46 - waga.Height)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Skrzynia"), new Vector2(1045, YpositionFloor + 46 - waga.Height - content.Load<Texture2D>("Scena2/Skrzynia").Height)));
            platforms.Add(new Platform(sprezyna, new Vector2(1400, YpositionFloor - sprezyna.Height)));
            platforms.Add(new Platform(sprezynaPlatforma, new Vector2(1400, YpositionFloor - sprezyna.Height - sprezynaPlatforma.Height), false, 0, 0, PlatformType.SPRING));
            platforms.Add(new Platform(mur, new Vector2(1580, YpositionFloor - mur.Height)));
            platforms.Add(new Platform(zapadka, new Vector2(1580, YpositionFloor - zapadka.Height - mur.Height), false, 0, 0, PlatformType.PAWL));
            platforms.Add(new Platform(sciana, new Vector2(1580, YpositionFloor - zapadka.Height - mur.Height - sciana.Height)));
            platforms.Add(new Platform(sciana, new Vector2(1580 + 89, YpositionFloor - zapadka.Height - mur.Height - sciana.Height)));

            // utworzenie obiektu skrzynki na ryby
            float fishBoxHeight = content.Load<Texture2D>("Scena2/Skrzynia").Height;
            float fishBoxWidth = content.Load<Texture2D>("Scena2/Skrzynia").Width;

            Point fishBoxPosition = new Point(1045, (int)(YpositionFloor + 46 - waga.Height - fishBoxHeight));
            Point fishBoxSize = new Point((int)fishBoxWidth, (int)fishBoxHeight);
            _fishBox = new ActionElement(content.Load<Texture2D>("Scena2/Skrzynia"), fishBoxPosition, fishBoxSize, actionSector: 40);

            // rybka 
            Point fishItemSize = new Point(content.Load<Texture2D>(@"Scena2/ryba").Width, content.Load<Texture2D>(@"Scena2/ryba").Height);
            _fishItem = new Bonus(content.Load<Texture2D>(@"Scena2/ryba"), new Point(50, 300), fishItemSize);
            _fishItem.IsActive = true;
            // muzyka tła
            if (SoundManager.SoundOn)
            {
                _themeSong = content.Load<Song>("Audio/Waves/scene1_theme");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = SoundManager.Volume;
                MediaPlayer.Play(_themeSong);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_playMiniGame)
            {
                _miniGame.Draw(spriteBatch);
            }
            else
            {
                

                if (_miniGame.EndOfGame)
                    _textLabel.Draw(spriteBatch, true);

                // narysowanie menu wyboru ekwipunku
                _chooseItemMenu.Draw(spriteBatch);

                foreach (Platform platform in platforms)
                    platform.Draw(spriteBatch);

                foreach (Penguin penguin in penguins)
                    penguin.DrawAnimation(spriteBatch);

            }
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            // to się wykona tylko raz po zakończeniu minigry
            if (_miniGame.EndOfGame && _canPlayMiniGame)
            {
                // dodanie ryb do ekwipunku Rico
                var rico = penguins.FirstOrDefault(p => p.penguinType == PenguinType.RICO);
                rico.Equipment.AddItem(new EquipmentItem(_fishItem));

                _textLabel = new TextLabel(rico.Equipment.Items[0].Item.Position.ToVector2(), 
                    100, "x 20", content.Load<SpriteFont>("JingJing"),
                    rico.Equipment.Items[0].Item.Texture);


                // Rico wychodzi w następnej rurze, więc go tam ustawiam
                rico.Position = platforms.LastOrDefault(p => p.platformType == PlatformType.MAGICPIPE).Position;
                rico.Position.Y -= rico.rectangle.Height;
                _canPlayMiniGame = false;
                _playMiniGame = false;
                camera.active = false;
                playerPanel.activeDraw = true;
            }

            if (_playMiniGame)
            {
                _miniGame.Update(gameTime);
            }
            else
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


                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _canPlayMiniGame)
                {
                    _playMiniGame = true;
                }

                // odświeżenie paska gracza
                playerPanel.Update(player);

                int i;
                foreach (Platform platform in platforms)
                {
                    if (platform.platformType == PlatformType.SPIKE)
                    {
                        if (platform.Position.Y <= 663)
                        {
                            platform.Position.Y--;
                        }
                        else if(platform.Position.Y<=(662-(platform.PlatformRectangle.Height+50)))
                        {
                            platform.Position.Y++;
                        }
                    }
                    if (platform.active)
                    {
                        foreach (Penguin penguin in penguins)
                        {
                            // opadanie ryby jest dostępne tylko wtedy gdy Rico ją wypluje
                            if (_fishItem.IsActive)
                            {
                                _fishItem.FallDown();

                                if (_fishItem.IsCollisionDetect(platform))
                                {
                                    _fishItem.CanFallDown = false;
                                }

                            }

                            // sprawdzenie czy pingwin nie stoi na rurze
                            if (platform.platformType == PlatformType.MAGICPIPE &&
                                penguin.penguinType == PenguinType.RICO &&
                                !_miniGame.EndOfGame) // jeśli już gra została zakończona to sorry ;P
                            {
                                if (penguin.Collision(platform.PlatformRectangle))
                                {
                                    // załaduj i pokaż strzałke nad rurą
                                    _chooseItemMenu.Update(platform, new List<Texture2D>() { content.Load<Texture2D>(@"Scena2\arrow_down") }, topMargin: 100);
                                    _chooseItemMenu.IsVisible = true;

                                    // teraz możliwe jest włączenie  minigry
                                    _canPlayMiniGame = true;
                                }
                            }
                            // jeśli jest poza nią to przestań wyświetlać strzałkę MŁ
                            else if (platform.platformType != PlatformType.MAGICPIPE &&
                                     penguin.penguinType == PenguinType.RICO &&
                                     penguin.Collision(platform.PlatformRectangle))
                            {
                                _chooseItemMenu.IsVisible = false;
                                _canPlayMiniGame = false;
                            }

                            // sprawdzenie czy pingwin (RICO) jest w obrębie skrzynki MŁ
                            else if (penguin.penguinType == PenguinType.RICO && _fishBox.IsInActionSector(penguin))
                            {
                                // wyświetlenie menu wyboru ekwipunku
                                if (penguin.Equipment.Items.Count > 0)
                                {
                                    _chooseItemMenu.IsVisible = true;
                                    List<Texture2D> textures = penguin.Equipment.Items.Select(equipmentItem => equipmentItem.Item.Texture).ToList();
                                    _chooseItemMenu.Update(penguin, textures, 40);
                                    penguin.SelectedItem = penguin.Equipment.Items[_chooseItemMenu.SelectedIndex];
                                }
                            }
                            else if (penguin.penguinType == PenguinType.RICO && 
                                     !_fishBox.IsInActionSector(penguin) &&
                                     _miniGame.EndOfGame)
                            {
                                _chooseItemMenu.IsVisible = false;
                            }

                            // sprawdzenie czy Rico wypluł rybę
                            if (_fishItem.IsCollisionDetect(_fishBox))
                            {
                                _fishBox.IsActive = false;
                            }



                            //kolizja z innymi pingwinami
                            for (i = 0; i < penguins.Count; i++)
                            {
                                if (penguins[i].penguinType != penguin.penguinType)
                                    if (penguin.Collision(penguins[i].rectangle, penguins[i].penguinType))
                                        penguin.JumpStop(0);
                            }

                            //Jak kolizja ze sprezyna to wysoki jump
                            if (platform.platformType == PlatformType.SPRING)
                                if (penguin.CollisionRectangle(platform.PlatformRectangle))
                                {
                                    penguin.JumpSpring();
                                }


                            /*    if (platform.platformType == PlatformType.MAGICPIPE)
                                {
                                    if (penguin.Collision(platform.PlatformRectangle, platform.platformType))
                                    {
                                        penguin.Collision(platform.PlatformRectangle, platform.platformType);
                                    }

                                }*/


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



        }

    }

}
