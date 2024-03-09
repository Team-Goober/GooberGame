using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
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
