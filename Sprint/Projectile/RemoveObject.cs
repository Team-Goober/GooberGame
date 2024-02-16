using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Projectile
{
    internal class RemoveObject : ICommand
    {

        private FireBall projectile;
        private GameObjectManager objectManager;

        float time;

        public RemoveObject(FireBall newProjectile, GameObjectManager newObjectManager, float newTime) 
        {
            this.projectile = newProjectile;
            this.objectManager = newObjectManager;

            this.time = newTime;
        }

        public void Execute()
        {
            if(time > 1)
            {
                objectManager.Remove(projectile);
            }  
        }

    }
}
