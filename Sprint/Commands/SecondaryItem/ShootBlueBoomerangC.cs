using Sprint.Interfaces;
using Sprint.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootBlueBoomerangC : ICommand
    {
        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootBlueBoomerangC(SimpleProjectileFactory newFactory, GameObjectManager newObjManager)
        {
            this.factory = newFactory;
            this.objManager = newObjManager;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateBlueBoomerang();
            objManager.Add(projectile);
        }
    }
}
