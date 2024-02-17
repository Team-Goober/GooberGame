﻿using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class ShootBoomarangC : ICommand
    {
        private SimpleProjectileFactory factory;
        private GameObjectManager objManager;

        public ShootBoomarangC(SimpleProjectileFactory newFactory, GameObjectManager newObjManager)
        {
            this.factory = newFactory;
            this.objManager = newObjManager;
        }

        public void Execute()
        {
            IProjectile projectile = factory.CreateBoomarang();
            objManager.Add(projectile);
        }
    }
}
