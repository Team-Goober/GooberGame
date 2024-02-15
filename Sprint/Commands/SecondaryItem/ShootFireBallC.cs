using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootFireBallC : ICommand
    {
        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootFireBallC(SimpleProjectileFactory newFactory, GameObjectManager newObjManager)
        {
            this.factory = newFactory;
            this.objManager = newObjManager;
        }

        public void Execute()
        {
            FireBall projectile = factory.CreateFireBall();
            projectile.GetobjMangement(objManager);
            objManager.Add(projectile);
        }
    }
}
