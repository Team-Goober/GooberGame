using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class SimpleProjectile : IProjectile
    {

        ISprite sprite;
        Vector2 position;
        Vector2 velocity;

        public SimpleProjectile(ISprite sprite, Vector2 startPos, Vector2 velocity)
        {
            position = startPos;
            this.velocity = velocity;
            this.sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            sprite.Draw(spriteBatch, position, gameTime, rotation);
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
