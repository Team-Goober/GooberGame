using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory : IProjectileFactory
    {

        ISprite projSprite;
        float speed;
        Vector2 position;
        GameObjectManager objectManager;
        Vector2 direction;

        public SimpleProjectileFactory(GameObjectManager entityManager, ISprite projSprite, float speed, Vector2 position)
        {
            this.projSprite = projSprite;
            this.speed = speed;
            this.position = position;
            this.objectManager = entityManager;
        }

        public void Create()
        {
            // Start of projectile with correct initial position and velocity
            Vector2 velocity;
            if (direction.Length() == 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = Vector2.Normalize(direction) * speed;
            }
            Debug.WriteLine(velocity);
            IProjectile proj = new SimpleProjectile(projSprite, position, velocity);

            // Add projectile to game's entity manager
            objectManager.Add(proj);
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }
    }
}
