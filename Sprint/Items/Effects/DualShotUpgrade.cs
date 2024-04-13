using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;

namespace Sprint.Items.Effects
{
    internal class DualShotUpgrade : IUpgradeEffect, IProjectileEffect
    {

        IProjectileEffect baseEffect;

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            baseEffect.Execute(player);
            projs.SetDirection(Directions.Opposite(projs.GetDirection()));
            baseEffect.Execute(player);
            projs.SetDirection(Directions.Opposite(projs.GetDirection()));
        }

        public void SetBase(IEffect baseEffect)
        {
            this.baseEffect = (IProjectileEffect)baseEffect;
        }
    }
}
