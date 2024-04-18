using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class InfiniteAmmoUpgrade : IUpgradeEffect
    {

        IStackedPowerup baseAbility;

        private bool prevState; // Preserve previous unlimited level

        public void Execute(Player player)
        {
            // No behavior on execute
            ((IAbility)baseAbility).Activate();
        }

        public void Reverse(Player player)
        {
            // Reset powerup infiniteness to what it was previously
            baseAbility.SetUnlimited(prevState);
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility?.SetUnlimited(prevState);
            baseAbility = powerup as IStackedPowerup;

            prevState = baseAbility.GetUnlimited();
            // Set powerup to be infinite
            baseAbility.SetUnlimited(true);
        }

        public IEffect Clone()
        {
            return new InfiniteAmmoUpgrade();
        }

    }
}

