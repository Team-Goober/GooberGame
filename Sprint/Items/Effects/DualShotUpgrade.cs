using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;

namespace Sprint.Items.Effects
{
    internal class DualShotUpgrade : IUpgradeEffect
    {

        IAbility baseAbility;

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            // Shoot first shot
            baseAbility.Activate();
            // Flip shooting direction then shoot backwards
            projs.SetDirection(Directions.Opposite(projs.GetDirection()));
            baseAbility.Activate();
            // Flip direction back
            projs.SetDirection(Directions.Opposite(projs.GetDirection()));
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

        public IEffect Clone()
        {
            return new DualShotUpgrade();
        }
    }
}
