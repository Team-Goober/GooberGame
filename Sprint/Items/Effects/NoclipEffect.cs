using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class NoclipEffect : IEffect
    {

        public void Execute(Player player)
        {
            // Enable player shield
            player.SetNoclip(true);
        }

        public void Reverse(Player player)
        {
            // Disable player shield
            player.SetNoclip(false);
        }

    }
}
