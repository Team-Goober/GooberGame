using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System;

namespace Sprint.Items.Effects
{
    internal class SpawnOrRetractHookEffect : IEffect
    {

        private Hook hook;

        public void Execute(Player player)
        {
            // Create new projectile
            hook = (Hook)player.GetProjectileFactory().CreateFromString("hook", player);
            hook.Create();
        }

        public void Reverse(Player player)
        {
            // Retract existing projectile
            hook.Retract();
            hook = null;
        }

        public Hook GetHook()
        {
            return hook;
        }

        public IEffect Clone()
        {
            return new SpawnOrRetractHookEffect() { };
        }
    }
}
