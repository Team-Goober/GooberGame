
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class HealPlayerEffect : IEffect
    {

        int amount;

        public HealPlayerEffect(int amount)
        {
            this.amount = amount;
        }
       
        public void Execute(Player player)
        {
            // Heal player
            player.Heal(amount);
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }
    }
}
