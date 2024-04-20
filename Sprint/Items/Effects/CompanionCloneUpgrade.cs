using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class CompanionCloneUpgrade : IUpgradeEffect
    {

        private int clones;
        private IStackedPowerup basePowerup;

        public CompanionCloneUpgrade(int clones)
        {
            this.clones = clones;
        }

        public void Execute(Player player)
        {
            // Apply once
            basePowerup.Apply(player);
            // Activate base to spawn for the number of clones
            for(int i = 0; i< clones; i++)
            {
                // Run execute effect instead of directly applying
                basePowerup.GetEffect().Execute(player);
                // Add one to reflect new companion
                basePowerup.AddAmount(1);
            }
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

        public void SetBase(IPowerup powerup)
        {
            basePowerup = (IStackedPowerup)powerup;
            // Add clones for each existing companion
            int currQuantity = ((IStackedPowerup)basePowerup).Quantity();
            for (int i = 0; i < clones * currQuantity; i++)
            {
                // Base already has player set, so null player shouldn't matter
                // Run execute effect instead of directly applying
                basePowerup.GetEffect().Execute(null);
                // Add one to reflect new companion
                basePowerup.AddAmount(1);
            }
        }

    }
}
