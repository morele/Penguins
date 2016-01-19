using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Testgame.MIniGames.Swiming;
using TestGame.Menu;
using TestGame.MIniGames.Numbers;
using TestGame.Interfaces;
using TestGame.MIniGames.Memory;

namespace TestGame.Scene
{
    public class Scene2 : Scene
    {
        // muzyka w tle
        private Song _themeSong;


        private List<SoundEffect> _sceneSounds = new List<SoundEffect>();
        private List<bool> _isSceneSoundPlay = new List<bool>(2);
        bool _blockD1 = false;
        bool _blockD2 = false;
        bool _blockD3 = false;
        bool _blockD4 = false;

        // menu wyboru ekwipunku
        private ChooseItemMenu _chooseItemMenu;

        // minigra - "Swiming"
        private bool _playMiniGame;
        private Swiming _miniGame;
        private bool _canPlayMiniGame;

        private bool _isCollisionWithJulian = true;

        // czy to czas na Kowalskiego
        private bool _isKowalskiTime;
        private bool _canDoItKowalski;
        // minigra - "Memory"
        public Memory _minigameMemory;
        private bool _canPlayMiniGameMemory;
        private bool _playMiniGameMemory;



        // rybka zebrana przez Rico
        private Bonus _fishItem;

        // skrzynka do której Rico ma wrzucić rybki
        private ActionElement _fishBox;
        //LG: akcja na juliana
        private ActionElement _julek;

        private bool _firstStart = true;

        private ContentManager _content;

        private GameTime gametime;
        private bool ActiveCar = false;
        private int indexOfCar;
        // bool fisrtStartCarTEMPORARY = true;
        public Scene2(ContentManager content, Camera camera, GameTime gametime, GraphicsDevice device) : base(content, camera, gametime)
        {
            _chooseItemMenu = new ChooseItemMenu();
            _chooseItemMenu.IsVisible = false;

            _miniGame = new Swiming(new SpriteBatch(device), content, device);
            _minigameMemory = new Memory(new SpriteBatch(device), content, device);

            _content = content;
        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            // załadowanie dźwięków sceny
            _sceneSounds.Add(content.Load<SoundEffect>(@"Audio\Waves\skipper_przykroMiLemurze"));
            _sceneSounds.Add(content.Load<SoundEffect>(@"Audio\Waves\julian3"));
            _sceneSounds.Add(content.Load<SoundEffect>(@"Audio\Waves\julian1"));

            _isSceneSoundPlay.Add(false);
            _isSceneSoundPlay.Add(false);
            _isSceneSoundPlay.Add(false);

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



            Texture2D Julek = content.Load<Texture2D>("Postacie/Julek/JulianSpriteMachanie");
            Vector2 tempPositionOfJulian = new Vector2(-2500, YpositionFloor - (Julek.Height / 4));

            _julek = new ActionElement(new Animation(Julek, 4, 120, tempPositionOfJulian), tempPositionOfJulian.ToPoint(), 5);


            //platformy
            platforms.Add(new Platform(platfroma2, new Vector2(-3100, YpositionFloor)));
            platforms.Add(new Platform(platfroma2, new Vector2(-2100, YpositionFloor)));
            platforms.Add(new Platform(platfroma2, new Vector2(-1100, YpositionFloor)));
            platforms.Add(new Platform(platfroma2, new Vector2(-100, YpositionFloor)));
            platforms.Add(new Platform(platforma1, new Vector2(1221, YpositionFloor)));

            //woda i ograniczenia
            platforms.Add(new Platform(mur, new Vector2(-3100, YpositionFloor + platfroma2.Height - 2)));

            for (int i = 1; i <= 30; i++)
            {
                platforms.Add(new Platform(new Animation(content.Load<Texture2D>("Scena2/woda/WodaAnimacja"), 3, 120,
                        new Vector2(-3100 + mur.Width * i, YpositionFloor + platfroma2.Height + 30)), false, 0, 0, PlatformType.WATER));
            }
;
            platforms.Add(new Platform(mur, new Vector2(-1100 + mur.Width + woda.Width, YpositionFloor + platfroma2.Height - 2)));
            platforms.Add(new Platform(rura, new Vector2(-2700, YpositionFloor - rura.Height), false, 0, 0, PlatformType.MAGICPIPE));

            //platforms.Add(new Platform(new Animation(content.Load<Texture2D>("Postacie/Animacje/RicoAnimacja_poprawiony"), 8, 50,
            //    new Vector2(-2500, YpositionFloor - content.Load<Texture2D>("Postacie/Animacje/RicoAnimacja_poprawiony").Height))));

            platforms.Add(new Platform(new Animation(content.Load<Texture2D>("Scena2/autko/AutkoAnimacja"), 6, 50,
               new Vector2(-2300, YpositionFloor - content.Load<Texture2D>("Scena2/autko/Autko1").Height)), false, 0f, 0f, PlatformType.CAR));

            platforms.Add(new Platform(content.Load<Texture2D>("Scena2/Kaluza"), new Vector2(-1500, YpositionFloor), false, 0, 0, PlatformType.CAR));



            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(-400, YpositionFloor + 4)), true, 1, kolec.Height, PlatformType.SPIKEFIRST));
            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(-440, YpositionFloor + 4)), true, 1, kolec.Height, PlatformType.SPIKE));
            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(-480, YpositionFloor + 4)), true, 1, kolec.Height, PlatformType.SPIKE));
            platforms.Add(new Platform(new Animation(kolec, 3, 50, new Vector2(-520, YpositionFloor + 4)), true, 1, kolec.Height, PlatformType.SPIKELAST));

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
            _themeSong = content.Load<Song>("Audio/Waves/scene2_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = SoundManager.Volume;

            // przypisanie zdarzenia dla pingwna, który spadł z platformy
            foreach (var penguin in penguins)
            {
                penguin.DeathLine = 900;
                penguin.PenguinDeathByFallingHandler += Penguin_PenguinDeathByFallingHandler;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // narysowanie rybki
            _fishItem.Draw(spriteBatch);

            if (_playMiniGame)
            {
                _miniGame.Draw();
            }
            else if (_playMiniGameMemory)
            {
                _minigameMemory.Draw();
            }
            else
            {
                // narysowanie Juliana
                _julek.Draw(spriteBatch);

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
            if (_firstStart)
            {
                if (SoundManager.SoundOn)
                {
                    MediaPlayer.Play(_themeSong);
                }
                _firstStart = false;
            }


            #region Zakończenie minigry "Swimming"

            // to się wykona tylko raz po zakończeniu minigry "Swiming"
            if (_miniGame.EndOfGame && _canPlayMiniGame)
            {
                // dodanie ryb do ekwipunku Rico
                var rico = penguins.FirstOrDefault(p => p.penguinType == PenguinType.RICO);
                rico.Equipment.AddItem(new EquipmentItem(_fishItem));

                _fishItem.OnChecked();

                // Rico wychodzi w następnej rurze, więc go tam ustawiam
                rico.Position = platforms.LastOrDefault(p => p.platformType == PlatformType.MAGICPIPE).Position;
                rico.Position.Y -= rico.rectangle.Height - 20;
                _canPlayMiniGame = false;
                _playMiniGame = false;
                camera.active = false;
                playerPanel.activeDraw = true;
            }

            #endregion

            #region Zakończenie minigry "Memory"

            // to się wykona tylko raz po zakończeniu minigry "Memory"
            if (_minigameMemory.EndOfGame && _canPlayMiniGameMemory)
            {
                _canPlayMiniGameMemory = false;
                _playMiniGameMemory = false;

                // odtworzenie dźwięku "radosnego" Skippera
                var sound = content.Load<SoundEffect>(@"Audio\Waves\skipper_przykroMiLemurze");
                sound.Play();

                _julek.Animation.Texture = _content.Load<Texture2D>("Postacie/Julek/JulianSpriteBateria");

                // Kowalski może odegrać swoją role
                _isKowalskiTime = true;

            }

            #endregion

            if (_playMiniGame)
            {
                _miniGame.Update(gameTime);
            }
            else if (_playMiniGameMemory)
            {
                _minigameMemory.Update(gameTime);
            }
            else
            {
                // metoda ustawia wszystkich graczy na pozycji początkowej
                if (firstStart) FirstStart(gameTime);
                _julek.Update(gameTime);

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






                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (!_miniGame.EndOfGame && _canPlayMiniGame)
                        _playMiniGame = true;

                    else if (!_minigameMemory.EndOfGame && _canPlayMiniGameMemory)
                    {
                        _playMiniGameMemory = true;
                    }
                    else if (_isKowalskiTime && _canDoItKowalski)
                    {
                        _isCollisionWithJulian = false;
                        _julek.Animation.Texture = _content.Load<Texture2D>("Postacie/Julek/JulianSpritePozegnanie");

                        var car = platforms.FirstOrDefault(element => element.platformType == PlatformType.CAR);
                        if (car != null)
                        {
                            indexOfCar = platforms.IndexOf(car);
                            platforms[indexOfCar] = new Platform(new Animation(content.Load<Texture2D>("Scena2/autko/AutkoAnimacja2"), 6, 50,
                                                            new Vector2(-2300, 700 - content.Load<Texture2D>("Scena2/autko/Autko1").Height)), false, 0, 0, PlatformType.CAR);
                            platforms[indexOfCar].ActiveCar = true;

                        }
                        //skipperem prowadzimy - aktywacja gracza
                        player = ActiveAndDeactivationPlayer(true, false, false, false);
                        _blockD1 = true;
                        ActiveCar = true;

                    }

                }

                // odświeżenie paska gracza
                playerPanel.Update(player);

                //jesli aktywne poruszanie sie samochodem
                if (ActiveCar && player.penguinType == PenguinType.SKIPPER)
                {
                    foreach (Penguin penguin in penguins)
                    {
                        if (penguin.penguinType == PenguinType.KOWALSKI)
                        {
                            penguin.ActiveDraw = false;
                            penguin.UpdateStartPosition(platforms[indexOfCar].Animation.PositionStaticItems.Location.ToVector2());
                        }

                        if (penguin.penguinType == PenguinType.SZEREGOWY)
                        {
                            penguin.ActiveDraw = false;
                            penguin.UpdateStartPosition(platforms[indexOfCar].Animation.PositionStaticItems.Location.ToVector2());
                        }

                        if (penguin.penguinType == PenguinType.SKIPPER)
                        {
                            penguin.ActiveDraw = false;
                            penguin.UpdateStartPosition(platforms[indexOfCar].Animation.PositionStaticItems.Location.ToVector2());
                        }

                    }
                    _blockD2 = true;//blokada przelaczenia pingwinow, mozliwy tylko skipper kierowca i rico 
                    _blockD4 = true;


                    foreach (Platform platform in platforms)
                    {
                        //sprawdza czy kolizja pomiedzy rurami i kolcami, jak tak to blokuje kierunki 
                        if (platform.platformType == PlatformType.SPIKEFIRST ||
                            platform.platformType == PlatformType.SPIKE ||
                            platform.platformType == PlatformType.SPIKELAST ||
                            platform.platformType == PlatformType.MAGICPIPE)
                        {
                            platforms[indexOfCar].CollisionCar(platform.PlatformRectangle, platform.platformType);
                        }

                        //sprawdza czy samochod jest nad wysunietymi kolcami
                        if (platform.platformType == PlatformType.SPIKEFIRST ||
                           platform.platformType == PlatformType.SPIKE ||
                           platform.platformType == PlatformType.SPIKELAST)
                        {
                            if (platforms[indexOfCar].CollisionPlatform(new Rectangle(platform.PlatformRectangle.X + 4, platform.PlatformRectangle.Y, platform.PlatformRectangle.Width - 8, platform.PlatformRectangle.Height)))
                            {
                                IsGameOver = true;
                            }
                        }

                        //jak auto dobije do rury wtedy maja wszyscy wysiasc
                        if (platform.platformType == PlatformType.MAGICPIPE && platform.PlatformRectangle.X > 750)
                        {
                            if (platforms[indexOfCar].CollisionPlatform(platform.PlatformRectangle))
                            {
                                ActiveCar = false;
                                platforms[indexOfCar].ActiveCar = false;
                                _blockD2 = false;
                                _blockD4 = false;
                                platforms[indexOfCar].Animation =
                                    new Animation(content.Load<Texture2D>("Scena2/autko/AutkoAnimacja"), 6, 50,
                                        platforms[indexOfCar].Position.ToVector2());
                                platforms[indexOfCar].ActiveCar = false;

                                foreach (Penguin penguin in penguins)
                                {
                                    if (penguin.penguinType == PenguinType.KOWALSKI)
                                    {
                                        penguin.ActiveDraw = true;
                                        penguin.UpdateStartPosition(new Vector2(1220, 500));
                                    }

                                    if (penguin.penguinType == PenguinType.SZEREGOWY)
                                    {
                                        penguin.ActiveDraw = true;
                                        penguin.UpdateStartPosition(new Vector2(1340, 500));
                                    }

                                    if (penguin.penguinType == PenguinType.SKIPPER)
                                    {
                                        penguin.ActiveDraw = true;
                                        penguin.UpdateStartPosition(new Vector2(1060, 500));
                                    }
                                    if (penguin.penguinType == PenguinType.RICO)
                                    {
                                        penguin.ActiveDraw = true;
                                      //  penguin.UpdateStartPosition(new Vector2(1280, 500));
                                    }

                                }
                            }
                        }
                    }


                    foreach (Platform platform in platforms)
                        if (platform.platformType == PlatformType.SPIKEFIRST ||
                            platform.platformType == PlatformType.SPIKE ||
                            platform.platformType == PlatformType.SPIKELAST ||
                            platform.platformType == PlatformType.WATER)
                            platform.UpdatePosition(gameTime);

                    _chooseItemMenu.IsVisible = false;

                    platforms[indexOfCar].UpdateCar(gameTime);
                    camera.Update(platforms[indexOfCar].Animation.PositionStaticItems);

                }
                else
                {
                    int i;
                    foreach (Platform platform in platforms)
                    {
                        if (platform.active)
                        {
                            // opadanie ryby jest dostępne tylko wtedy gdy Rico ją wypluje
                            if (_fishItem.IsActive)
                            {
                                _fishItem.FallDown();

                                if (_fishItem.IsCollisionDetect(platform))
                                    _fishItem.CanFallDown = false;
                            }

                            foreach (Penguin penguin in penguins)
                            {
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
                                         _miniGame.EndOfGame && _canPlayMiniGame)
                                {
                                    _chooseItemMenu.IsVisible = false;
                                }

                                // sprawdzenie czy Rico wypluł rybę
                                if (_fishItem.IsCollisionDetect(_fishBox))
                                {
                                    var pawl = platforms.FirstOrDefault(p => p.platformType == PlatformType.PAWL);
                                    platforms[platforms.IndexOf(pawl)].active = false;
                                    _fishItem.IsActive = false;
                                }


                                // sprawdzenie kolizji między Skipperem a Julianem
                                if (_julek.IsCollisionDetect(penguin) && _isCollisionWithJulian)
                                {
                                    penguin.Position.X -= 2;
                                }

                                // sprawdzenie czy Skipper może rozpocząć grę z Julianem
                                if (penguin.penguinType == PenguinType.SKIPPER &&
                                    _julek.IsInActionSector(penguin) &&
                                    !_minigameMemory.EndOfGame)
                                {
                                    // odtworzenie dźwięku
                                    if (SoundManager.SoundOn)
                                        if (!_isSceneSoundPlay[1])
                                        {
                                            _isSceneSoundPlay[1] = true;
                                            _sceneSounds[1].Play();
                                        }

                                    _chooseItemMenu.Update(penguin, new List<Texture2D>()
                                {
                                    content.Load<Texture2D>(@"Scena2\talkIcon")
                                }, topMargin: 100);

                                    _chooseItemMenu.IsVisible = true;

                                    // teraz możliwe jest włączenie  minigry
                                    _canPlayMiniGameMemory = true;
                                }
                                else if (penguin.penguinType == PenguinType.SKIPPER &&
                                         !_julek.IsInActionSector(penguin))
                                {
                                    _isSceneSoundPlay[1] = false;
                                    _canPlayMiniGameMemory = false;
                                }

                                //kolizja z innymi pingwinami
                                for (i = 0; i < penguins.Count; i++)
                                {
                                    if (penguins[i].penguinType != penguin.penguinType)
                                        if (penguin.Collision(penguins[i].rectangle, penguins[i].penguinType))
                                            penguin.JumpStop(0);
                                }

                                #region AKCJE KOWALSKIEGO

                                if (penguin.penguinType == PenguinType.KOWALSKI &&
                                    _julek.IsInActionSector(penguin) && _isKowalskiTime)
                                {
                                    if (SoundManager.SoundOn)
                                        if (!_isSceneSoundPlay[2])
                                        {
                                            _isSceneSoundPlay[2] = true;
                                            _sceneSounds[2].Play();
                                        }

                                    _chooseItemMenu.Update(penguin, new List<Texture2D>()
                                { content.Load<Texture2D>(@"Scena2\battery") }, topMargin: 100);
                                    _chooseItemMenu.IsVisible = true;
                                    _canDoItKowalski = true;
                                }
                                else if (penguin.penguinType == PenguinType.KOWALSKI &&
                                         !_julek.IsInActionSector(penguin) && _isKowalskiTime)
                                {
                                    _chooseItemMenu.IsVisible = false;
                                    _isSceneSoundPlay[2] = false;
                                    _canDoItKowalski = false;
                                }

                                #endregion

                                //Jak kolizja ze sprezyna to wysoki jump
                                if (platform.platformType == PlatformType.SPRING)
                                    if (penguin.CollisionRectangle(platform.PlatformRectangle))
                                    {
                                        penguin.JumpSpring();
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
                           // if(platform.platformType != PlatformType.CAR)
                            platform.UpdatePosition(gameTime);

                            // sprawdzenie czy poziom został ukończony
                            // jeśli wszystkie pingwiny mają współrzędne większe niż takie 
                            // jak były współrzędne zapadki to poziom został ukończony
                            if (penguins.Count(p => p.Position.X > 1580) == penguins.Count)
                                IsCompleted = true;

                        }

                    }

                    foreach (Penguin penguin in penguins)
                        penguin.UpdatePosition(gameTime);

                    camera.Update(player);
                }
            }
        }

        private void Penguin_PenguinDeathByFallingHandler(object sender, System.EventArgs e)
        {
            IsGameOver = true;
        }

        public override void ResetScene()
        {
            _canPlayMiniGame = false;
            _canPlayMiniGameMemory = false;
            _firstStart = true;
            _playMiniGameMemory = false;
            _playMiniGame = false;
            _miniGame.EndOfGame = false;
            _minigameMemory.EndOfGame = false;
        }
    }

}
