using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;

namespace Sprint.Items.Effects
{
    internal class UpgradeProjectileUpgrade : IUpgradeEffect
    {

        IAbility baseAbility;
        private string projectile;
        private bool prev; // Last level of upgrade

        public UpgradeProjectileUpgrade(string proj)
        {
            projectile = proj;
        }

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            // Upgrade type
            prev = projs.GetUpgraded(projectile);
            projs.SetUpgraded(projectile, true);
            // Shoot
            baseAbility.Activate();
            // Return upgrade level
            projs.SetUpgraded(projectile, prev);
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }

        public void Reverse(Player player)
        {
            // Reverse upgrade level if interrupted
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            projs.SetUpgraded(projectile, prev);
        }
    }
}
