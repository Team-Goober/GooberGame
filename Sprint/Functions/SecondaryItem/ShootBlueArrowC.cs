using Sprint.Interfaces;
using Sprint.Levels;
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
            BlueArrow projectile = factory.CreateBlueArrow();
            projectile.GetObjManagement(objManager);
            objManager.Add(projectile);
        }
    }
}
