using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class ShootBoomerangC : ICommand
    {
        private SimpleProjectileFactory factory;

        public ShootBoomerangC(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateBoomerang();
            projectile.Create();
        }
    }
}
