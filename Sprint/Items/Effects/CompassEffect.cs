using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class CompassEffect : IEffect
    {
        public void Execute(Player player)
        {
            // Tell player to tell dungeon to place compass on map
            player.GetMap().PlaceCompass();
        }
    }
}
