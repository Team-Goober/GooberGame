using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class MapEffect : IEffect
    {
        public void Execute(Player player)
        {
            // Tell player to tell dungeon to reveal the whole map
            player.GetMap().RevealAll(true);
        }

        public void Reverse(Player player)
        {
            // Tell player to tell dungeon to unreveal the whole map
            player.GetMap().RevealAll(false);
        }

    }
}
