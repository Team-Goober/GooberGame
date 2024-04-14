using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class MapEffect : IEffect
    {
        public void Execute(Player player)
        {
            player.GetMap().RevealAll();
        }
    }
}
