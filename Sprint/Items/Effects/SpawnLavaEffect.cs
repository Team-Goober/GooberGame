using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class SpawnLavaEffect : IEffect
    {

        private Queue<LavaBucket> movingHooks = new(); // List of hooks that have not yet pierced something

        public void Execute(Player player)
        {
            // Create new projectile
            LavaBucket lava = (LavaBucket)player.GetProjectileFactory().CreateFromString("lavabucket", player);
            lava.Create();
            movingHooks.Enqueue(lava);
        }

        public void Reverse(Player player)
        {
            // Retract existing projectiles
            LavaBucket h;
            while (movingHooks.TryDequeue(out h))
            {
                h.Retract();
            }
        }

        public Queue<LavaBucket> GetHooks()
        {
            return movingHooks;
        }

    }
}
