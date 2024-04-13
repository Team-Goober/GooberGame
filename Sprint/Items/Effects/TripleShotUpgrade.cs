using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System;

namespace Sprint.Items.Effects
{
    internal class TripleShotUpgrade : IUpgradeEffect, IProjectileEffect
    {

        IProjectileEffect baseEffect;

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            Vector2 originalDir = projs.GetDirection();
            baseEffect.Execute(player);
            projs.SetDirection(Vector2.Transform(originalDir, Matrix.CreateRotationZ((float)(Math.PI / 6))));
            baseEffect.Execute(player);
            projs.SetDirection(Vector2.Transform(originalDir, Matrix.CreateRotationZ((float)(- Math.PI / 6))));
            baseEffect.Execute(player);
            projs.SetDirection(originalDir);
        }

        public void SetBase(IEffect baseEffect)
        {
            this.baseEffect = (IProjectileEffect)baseEffect;
        }
    }
}
