
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootArrowCommand : ICommand
    {

        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootArrowCommand(SimpleProjectileFactory newFactory, GameObjectManager newObjManager)
        {
            this.factory = newFactory;
            this.objManager = newObjManager;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateArrow();
            objManager.Add(projectile);
        }
    }
}
