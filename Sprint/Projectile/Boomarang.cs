using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using Sprint.Collision;

namespace Sprint.Projectile
{
    internal class Boomarang : IProjectile, IMovingCollidable
    {
        ISprite sprite;
        Vector2 position;
        Vector2 originalPosition;
        Vector2 velocity;
        Vector2 direction;

        const float speed = 400;
        const float backSpeed = -400;

        public Rectangle BoundingBox => new((int)(position.X - 4 * 3),
            (int)(position.Y - 4 * 3),
            8, 8);

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PROJECTILE };

        public Boomarang(ISprite sprite, Vector2 startPos, Vector2 newDirection)
        {
            this.position = startPos;
            this.originalPosition = this.position;
            this.direction = newDirection;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            this.sprite = sprite;
        }

        private void returnBack(Vector2 currentPosition)
        {
            float difX = Math.Abs(currentPosition.X - this.originalPosition.X);
            float difY = Math.Abs(currentPosition.Y - this.originalPosition.Y);
            if(difX > 200 || difY > 200)
            {
                velocity = Vector2.Normalize(direction) * backSpeed;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            sprite.Draw(spriteBatch, position, gameTime, rotation);
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly one way, then flip to the other way
            returnBack(position);
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

            sprite.Update(gameTime);
        }

        public void Move(Vector2 distance)
        {
            position += distance;
        }
    }
}
