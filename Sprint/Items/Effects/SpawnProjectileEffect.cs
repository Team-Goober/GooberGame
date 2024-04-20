using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using System;

namespace Sprint.Items.Effects
{
    internal class SpawnProjectileEffect : IEffect
    {

        private string projName;

        public SpawnProjectileEffect(string projName)
        {
            this.projName = projName;
        }

        public void Execute(Player player)
        {
            // Create projectile based on string and add to room
            player.GetProjectileFactory().CreateFromString(projName, player).Create();
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

    }
}
