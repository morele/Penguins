using System.Collections.Generic;
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
        public TextLabel Text { get; set; }

        private Equipment _panelEquipment;

        private Texture2D _panelTexture;
        private readonly Rectangle _panelRectangle;

        public PlayerPanel(Texture2D Image, Vector2 position, Vector2 size, SpriteFont font, Texture2D defalutAvatar) : base(Image, position)
        {
            var screenWidth = (int)size.X;
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, screenWidth, Const.PLAYER_PANEL_HEIGHT);

            Text = new TextLabel(new Vector2(10,15), 50, string.Empty, font, defalutAvatar);
            Text.alignment = TextLabel.Alignment.Right;
            _panelEquipment = new Equipment();
        }

        public PlayerPanel(Texture2D Image, Vector2 position) : base(Image, position)
        {
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, 1000, Const.PLAYER_PANEL_HEIGHT);
            _panelEquipment = new Equipment();
        }

        public PlayerPanel(Texture2D Image, Vector2 position, float speedValue, float gravitation) : base(Image, position, speedValue, gravitation)
        {
            _panelTexture = Image;
            _panelRectangle = new Rectangle((int)position.X, (int)position.Y, 1000, Const.PLAYER_PANEL_HEIGHT);
            _panelEquipment = new Equipment();
        }

        /// <summary>
        /// Metoda wyświetla avatar aktywnego pingwina oraz jego nazwę
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch do rysowania</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, _panelRectangle, Color.White);
            Text.Draw(spriteBatch,true);

            // wyświetlenie ekwipunku
            foreach (var item in _panelEquipment.Items)
            {
                spriteBatch.Draw(item.Item.Texture, new Rectangle(item.Item.Position, item.Item.Size), Color.White);
            }
        }

        /// <summary>
        /// Metoda aktualizuje pasek gracza
        /// </summary>
        /// <param name="currentPenguin">Aktywny pingwin</param>
        public void Update(Penguin currentPenguin)
        {
            // ustawiam avatar aktywnego pingwina
            Text.Background = currentPenguin.Avatar;
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

            // aktualizacja ekwipunku
            _panelEquipment = currentPenguin.Equipment;
        }

    }
}
