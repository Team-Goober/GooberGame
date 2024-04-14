using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface IUpgradeEffect : IEffect
    {

        public void SetBase(IPowerup powerup);

    }
}
