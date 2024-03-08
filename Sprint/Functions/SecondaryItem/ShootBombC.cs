using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootBombC : ICommand
    {
        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootBombC(SimpleProjectileFactory newFactory, GameObjectManager newObjManager)
        {
            this.factory = newFactory;
            this.objManager = newObjManager;
        }

        public void Execute()
        {
            Bomb projectile = factory.CreateBomb();
            projectile.SetObjManagement(objManager);
            objManager.Add(projectile);
        }
    }
}
