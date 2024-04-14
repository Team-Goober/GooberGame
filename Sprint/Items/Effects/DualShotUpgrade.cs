using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;

namespace Sprint.Items.Effects
{
    internal class DualShotUpgrade : IUpgradeEffect, IProjectileEffect
    {

        IAbility baseAbility;

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            baseAbility.Activate();
            projs.SetDirection(Directions.Opposite(projs.GetDirection()));
            baseAbility.Activate();
            projs.SetDirection(Directions.Opposite(projs.GetDirection()));
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }
    }
}
