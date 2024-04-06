using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Functions.SecondaryItem;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal abstract class DissipatingProjectile : SimpleProjectile
    {

        protected int travel;
        protected Vector2 startPos;
        protected Vector2 velocity;
        public readonly double dmg = 0;

        public DissipatingProjectile(ISprite sprite, Vector2 startPos, Vector2 direction, int speed, int travel, bool isEnemy, Room room) :
            base(sprite, startPos, isEnemy, room)
        {
            this.travel = travel;
            this.startPos = startPos;
            velocity = Vector2.Normalize(direction) * speed;
        }

        public override void Update(GameTime gameTime)
        {
            // Move linearly
            if ((position - startPos).Length() > travel)
            {
                Dissipate();
            }
            else
            {
                position += velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            sprite.Draw(spriteBatch, position, gameTime, rotation);
        }

        public abstract void Dissipate();

    }
}
