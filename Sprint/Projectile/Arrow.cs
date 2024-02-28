using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Commands;
using System;

namespace Sprint.Projectile
{
    internal class Arrow : IProjectile
    {

        ISprite sprite;
        ISprite smoke;
        Vector2 position;
        Vector2 startPosition;
        Vector2 velocity;

        private float time;

        GameObjectManager objManager;

        const float speed = 300;
        const float travel = 200;

        public Arrow(ISprite sprite, ISprite smoke, Vector2 startPos, Vector2 direction)
        {
            // Use to correct spawn position
            this.position = startPos; // + Vector2.Normalize(direction) * 40;
            this.startPosition = position;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            this.sprite = sprite;
            this.smoke = smoke;

        }

        private float distance()
        {
            float disX = Math.Abs(position.X - startPosition.X);
            float disY = Math.Abs(position.Y - startPosition.Y);

            if (disX != 0.0)
            {
                return disX;
            }

            return disY;
        }

        public void GetObjManagement(GameObjectManager newObjManager)
        {
            this.objManager = newObjManager;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            if(distance() < travel)
            {
                sprite.Draw(spriteBatch, position, gameTime, rotation);
            } else
            {
                smoke.Draw(spriteBatch, position, gameTime);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly
            if (distance() < travel)
            {
                position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                time += (float)(gameTime.ElapsedGameTime.TotalSeconds);
                new RemoveObject(this, objManager, time, 0.5f).Execute();
            }

            sprite.Update(gameTime);
        }
    }
}
