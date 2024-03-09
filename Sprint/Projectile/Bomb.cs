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
                CollisionTypes[] types = new CollisionTypes[2];
                types[0] = explosionTimer.Ended ? CollisionTypes.BOMB : CollisionTypes.EXPLOSION;
                types[1] = isEnemy ? CollisionTypes.ENEMY_PROJECTILE : CollisionTypes.PROJECTILE;
                return types;
            }
        }

        private const int SPEED = 150;
        private const int TRAVEL = 50;

        public Bomb(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, GameObjectManager objManager) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, objManager)
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
