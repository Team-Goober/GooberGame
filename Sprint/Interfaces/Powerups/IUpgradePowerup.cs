using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces.Powerups
{
    internal interface IUpgradePowerup : IPowerup
    {
        // Set list of base powerups that this upgrade can stack on
        public void SetUpgradeOptions(List<string> bases);

        // Get the label that describes this upgrade
        public string GetTrueLabel();

        // Get the effect for this upgrade, not the base effect
        public IEffect GetTrueEffect();

        // Checks the entire chain of upgrades to see if a certain base or upgrade is present
        public IPowerup FindInChain(string label);

    }
}
