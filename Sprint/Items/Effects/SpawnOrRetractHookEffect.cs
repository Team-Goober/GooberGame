using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class SpawnOrRetractHookEffect : IEffect
    {

        private Queue<Hook> movingHooks = new(); // List of hooks that have not yet pierced something

        public void Execute(Player player)
        {
            // Create new projectile
            Hook hook = (Hook)player.GetProjectileFactory().CreateFromString("hook", player);
            hook.Create();
            movingHooks.Enqueue(hook);
        }

        public void Reverse(Player player)
        {
            // Retract existing projectiles
            Hook h;
            while (movingHooks.TryDequeue(out h))
            {
                h.Retract();
            }
        }

        public Queue<Hook> GetHooks()
        {
            return movingHooks;
        }

        public IEffect Clone()
        {
            return new SpawnOrRetractHookEffect() { };
        }
    }
}
