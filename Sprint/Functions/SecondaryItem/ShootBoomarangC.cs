using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class ShootBoomarangC : ICommand
    {
        private SimpleProjectileFactory factory;

        public ShootBoomarangC(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateBoomarang();
            projectile.Create();
        }
    }
}
