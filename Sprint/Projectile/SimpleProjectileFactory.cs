using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory : IProjectileFactory
    {

        ISprite projSprite;
        float speed;
        Vector2 position;
        EntityManager entityManager;

        public SimpleProjectileFactory(EntityManager entityManager, ISprite projSprite, float speed, Vector2 position)
        {
            this.projSprite = projSprite;
            this.speed = speed;
            this.position = position;
            this.entityManager = entityManager;
        }

        public void Create(Vector2 direction)
        { 
            // Start of projectile with correct initial position and velocity
            Vector2 velocity = Vector2.Normalize(direction) * speed;
            IEntity proj = new SimpleProjectile(projSprite, position, velocity);

            // Add projectile to game's entity manager
            entityManager.AddEntity(proj);
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }
    }
}
