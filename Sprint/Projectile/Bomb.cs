using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Collision;
using System.Runtime.Serialization;
using Sprint.Levels;

namespace Sprint.Projectile
{
    internal class Bomb : DissipatingProjectile
    {

        Timer explosionTimer;

        public override CollisionTypes[] CollisionType
        {
            get
            {
                if (!explosionTimer.Ended)
                {
                    return new CollisionTypes[] { CollisionTypes.EXPLOSION, CollisionTypes.PROJECTILE };
                }
                else
                {

                    return new CollisionTypes[] { CollisionTypes.BOMB, CollisionTypes.PROJECTILE };
                }
            }
        }

        private const int SPEED = 150;
        private const int TRAVEL = 50;

        public Bomb(ISprite sprite, Vector2 startPos, Vector2 direction, GameObjectManager objManager) :
            base(sprite, startPos, direction, SPEED, TRAVEL, objManager)
        {
            explosionTimer = new Timer(1);
        }

        public override void Dissipate()
        {
            if (explosionTimer.Ended)
            {
                sprite.SetAnimation("explosion");
                explosionTimer.Start();
                velocity = Vector2.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle timers for detonation
            explosionTimer.Update(gameTime);
            if (explosionTimer.JustEnded)
            {
                objManager.Remove(this);
            }

            base.Update(gameTime);

        }
    }
}
