using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;
using System.Diagnostics;

namespace Sprint.Factory.HUD
{
    internal class HUDFrame : IHUD
    {
        protected ISprite sprite;
        protected Vector2 position;

        public HUDFrame(ISprite sprite, Vector2 position) 
        {
            this.sprite = sprite;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            //Nothing
        }
    }
}
