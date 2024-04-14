using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class CompassEffect : IEffect
    {
        public void Execute(Player player)
        {
            player.GetMap().PlaceCompass();
        }
    }
}
