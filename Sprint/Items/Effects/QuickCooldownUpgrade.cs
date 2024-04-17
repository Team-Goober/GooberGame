using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;

namespace Sprint.Items.Effects
{
    internal class QuickCooldownUpgrade : IUpgradeEffect
    {

        IAbility baseAbility;

        public void Execute(Player player)
        {
            // Instantly pass half of the cooldown duration
            ((ICooldownPowerup)baseAbility).SetTimeLeft(((ICooldownPowerup)baseAbility).GetDuration() / 2f);
            // Do base ability
            baseAbility.Activate();
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }
    }
}
