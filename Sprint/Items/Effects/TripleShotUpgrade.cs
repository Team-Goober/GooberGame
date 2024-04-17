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
            // Shoot first shot
            baseAbility.Activate();
            // Rotate one way and shoot
            projs.SetDirection(Vector2.Transform(originalDir, Matrix.CreateRotationZ((float)(Math.PI / 6))));
            baseAbility.Activate();
            // Rotate other way and shoot
            projs.SetDirection(Vector2.Transform(originalDir, Matrix.CreateRotationZ((float)(- Math.PI / 6))));
            baseAbility.Activate();
            // Reset to original direction
            projs.SetDirection(originalDir);
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
            return new TripleShotUpgrade();
        }
    }
}
