﻿using Microsoft.Xna.Framework;
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

        public abstract void UpdatePosition();

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Platform platform in platforms)
                platform.Draw(spriteBatch);

            foreach (Penguin penguin in penguins)
                penguin.Draw(spriteBatch);

        }
        protected Penguin ActiveAndDeactivationPlayer(bool ConSkipper, bool ConKowalski, bool ConRico, bool ConSzeregowy)
        {
            penguins[0].active = ConSkipper;
            penguins[1].active = ConKowalski;
            penguins[2].active = ConRico;
            penguins[3].active = ConSzeregowy;

            if (ConSkipper) return penguins[0];//skipper
            if (ConKowalski) return penguins[1];//kowalski
            if (ConRico) return penguins[2];//rico
            if (ConSzeregowy) return penguins[3];//szeregowy

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
            if (player.IsOnTopOf(platform))
            {
                player.speed.Y = 0f;
                player.jump = false;
                return true;
            }
            return false;
        }
    }
}
