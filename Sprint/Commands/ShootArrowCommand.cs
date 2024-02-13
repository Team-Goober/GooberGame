
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Commands
{
    internal class ShootArrowCommand : ICommand
    {

        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootArrowCommand(SimpleProjectileFactory factory, GameObjectManager objManager)
        {
            this.factory = factory;
            this.objManager = objManager;
        }

        public void Execute()
        {
            Arrow arrow = factory.CreateArrow();
            objManager.Add(arrow);
        }
    }
}
