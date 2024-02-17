using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Commands;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class BlueArrow : IProjectile
    {
        ISprite sprite;
        ISprite smoke;
        Vector2 position;
        Vector2 startPosition;
        Vector2 velocity;

        private float time;

        GameObjectManager objManager;

        const float speed = 300;
        const float travel = 400;

        public BlueArrow(Texture2D sheet, Texture2D smokeT, Vector2 startPos, Vector2 direction)
        {
            //Use to correct spawn position
            this.position = startPos; //+ Vector2.Normalize(direction) * 40;
            this.startPosition = position;

            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }

            sprite = new AnimatedSprite(sheet);
            smoke = new AnimatedSprite(smokeT);
            IAtlas right = new SingleAtlas(new Rectangle(0, 125, 16, 5), new Vector2(6, 2.5f));
            IAtlas smokeAtlas = new SingleAtlas(new Rectangle(0, 0, 7, 8), new Vector2(3.5f, 4));
            sprite.RegisterAnimation("right", right);
            smoke.RegisterAnimation("smoke", smokeAtlas);

            sprite.SetAnimation("right");
            smoke.SetAnimation("smoke");

            sprite.SetScale(4);
            smoke.SetScale(4);
        }

        public float Distance()
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

            if (Distance() < travel)
            {
                sprite.Draw(spriteBatch, position, gameTime, rotation);
            }
            else
            {
                smoke.Draw(spriteBatch, position, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            // Move linearly
            if (Distance() < travel)
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
