using Sprint.Interfaces;
using System.Numerics;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory : IProjectileFactory
    {

        ISprite projSprite;
        float speed;
        Vector2 position;

        public SimpleProjectileFactory(ISprite projSprite, float speed, Vector2 position)
        {
            this.projSprite = projSprite;
            this.speed = speed;
            this.position = position;
        }

        public void Create(Vector2 direction)
        {
            Vector2 velocity = Vector2.Normalize(direction) * speed;
            IProjectile proj = new SimpleProjectile(projSprite, position + velocity * 10, velocity);
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }
    }
}
