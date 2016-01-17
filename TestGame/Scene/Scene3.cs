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
        // muzyka w tle
        private Song _themeSong;

        bool _blockD1 = false;
        bool _blockD2 = false;
        bool _blockD3 = false;
        bool _blockD4 = false;

        // menu wyboru ekwipunku
        private ChooseItemMenu _chooseItemMenu;
        private TextLabel _textLabel;


        private GameTime gametime;

        public Scene3(ContentManager content, Camera camera, GameTime gametime, GraphicsDevice device) : base(content, camera, gametime)
        {
            _chooseItemMenu = new ChooseItemMenu();
            _chooseItemMenu.IsVisible = false;


        }

        public override void LoadContent(List<Penguin> penguins, PlayerPanel playerPanel, Penguin player)
        {
            base.LoadContent(penguins, playerPanel, player);

            platforms.Add(new Platform(content.Load<Texture2D>("Scena1/podloga"), new Vector2(-700, 700)));

            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/0"), new Vector2(1254, 263)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/1"), new Vector2(1477, 392)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/2"), new Vector2(1287, 784)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/3"), new Vector2(431, 424)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/4"), new Vector2(2244, 732)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/5"), new Vector2(1700, 482)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/6"), new Vector2(313, 637)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/7"), new Vector2(1723, 658)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/8"), new Vector2(2548, 265)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/9"), new Vector2(877, 681)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/10"), new Vector2(877, 714)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/2/11"), new Vector2(2755, 494)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/12"), new Vector2(1335, 681)));
            platforms.Add(new Platform(content.Load<Texture2D>("Scena3/1/13"), new Vector2(2742, 385)));



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
                    if (platform.platformType == PlatformType.SPIKE)
                    {
                   
                    }
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

                            //...






                            //...
                            
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
