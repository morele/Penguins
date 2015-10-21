using System;
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

        public TextLabel Text { get; }
        
        private Texture2D _panelTexture;
        private readonly Rectangle _panelRectangle;

        public PlayerPanel(Texture2D Image, Vector2 position, Vector2 size, SpriteFont font, Texture2D defalutAvatar) : base(Image, position)
        {
            var screenWidth = (int)size.X;
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, screenWidth, Const.PLAYER_PANEL_HEIGHT);

            Text = new TextLabel(Vector2.Zero, 20, string.Empty, font, defalutAvatar);
            Text.alignment = TextLabel.Alignment.Right;
        }

        public PlayerPanel(Texture2D Image, Vector2 position) : base(Image, position)
        {
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, 1000, Const.PLAYER_PANEL_HEIGHT);
        }

        public PlayerPanel(Texture2D Image, Vector2 position, float speedValue, float gravitation) : base(Image, position, speedValue, gravitation)
        {
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, 1000, Const.PLAYER_PANEL_HEIGHT);
        }

        /// <summary>
        /// Metoda wyświetla avatar aktywnego pingwina oraz jego nazwę
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch do rysowania</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 avatarMargin = new Vector2(10, 15);
            spriteBatch.Draw(Image, _panelRectangle, Color.White);
            spriteBatch.Draw(Avatar, avatarMargin, new Rectangle(new Point(0), new Point(Const.PENGUIN_AVATAR_SIZE)), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            Text.Draw(spriteBatch);
        }

        /// <summary>
        /// Metoda aktualizuje pasek gracza
        /// </summary>
        /// <param name="currentPenguin">Aktywny pingwin</param>
        public void Update(Penguin currentPenguin)
        {
            // ustawiam avatar aktywnego pingwina
            Avatar = currentPenguin.Avatar;

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
