
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Items.PowerupCommands
{
    internal class HealPlayerCommand : IPowerupCommand
    {

        int amount;

        public HealPlayerCommand(int amount)
        {
            this.amount = amount;
        }
       
        public void Execute(Player player)
        {
            player.TakeDamage(-amount);
        }
    }
}
