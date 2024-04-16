using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface IUpgradeEffect : IEffect
    {

        // Give a base powerup for this effect to modify behavior of
        public void SetBase(IPowerup powerup);

    }
}
