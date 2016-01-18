using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using TestGame.Menu;
using TestGame.Scene;
using Microsoft.Xna.Framework.Media;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Penguin rico;
        public Penguin kowalski;
        public Penguin skipper;
        public Penguin szeregowy;
        public Penguin player;
        private List<Platform> platforms = new List<Platform>();
        private List<TextLabel> playersLabel = new List<TextLabel>();
        private List<Penguin> penguins = new List<Penguin>();
        private float penguinSpeed;
        private float gravitation;
        private TextLabel _textLabel;
        // panel gracza
        private PlayerPanel _playerPanel;
        Camera camera;
        private bool firstStart = true;
        private GameTime gametime;

        private bool _isGamePause;
        private bool _blockESCKey;
        private Texture2D _background;

        private PauseMenu _pauseMenu;

        private GameMenu _gameMenu;
        private bool _canShowGameMenu =  true;

        // flaga informuje 
        private bool _isSceneLoading = true;
        private Vector2 _levelTitlePosition;
        private Vector2 _levelNumberPosition;

        //SCREEN Z AUTORAMI
        private Texture2D _authorsScreen;
        
        //SCREEN ZE STEROWANIEM
        private Texture2D _contorolScreen;

        // SCENA 1
        private Scene1 scene1;
        private Texture2D _scene1TitleTexture;
        private Texture2D _scene1LevelNumberTexture;


        // SCENA 2
        private Scene2 scene2;
        private Texture2D _scene2TitleTexture;
        private Texture2D _scene2LevelNumberTexture;


        // SCENE 3
        private Scene3 scene3;
        private Texture2D _scene3TitleTexture;
        private Texture2D _scene3LevelNumberTexture;

        // zmienne dla obsługi dźwięku
        private bool _blockSoundOn;
        private bool _blockSoundUp;
        private bool _blockSoundDown;

        private Rectangle _soundIconRectangle;
        private Texture2D _soundOnTexture;
        private Texture2D _soundOffTexture;
        private Vector2 _soundIconPosition = Vector2.Zero;

        private CurrentScene _currentScene;
        private bool _canShowControlScreen;
        private bool _canShowAuthorsScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();

            // ustawienie początkowego poziomu na scene 1 MŁ
            _currentScene = CurrentScene.Scene3;
        }

        protected override void Initialize()
        {
            penguinSpeed = 4; //szybkość poruszania się pingwinów
            gravitation = 7f; // wysokość wybicia przy skoku( = 5 ~ 100px)
            camera = new Camera();

            switch (_currentScene)
            {
                case CurrentScene.Scene1:
                    scene1 = new Scene1(Content, camera, gametime);
                    break;
                case CurrentScene.Scene2:
                    scene2 = new Scene2(Content, camera, gametime, GraphicsDevice);
                    break;
                case CurrentScene.Scene3:
                    scene3 = new Scene3(Content, camera, gametime, GraphicsDevice);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            // inicjalizacja panelu gracza - podstawowy gracz - skipper           
            _playerPanel = new PlayerPanel(Content.Load<Texture2D>("panel_background"), new Vector2(0, 0), new Vector2(GraphicsDevice.Viewport.Width, 150), Content.Load<SpriteFont>("JingJing"), Content.Load<Texture2D>("WyborPostaci/Skipper"));

            // inicjalizacja menu pauzy
            _pauseMenu = new PauseMenu(GraphicsDevice, Content.Load<Texture2D>(@"MenuPauzy\JedzILow"), Content.Load<Texture2D>(@"MenuPauzy\Poziom2"), Content.Load<Texture2D>("Wskaznik"), Content.Load<Texture2D>(@"MenuPauzy\bigBackground"), Content.Load<Texture2D>(@"MenuPauzy\resumeText"), Content.Load<Texture2D>(@"MenuPauzy\exitText"), Content.Load<Texture2D>(@"MenuPauzy\tryAgainText"));


            // inicjalizacja menu głównego
            _gameMenu = new GameMenu(this.Content, GraphicsDevice);

            // grafiki obsługi dźwięku
            _soundOnTexture = Content.Load<Texture2D>(@"Audio\Graph\soundOn");
            _soundOffTexture = Content.Load<Texture2D>(@"Audio\Graph\soundOff");
            _soundIconRectangle = new Rectangle(0, 0, _soundOnTexture.Width, _soundOnTexture.Height);

            _soundIconPosition.X = GraphicsDevice.Viewport.Width - _soundIconRectangle.Width/2 - 10;
            _soundIconPosition.Y = 10;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // załadowanie grafik dla screenów
            _authorsScreen = Content.Load<Texture2D>("MenuAuthorsScreen");
            _contorolScreen = Content.Load<Texture2D>("MenuControlScreen");

            switch (_currentScene)
            {
                case CurrentScene.Scene1:
                    _scene1TitleTexture = Content.Load<Texture2D>(@"MenuPauzy\Scene1Title");
                    _scene1LevelNumberTexture = Content.Load<Texture2D>(@"MenuPauzy\Scene1Number");
                    _background = Content.Load<Texture2D>("scene1_background");
                    break;
                case CurrentScene.Scene2:
                    _scene2TitleTexture = Content.Load<Texture2D>(@"MenuPauzy\JedzILow");
                    _scene2LevelNumberTexture = Content.Load<Texture2D>(@"MenuPauzy\Poziom2");
                    _background = Content.Load<Texture2D>("scene2_background");
                    break;
                case CurrentScene.Scene3:
                    _scene3TitleTexture = Content.Load<Texture2D>(@"MenuPauzy\Scene3Title");
                    _scene3LevelNumberTexture = Content.Load<Texture2D>(@"MenuPauzy\Scene3Number");
                    _background = Content.Load<Texture2D>("scene3_background"); 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            // ustawienie tytułu i numeru poziomu
            float screenWidth = GraphicsDevice.Viewport.Width;
            float screenHeight = GraphicsDevice.Viewport.Height;


            // zabezpiedczenie przed dublowaniem pingwinów na liście
            // gdy przechodzę do następnego poziomu to wywoływany jest LoadContent()
            // wtedy zawsze dodaje 2 razy pingwiny
            if (firstStart)
            {
                rico = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/RicoAnimacja_poprawiony"),
                    Content.Load<Texture2D>("Postacie/Animacje/RicoPlywa"), //Ł.G: tymczasowo zmienione 
                    Content.Load<Texture2D>("WyborPostaci/Rico"), new Vector2(-2980, 400),
                    penguinSpeed, gravitation, PenguinType.RICO, Const.RICO_MASS, new Point(480, 815));
                //Ł.G : dodanie rozmiaru frame do Animacji

                // dźwięki wydawane przez Rico
                rico.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\rico_start"));
                rico.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\rico1"));
                rico.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\rico2"));
                // specjalny dźwięk
                rico.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\rico_rzygorzut"));
                skipper = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/SkipperAnimacja"),
                    Content.Load<Texture2D>("Postacie/Animacje/SkipperSlizg"),
                    Content.Load<Texture2D>("WyborPostaci/Skipper"), new Vector2(-3080, 400),
                    penguinSpeed, gravitation, PenguinType.SKIPPER, Const.SKIPPER_MASS, new Point(422, 663));

                // dźwięki wydawane przez skippera
                skipper.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\skipper_start"));
                skipper.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\skipper1"));
                skipper.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\skipper2"));
                skipper.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\skipper3"));
                skipper.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\skipper4"));

                szeregowy = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/SzeregowySheet"),
                    Content.Load<Texture2D>("Postacie/Animacje/SzeregowySlizg"),
                    Content.Load<Texture2D>("WyborPostaci/Szeregowy"), new Vector2(-2930, 400),
                    penguinSpeed, gravitation, PenguinType.SZEREGOWY, Const.SZEREGOWY_MASS, new Point(352, 635));

                // dźwięki wydawane przez szeregowego
                szeregowy.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\szeregowy1"));
                szeregowy.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\szeregowy2"));
                szeregowy.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\szeregowy3"));


                kowalski = new Penguin(Content.Load<Texture2D>("Postacie/Animacje/KowalskiAnimacja"),
                    Content.Load<Texture2D>("Postacie/Animacje/KowalskiPlywanie"),
                    Content.Load<Texture2D>("WyborPostaci/Kowalski"), new Vector2(-3030, 400),
                    penguinSpeed, gravitation, PenguinType.KOWALSKI, Const.KOWALSKI_MASS, new Point(412, 882));

                // dźwięki wydawane przez Kowalskiego
                kowalski.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\kowalski1"));
                kowalski.Voices.Add(Content.Load<SoundEffect>(@"Audio\Waves\kowalski2"));


                // SoundManager.SoundOn = false;

                penguins.Add(skipper);
                penguins.Add(kowalski);
                penguins.Add(rico);
                penguins.Add(szeregowy);
            }
            //Podstawowy gracz - skipper
            player = ActiveAndDeactivationPlayer(true, false, false, false);
           

            switch (_currentScene)
            {
                case CurrentScene.Scene1:
                    _levelNumberPosition = new Vector2((screenWidth / 2 - _scene1LevelNumberTexture.Width / 2), screenHeight / 2 - 100);
                    _levelTitlePosition = new Vector2((screenWidth / 2 - _scene1TitleTexture.Width / 2) + 5, screenHeight / 2 + 20);
                    setStartPosition(1);
                    scene1.LoadContent(penguins, _playerPanel, player);
                    break;
                case CurrentScene.Scene2:
                    _levelNumberPosition = new Vector2((screenWidth / 2 - _scene2LevelNumberTexture.Width / 2), screenHeight / 2 - 100);
                    _levelTitlePosition = new Vector2((screenWidth / 2 - _scene2TitleTexture.Width / 2) + 5, screenHeight / 2 + 20);
                    setStartPosition(2);
                    scene2.LoadContent(penguins, _playerPanel, player);
                    break;
                case CurrentScene.Scene3:
                    _levelNumberPosition = new Vector2((screenWidth / 2 - _scene3LevelNumberTexture.Width / 2), screenHeight / 2 - 100);
                    _levelTitlePosition = new Vector2((screenWidth / 2 - _scene3TitleTexture.Width / 2) + 5, screenHeight / 2 + 20);
                    setStartPosition(3);
                    scene3.LoadContent(penguins, _playerPanel, player);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void setStartPosition(int numberScene)
        {
            switch (numberScene)
            {
                case 1:
                    foreach (Penguin penguin in penguins)
                    {
                        if (penguin.penguinType == PenguinType.SKIPPER) penguin.UpdateStartPosition(new Vector2(-1080, 400));
                        if (penguin.penguinType == PenguinType.KOWALSKI) penguin.UpdateStartPosition(new Vector2(-1030, 400));
                        if (penguin.penguinType == PenguinType.RICO) penguin.UpdateStartPosition(new Vector2(-980, 400));
                        if (penguin.penguinType == PenguinType.SZEREGOWY) penguin.UpdateStartPosition(new Vector2(-930, 400));
                    }
                    //scene1.ResetScene();
                        break;
                case 2:
                    foreach (Penguin penguin in penguins)
                    {
                        if (penguin.penguinType == PenguinType.SKIPPER) penguin.UpdateStartPosition(new Vector2(-3080, 400));
                        if (penguin.penguinType == PenguinType.KOWALSKI) penguin.UpdateStartPosition(new Vector2(-3030, 400));
                        if (penguin.penguinType == PenguinType.RICO) penguin.UpdateStartPosition(new Vector2(-2980, 400));
                        if (penguin.penguinType == PenguinType.SZEREGOWY) penguin.UpdateStartPosition(new Vector2(-2930, 400));
                    }
                    scene2.ResetScene();
                    break;
                case 3:
                    foreach (Penguin penguin in penguins)
                    {
                        if (penguin.penguinType == PenguinType.SKIPPER) penguin.UpdateStartPosition(new Vector2(10, 400));
                        if (penguin.penguinType == PenguinType.KOWALSKI) penguin.UpdateStartPosition(new Vector2(60, 400));
                        if (penguin.penguinType == PenguinType.RICO) penguin.UpdateStartPosition(new Vector2(110, 400));
                        if (penguin.penguinType == PenguinType.SZEREGOWY) penguin.UpdateStartPosition(new Vector2(160, 400));
                    }
                    //scene3.ResetScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (_canShowGameMenu)
            {
                _gameMenu.Update();
                
                // sprawdzenie jaką opcję wybrał użytkownik
                switch (_gameMenu.SelectedOptionMenu)
                {
                    case SelectedOptionMenu.NewGame:
                        _canShowGameMenu = false;
                        break;
                    case SelectedOptionMenu.Control:
                        _canShowControlScreen = true;
                        _gameMenu.SelectedOptionMenu = SelectedOptionMenu.None;
                        break;
                    case SelectedOptionMenu.Authors:
                        _canShowAuthorsScreen = true;
                        _gameMenu.SelectedOptionMenu = SelectedOptionMenu.None;
                        break;
                    case SelectedOptionMenu.Exit:
                        Exit();
                        break;
                    default:
                        
                        break;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !_blockESCKey)
                {
                    _blockESCKey = true;
                    _canShowControlScreen = false;
                    _canShowAuthorsScreen = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                    _blockESCKey = false;

            }
            else
            {
                // gra została juz wczytana
                firstStart = false;

                #region SPRAWDZENIE, CZY KONIEC GRY

                if (_currentScene == CurrentScene.Scene1)
                {
                    if (scene1.IsGameOver)
                    {
                        _isSceneLoading = true;
                    }
                }

                else if (_currentScene == CurrentScene.Scene2)
                {
                    if (scene2.IsGameOver)
                    {
                        _canShowGameMenu = true;
                        _isGamePause = false;
                        _gameMenu.SelectedOptionMenu = SelectedOptionMenu.None;
                        _pauseMenu.SelectedOptionPauseMenu = SelectedOptionPauseMenu.None;
                        setStartPosition((int)_currentScene);
                        _isSceneLoading = true;
                        scene2.IsGameOver = false;
                    }
                }

                #endregion

                #region SPRAWDZENIE KTÓRY ZAŁADOWAĆ POZIOM

                if (_currentScene == CurrentScene.Scene1)
                {
                    if (scene1.IsCompleted)
                    {
                        _currentScene = CurrentScene.Scene2;
                        scene2 = new Scene2(Content, camera, gametime, GraphicsDevice);
                        LoadContent();
                        _isSceneLoading = true;
                    }
                }

                else if (_currentScene == CurrentScene.Scene2)
                {
                    if (scene2.IsCompleted)
                    {
                        _currentScene = CurrentScene.Scene3;
                        scene3 = new Scene3(Content, camera, gametime, GraphicsDevice);
                        LoadContent();
                        _isSceneLoading = true;
                    }
                }

                else if (_currentScene == CurrentScene.Scene3)
                {
                    //if (scene3.IsCompleted)
                    //{
                    //   /// JESTEŚ ZAJEBISTY WYGRAŁEŚ :)
                    //}
                }

                #endregion

                // wyłączenie ekranu ładowania MŁ
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && _isSceneLoading)
                {
                    _isSceneLoading = false;
                }


                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !_blockESCKey)
                {
                    _blockESCKey = true;
                    _isGamePause = !_isGamePause;
                    _pauseMenu.ShowMenu = true;
                }

                #region DŹWIĘK

                MediaPlayer.Volume = SoundManager.Volume;

                if (Keyboard.GetState().IsKeyDown(Keys.Q) && !_blockSoundOn)
                {
                    SoundManager.SoundOn = !SoundManager.SoundOn;

                    _blockSoundOn = true;

                    // odtworzenie dźwięku tła jeśli jest to włączone
                    if (SoundManager.SoundOn)
                    {
                        SoundManager.Volume = 1.0f;
                        MediaPlayer.Resume();
                    }
                    else
                    {
                        SoundManager.Volume = 0.0f;
                        MediaPlayer.Pause();
                    }
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Q))
                    _blockSoundOn = false;

                #endregion

                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                {
                    _blockESCKey = false;
                }

                if (!_isGamePause)
                {
                    switch (_currentScene)
                    {
                        case CurrentScene.Scene1:
                            scene1.UpdatePosition(gameTime);
                            break;
                        case CurrentScene.Scene2:
                            scene2.UpdatePosition(gameTime);
                            break;
                        case CurrentScene.Scene3:
                            scene3.UpdatePosition(gameTime);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    _pauseMenu.Update();

                    switch (_pauseMenu.SelectedOptionPauseMenu)
                    {
                        case SelectedOptionPauseMenu.Resume:
                            _isGamePause = false;
                            _pauseMenu.SelectedOptionPauseMenu = SelectedOptionPauseMenu.None;
                            break;
                        case SelectedOptionPauseMenu.TryAgain:
                            setStartPosition((int)_currentScene);
                            _isSceneLoading = true;
                            _isGamePause = false;
                            _pauseMenu.SelectedOptionPauseMenu = SelectedOptionPauseMenu.None;
                            break;
                        case SelectedOptionPauseMenu.BackToMenu:
                            _currentScene = CurrentScene.Scene1;
                            setStartPosition(1);
                            _isSceneLoading = true;
                            _isGamePause = false;
                            _canShowGameMenu = true;
                            _pauseMenu.SelectedOptionPauseMenu = SelectedOptionPauseMenu.None;
                            break;
                        default:
                            break;
                    }              
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gametime = gameTime;

            if (_canShowGameMenu)
            {
                if (_canShowAuthorsScreen)
                {
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();
                    spriteBatch.Draw(_authorsScreen, Vector2.Zero, Color.White);
                    spriteBatch.End();
                }
                else if (_canShowControlScreen)
                {
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();
                    spriteBatch.Draw(_contorolScreen, Vector2.Zero, Color.White);
                    spriteBatch.End();
                }
                else
                {
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();
                    _gameMenu.Draw(spriteBatch);
                    spriteBatch.End();
                }


            }
            else
            {
                switch (_currentScene)
                {
                    case CurrentScene.Scene1:
                        GraphicsDevice.Clear(Color.Indigo);
                        break;
                    case CurrentScene.Scene2:
                        GraphicsDevice.Clear(Color.LimeGreen);
                        break;
                    case CurrentScene.Scene3:
                        GraphicsDevice.Clear(Color.MediumVioletRed);
                        break;
                    default:
                        GraphicsDevice.Clear(Color.CornflowerBlue);
                        break;
                }
                // wyświetlenie screenu początkowego z nazwą oraz tytułem poziomu
                if (_isSceneLoading)
                {
                    spriteBatch.Begin();

                    switch (_currentScene)
                    {
                        case CurrentScene.Scene1:
                            spriteBatch.Draw(_scene1TitleTexture, _levelTitlePosition, Color.White);
                            spriteBatch.Draw(_scene1LevelNumberTexture, _levelNumberPosition, Color.White);
                            break;
                        case CurrentScene.Scene2:
                            spriteBatch.Draw(_scene2TitleTexture, _levelTitlePosition, Color.White);
                            spriteBatch.Draw(_scene2LevelNumberTexture, _levelNumberPosition, Color.White);
                            break;
                        case CurrentScene.Scene3:
                            spriteBatch.Draw(_scene3TitleTexture, _levelTitlePosition, Color.White);
                            spriteBatch.Draw(_scene3LevelNumberTexture, _levelNumberPosition, Color.White);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    spriteBatch.End();
                }
                else
                {
                    if (!_isGamePause)
                    {
                        // narysowanie panelu użytkownika
                        spriteBatch.Begin();
                        spriteBatch.Draw(_background, new Rectangle(new Point(0, 0), new Point(1200, 900)), Color.White);
                        _playerPanel.Draw(spriteBatch);
                        spriteBatch.End();

                        // narysowanie aktualnej sceny
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

                        switch (_currentScene)
                        {
                            case CurrentScene.Scene1:
                                scene1.Draw(spriteBatch);
                                break;
                            case CurrentScene.Scene2:
                                scene2.Draw(spriteBatch);
                                break;
                            case CurrentScene.Scene3:
                                scene3.Draw(spriteBatch);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        spriteBatch.End();
                    }
                    else // MENU PAUZY
                    {
                        _pauseMenu.Draw(spriteBatch);
                    }

                    #region IKONKA DŹWIĘKU

                    // rysowana jest na końcu po to aby była zawsze na wierzchu
                    spriteBatch.Begin();

                    if (SoundManager.SoundOn)
                        spriteBatch.Draw(_soundOnTexture, _soundIconPosition, _soundIconRectangle, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                    else
                        spriteBatch.Draw(_soundOffTexture, _soundIconPosition, _soundIconRectangle, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                    spriteBatch.End();

                    #endregion
                }
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Metoda aktywuje i dezaktuwuje graczy
        /// </summary>
        /// <returns>Zwracam referencje do aktywnego gracza</returns>
        private Penguin ActiveAndDeactivationPlayer(bool ConSkipper, bool ConKowalski, bool ConRico, bool ConSzeregowy)
        {
            skipper.active = ConSkipper;
            kowalski.active = ConKowalski;
            rico.active = ConRico;
            szeregowy.active = ConSzeregowy;

            if (ConSkipper) return skipper;
            if (ConKowalski) return kowalski;
            if (ConRico) return rico;
            if (ConSzeregowy) return szeregowy;

            return skipper;
        }
    }
}