using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Commands;
using System;
using Sprint.Levels;

namespace Sprint.Projectile
{
    internal class FireBall : IProjectile
    {
        ISprite sprite;
        Vector2 position;
        Vector2 startPosition;
        Vector2 direction;
        Vector2 velocity;

        float time;

        GameObjectManager objManager;

        const float speed = 300;

        public FireBall(ISprite sprite, Vector2 startPos, Vector2 newDirection)
        {
            this.position = startPos;
            this.direction = newDirection;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            this.startPosition = position;

            this.sprite = sprite;
        }

        private float distance()
        {
            float disX = Math.Abs(position.X - startPosition.X);
            float disY = Math.Abs(position.Y - startPosition.Y);

            if(disX != 0.0)
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
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly
            if(distance() < 100) 
            {
                position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            } else
            {
                time += (float)(gameTime.ElapsedGameTime.TotalSeconds);
                new RemoveObject(this, objManager, time, 1).Execute();
            }
            
            sprite.Update(gameTime);
        }
    }
}
