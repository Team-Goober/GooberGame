using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class ShootBombC : ICommand
    {
        private SimpleProjectileFactory factory;

        public ShootBombC(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            Bomb projectile = factory.CreateBomb();
            projectile.Create();
        }
    }
}
