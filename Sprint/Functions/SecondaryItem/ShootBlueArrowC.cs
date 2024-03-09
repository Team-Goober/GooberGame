using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootBlueArrowC : ICommand
    {
        private SimpleProjectileFactory factory;

        public ShootBlueArrowC(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            BlueArrow projectile = factory.CreateBlueArrow();
            projectile.Create();
        }
    }
}
