using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootBlueArrowC : ICommand
    {
        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootBlueArrowC(SimpleProjectileFactory newFactory, GameObjectManager newObjManager)
        {
            this.factory = newFactory;
            this.objManager = newObjManager;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateBlueArrow();
            objManager.Add(projectile);
        }
    }
}
