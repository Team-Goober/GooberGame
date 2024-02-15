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

        public BlueArrow(Texture2D sheet, Vector2 startPos, Vector2 direction, string spriteDirection)
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
            IAtlas right = new SingleAtlas(new Rectangle(0, 125, 16, 5), new Vector2(-6, -2));
            IAtlas left = new SingleAtlas(new Rectangle(40, 85, 16, 5), new Vector2(6, -2));
            IAtlas up = new SingleAtlas(new Rectangle(45, 120, 5, 16), new Vector2(-4, 8));
            IAtlas down = new SingleAtlas(new Rectangle(5, 80, 5, 16), new Vector2(-2, -6));
            sprite.RegisterAnimation("right", right);
            sprite.RegisterAnimation("left", left);
            sprite.RegisterAnimation("up", up);
            sprite.RegisterAnimation("down", down);

            sprite.SetAnimation(spriteDirection);
            sprite.SetScale(4);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

            sprite.Update(gameTime);
        }
    }
}
