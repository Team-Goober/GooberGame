using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootArrowCommand : ICommand
    {

        private SimpleProjectileFactory factory;

        public ShootArrowCommand(SimpleProjectileFactory newFactory)
        {
            this.factory = newFactory;
        }

        public void Execute()
        {
            Arrow projectile = factory.CreateArrow();
            projectile.Create();
        }
    }
}
