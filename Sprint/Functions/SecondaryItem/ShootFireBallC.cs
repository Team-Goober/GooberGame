using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
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
