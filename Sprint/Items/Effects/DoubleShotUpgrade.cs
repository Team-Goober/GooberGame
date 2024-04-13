using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Items.Effects
{
    internal class DoubleShotUpgrade : IUpgradeEffect
    {

        SpawnProjectileEffect baseEffect;

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            baseEffect.Execute(player);
            projs.SetDirection(Directions.Opposite(player.Facing));
            baseEffect.Execute(player);
            projs.SetDirection(player.Facing);
        }

        public void SetBase(IEffect baseEffect)
        {
            this.baseEffect = (SpawnProjectileEffect)baseEffect;
        }
    }
}
