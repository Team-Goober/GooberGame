using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Items;
using Sprint.Projectile;
using System.Diagnostics;

namespace Sprint.Functions.SecondaryItem
{
    internal class ReleaseWeaponCommand : ICommand
    {

        private Inventory inventory;
        private int box; // The powerup box to bind to

        public ReleaseWeaponCommand(Player player, int box)
        {
            this.inventory = player.GetInventory();
            this.box = box;
        }

        public void Execute()
        {
            IAbility bp = inventory.GetSelection(box);
            // Only release if a powerup is in the chosen box and activated
            if(bp != null && bp.IsActive())
            {
                // Complete the behavior
                bp.Complete();
            }
        }
    }
}
