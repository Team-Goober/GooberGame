
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Items;

namespace Sprint.Functions.SecondaryItem
{
    internal class EarnCommand : ICommand
    {

        private IStackedPowerup rupees;
        private int number;

        public EarnCommand(Player player, int num)
        {
            rupees = player.GetInventory().GetPowerup(Inventory.RupeeLabel) as IStackedPowerup;
            number = num;
        }

        public void Execute()
        {
            // Add 1 rupee to player
            rupees.AddAmount(number);
        }
    }
}
