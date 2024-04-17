using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class ShootFireBallC : ICommand
    {
        private SimpleProjectileFactory factory;

        public ShootFireBallC(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            FireBall projectile = factory.CreateFireBall();
            projectile.Create();
        }
    }
}
