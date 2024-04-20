using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class SpawnLavaEffect : IEffect
    {

        public void Execute(Player player)
        {
            // Create new projectile
            LavaBucket lava = (LavaBucket)player.GetProjectileFactory().CreateFromString("lavabucket", player);
            lava.Create();
        }

        public void Reverse(Player player)
        {
            // Don't do anything
        }


    }
}
