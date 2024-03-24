using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Commands;
using System;
using Sprint.Levels;
using Sprint.Collision;
using Sprint.Functions.SecondaryItem;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class FireBall : DissipatingProjectile
    {

        private const int SPEED = 300;
        private const int TRAVEL = 100;
        private Timer sitTimer;

        public FireBall(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, SceneObjectManager objectManager) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, objectManager)
        {
            sitTimer = new Timer(1);
        }

        public override void Dissipate()
        {
            if (sitTimer.Ended)
            {
                sitTimer.Start();
                velocity = Vector2.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            sitTimer.Update(gameTime);

            base.Update(gameTime);

            if (sitTimer.JustEnded)
            {
                Delete();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }
    }
}
