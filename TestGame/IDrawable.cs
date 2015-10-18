﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    public interface IDrawableTextLabel
    {
        void LoadContent(ContentManager contentManager);
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime,string text);
    }
}