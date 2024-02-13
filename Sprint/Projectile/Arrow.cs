using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class Arrow : IProjectile
    {

        ISprite sprite;
        Vector2 position;
        Vector2 velocity;

        const float speed = 500;

        public Arrow(Texture2D sheet, Vector2 startPos, Vector2 direction)
        {
            position = startPos;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            sprite = new AnimatedSprite(sheet);
            IAtlas arrowAtlas = new SingleAtlas(new Rectangle(0, 45, 16, 5), new Vector2(0, 0));
            sprite.RegisterAnimation("arrow", arrowAtlas);
            sprite.SetAnimation("arrow");
            sprite.SetScale(4);
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

            sprite.Update(gameTime);
        }
    }
}
