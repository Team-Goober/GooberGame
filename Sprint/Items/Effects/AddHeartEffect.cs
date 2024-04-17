using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class AddHeartEffect : IEffect
    {

        public void Execute(Player player)
        {
            // Add a new heart to the player
            player.IncreaseHearts();
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }
    }
}
