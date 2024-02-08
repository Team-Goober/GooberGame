using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class SimpleProjectile : IEntity
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
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
