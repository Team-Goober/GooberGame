using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Collision;
using System.Runtime.Serialization;
using Sprint.Levels;

namespace Sprint.Projectile
{
    internal class Bomb : IProjectile, IMovingCollidable
    {
        ISprite sprite;
        Vector2 position;

        Timer countdownTimer;
        Timer explosionTimer;

        GameObjectManager objManager;

        public Rectangle BoundingBox => new((int)(position.X - 4 * 3),
            (int)(position.Y - 4 * 3),
            8, 8);

        public CollisionTypes[] CollisionType
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

        public Bomb(ISprite sprite, Vector2 startPos)
        {
            this.position = startPos;
            
            this.sprite = sprite;

            countdownTimer = new Timer(1);
            explosionTimer = new Timer(1);

            countdownTimer.Start();

        }

        public void SetObjManagement(GameObjectManager newObjManager)
        {
            this.objManager = newObjManager;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Handle timers for detonation
            countdownTimer.Update(gameTime);
            if (countdownTimer.JustEnded)
            {
                sprite.SetAnimation("explosion");
                explosionTimer.Start();
            }
            explosionTimer.Update(gameTime);
            if (explosionTimer.JustEnded)
            {
                objManager.Remove(this);
            }

            sprite.Update(gameTime);

        }

        public void Move(Vector2 distance)
        {
            position += distance;
        }
    }
}
