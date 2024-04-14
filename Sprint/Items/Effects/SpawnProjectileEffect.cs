using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using System;

namespace Sprint.Items.Effects
{
    internal class SpawnProjectileEffect : IEffect
    {

        string projName;

        public SpawnProjectileEffect(string projName)
        {
            this.projName = projName;
        }

        public void Execute(Player player)
        {
            player.GetProjectileFactory().CreateFromString(projName).Create();
        }
    }
}
