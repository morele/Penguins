﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    /// <summary>
    /// Klasa reprezentuje panel gracza wyświetlany u góry ekranu.
    /// Panel będzie zawierał avatar akutalnie wybranego pingwina
    /// oraz jego ekwipunek wraz z zebranymi bonusami.
    /// </summary>
    public class PlayerPanel : TextureManager
    {
        public Texture2D Avatar { get; private set; }

        public TextLabel Text { get; private set; }
        
        private Texture2D _panelTexture;
        private Rectangle _panelRectangle;

        public PlayerPanel(Texture2D Image, Vector2 position, Vector2 size, SpriteFont font, Texture2D defalutAvatar) : base(Image, position)
        {
            var panelWidth = (int)size.X;
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, panelWidth, 150);

            Text = new TextLabel(new Vector2(130, 60), 20, string.Empty, font, defalutAvatar);
            Text.alignment = TextLabel.Alignment.Right;
        }

        public PlayerPanel(Texture2D Image, Vector2 position) : base(Image, position)
        {
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, 1000, 150);
        }

        public PlayerPanel(Texture2D Image, Vector2 position, float speedValue, float gravitation) : base(Image, position, speedValue, gravitation)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, _panelRectangle, Color.White);
            spriteBatch.Draw(Avatar, new Vector2(10,15), new Rectangle(0,0,224,224), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            Text.Draw(spriteBatch);
        }

        public void Update(Penguin currentPenguin)
        {
            switch (currentPenguin.penguinType)
            {
                case PenguinType.RICO:
                    Text.Text = "RICO";
                    break;
                case PenguinType.SKIPPER:
                    Text.Text = "SKIPPER";
                    break;
                case PenguinType.KOWALSKI:
                    Text.Text = "KOWALSKI";
                    break;
                case PenguinType.SZEREGOWY:
                    Text.Text = "SZEREGOWY";
                    break;
            }
        }

    }
}
