using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame.Scene
{
    abstract public class Scene
    {
        protected ContentManager content;
        protected List<Penguin> penguins;
        protected Penguin player;
        protected List<Platform> platforms = new List<Platform>();
        protected Camera camera;
        protected PlayerPanel playerPanel;
        protected bool firstStart = true;

        public Scene(ContentManager content, Camera camera)
        {
            this.content = content;
            this.camera = camera;
        }
        public virtual void LoadContent(List<Penguin> penguins, PlayerPanel _playerPanel, Penguin player)
        {
            this.player = player;
            this.penguins = penguins;
            this.playerPanel = _playerPanel;

            //Podstawowy gracz - skipper
            player = ActiveAndDeactivationPlayer(true, false, false, false);

        }

        public abstract void UpdatePosition(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        protected Penguin ActiveAndDeactivationPlayer(bool ConSkipper, bool ConKowalski, bool ConRico, bool ConSzeregowy)
        {
            penguins[0].active = ConSkipper;
            penguins[1].active = ConKowalski;
            penguins[2].active = ConRico;
            penguins[3].active = ConSzeregowy;

            if (ConSkipper)
            {
                penguins[0].blockDircetionLEFT = penguins[0].blockDirectionRIGHT = false;
                return penguins[0];//skipper
            }

            if (ConKowalski)
            {
                penguins[1].blockDircetionLEFT = penguins[1].blockDirectionRIGHT = false;
                return penguins[1];//kowalski
            }
            
            if (ConRico)
            {
                penguins[2].blockDircetionLEFT = penguins[2].blockDirectionRIGHT = false;
                return penguins[2];//rico
            }
            
            if (ConSzeregowy)
            {
                penguins[3].blockDircetionLEFT = penguins[3].blockDirectionRIGHT = false;
                return penguins[3];//szeregowy
            }
            

            return penguins[0];
        }
        protected void FirstStart()
        {
            while (firstStart)
            {
                foreach (Penguin penguin in penguins)
                    penguin.UpdatePosition();


                foreach (Platform platform in platforms)
                    foreach (Penguin penguin in penguins)
                        if (IsOnTopOf(penguin, platform)) penguin.firstStart = false;

                if (!penguins[0].firstStart && !penguins[1].firstStart && !penguins[2].firstStart && !penguins[3].firstStart)
                {
                    firstStart = false;
                }
            }
        }
        protected bool IsOnTopOf(Penguin player, Platform platform)
        {
            if (player.Collision(platform.PlatformRectangle))
            {
                player.speed.Y = 0f;
                player.jump = false;
                return true;
            }
            return false;
        }
    }
}
