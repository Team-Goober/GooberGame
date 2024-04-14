using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System;

namespace Sprint.Items.Effects
{
    internal class TripleShotUpgrade : IUpgradeEffect
    {

        IAbility baseAbility;

        public void Execute(Player player)
        {
            SimpleProjectileFactory projs = player.GetProjectileFactory();
            Vector2 originalDir = projs.GetDirection();
            baseAbility.Activate();
            projs.SetDirection(Vector2.Transform(originalDir, Matrix.CreateRotationZ((float)(Math.PI / 6))));
            baseAbility.Activate();
            projs.SetDirection(Vector2.Transform(originalDir, Matrix.CreateRotationZ((float)(- Math.PI / 6))));
            baseAbility.Activate();
            projs.SetDirection(originalDir);
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }
    }
}
