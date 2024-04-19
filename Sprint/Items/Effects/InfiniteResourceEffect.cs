
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class InfiniteResourceEffect : IEffect
    {

        private string label;
        private bool prevState; // Preserve previous unlimited level
        private IStackedPowerup pup; // Powerup to make infinite

        public InfiniteResourceEffect(string label)
        {
            this.label = label;
        }

        public void Execute(Player player)
        {
            // Set powerup to be infinite
            pup = player.GetInventory().GetPowerup(label) as IStackedPowerup;
            prevState = pup.GetUnlimited();
            pup.SetUnlimited(true);
        }

        public void Reverse(Player player)
        {
            // Reset powerup infiniteness to what it was previously
            pup.SetUnlimited(prevState);
        }

    }
}
