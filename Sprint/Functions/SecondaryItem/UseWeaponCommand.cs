using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Items;
using Sprint.Projectile;
using System.Diagnostics;

namespace Sprint.Functions.SecondaryItem
{
    internal class UseWeaponCommand : ICommand
    {

        private Inventory inventory;
        private int box; // The powerup box to bind to

        public UseWeaponCommand(Player player, int box)
        {
            this.inventory = player.GetInventory();
            this.box = box;
        }

        public void Execute()
        {
            IAbility bp = inventory.GetSelection(box);
            // Only use if a powerup is in the chosen box
            // Try to ready up the powerup and initialize its effects
            if(bp != null && bp.ReadyUp())
            {
                // If item is able to activate, activate its behavior
                bp.Activate();
            }
        }
    }
}
