using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class ShieldEffect : IEffect
    {

        public void Execute(Player player)
        {
            // Enable player shield
            player.SetShielded(true);
        }

        public void Reverse(Player player)
        {
            // Disable player shield
            player.SetShielded(false);
        }

        public IEffect Clone()
        {
            return new ShieldEffect();
        }
    }
}
