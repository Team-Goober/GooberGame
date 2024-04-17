
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class HealPlayerEffect : IEffect
    {

        public double amount;
       
        public void Execute(Player player)
        {
            // Heal player
            player.Heal(amount);
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

        public IEffect Clone()
        {
            return new HealPlayerEffect() { amount = amount };
        }
    }
}
