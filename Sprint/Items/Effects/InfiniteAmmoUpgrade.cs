using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class InfiniteAmmoUpgrade : IUpgradeEffect
    {

        IAbility baseAbility;

        public void Execute(Player player)
        {
            // Add ammo to refund last use
            ((IStackedPowerup)baseAbility).AddAmount(1);
            // Do base ability
            baseAbility.Activate();
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }
    }
}

