using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootBlueBoomerangC : ICommand
    {
        private SimpleProjectileFactory factory;

        public ShootBlueBoomerangC(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateBlueBoomerang();
            projectile.Create();
        }
    }
}
