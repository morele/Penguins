using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame.Menu
{
    public class ChooseItemMenu
    {
        public bool IsVisible { get; set; }
        public int SelectedIndex { get; private set; }

        private Texture2D _arrowTexture;
        private Point _currentArrowFrame;
        private Point _arrowFramesSheet;
        private Point _arrowFrameSize;


        private Texture2D _itemTexture;
        private Point _itemPosition;
        private Point _itemSize;
        
        private bool _pull;

        public ChooseItemMenu()
        {
            IsVisible = false;
            SelectedIndex = 0;
        }

        public void Update(GameObject gameObject, List<Texture2D> textures, int topMargin = 600)
        {
            // jeśli lista jest pusta to przerwij akcję
            if (!textures.Any()) return;

            // sprawdzenie czy wskaźnik nie wystaje poza listę
            if (SelectedIndex >= textures.Count)
                SelectedIndex = 0;
            
            _itemTexture = textures[SelectedIndex];
            _itemSize = new Point(textures[SelectedIndex].Width, textures[SelectedIndex].Height);

            // wybranie następnego elementu z ekwipunku
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && _pull)
            {
                if (++SelectedIndex >= textures.Count)
                    SelectedIndex = 0;
                _pull = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Tab))
            {
                _pull = true;
            }

            if (IsVisible)
            {
                _itemPosition = new Point(gameObject.Position.X + 5, gameObject.Position.Y - topMargin);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(_itemTexture, new Rectangle(_itemPosition, _itemSize), Color.White);
            }
        }
    }
}
