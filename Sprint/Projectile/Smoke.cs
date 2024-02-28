using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;

namespace Sprint.Projectile
{
    internal class Smoke : IProjectile
    {
        ISprite sprite;
        Vector2 position;

        public Smoke(ISprite sprite, Vector2 startPos) 
        {
            this.position = startPos;

            this.sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
