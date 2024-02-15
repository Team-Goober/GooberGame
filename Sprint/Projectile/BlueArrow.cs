using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;

namespace Sprint.Projectile
{
    internal class BlueArrow : IProjectile
    {
        ISprite sprite;
        Vector2 position;
        Vector2 velocity;

        const float speed = 300;

        public BlueArrow(Texture2D sheet, Vector2 startPos, Vector2 direction)
        {
            this.position = startPos;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            sprite = new AnimatedSprite(sheet);
            IAtlas right = new SingleAtlas(new Rectangle(0, 125, 16, 5), new Vector2(6, 2.5f));

            sprite.RegisterAnimation("right", right);


            sprite.SetAnimation("right");
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
