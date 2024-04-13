using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface IUpgradePowerup : IPowerup
    {

        public void SetUpgradeOptions(List<string> bases);

    }
}
