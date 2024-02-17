using Sprint.Interfaces;
using Sprint.Projectile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Commands
{
    internal class RemoveObject : ICommand
    {

        private IProjectile projectile;
        private GameObjectManager objectManager;

        private float endTime;
        private float time;

        public RemoveObject(IProjectile newProjectile, GameObjectManager newObjectManager, float newTime, float newEndTime)
        {
            projectile = newProjectile;
            objectManager = newObjectManager;

            endTime = newEndTime;
            time = newTime;
        }

        public void Execute()
        {
            if (time > endTime)
            {
                objectManager.Remove(projectile);
            }
        }

    }
}
