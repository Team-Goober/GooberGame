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

        private SimpleProjectileFactory factory;
        private Inventory inventory;
        private Player player;
        private int box;

        public UseWeaponCommand(Player player, int box)
        {
            this.factory = player.GetProjectileFactory();
            this.inventory = player.GetInventory();
            this.player = player;
            this.box = box;
        }

        public void Execute()
        {
            IAbility bp = inventory.GetSelection(box);
            if(bp != null && bp.ReadyUp())
            {
                bp.Activate();
            }
            // Handle each possible item separately
            /*switch (inventory.GetSelection())
            {
                // Create and shoot correct projectile
                case ItemType.Boomerang:
                    factory.CreateBoomerang().Create();
                    break;
                case ItemType.BlueBoomerang:
                    factory.CreateBlueBoomerang().Create();
                    break;
                case ItemType.Bomb:
                    if (inventory.HasItem(ItemType.Bomb))
                    {
                        factory.CreateBomb().Create();
                        // Must use up a bomb
                        inventory.ConsumeItem(ItemType.Bomb);
                    }
                    break;
                case ItemType.Arrow:
                    // Bow required to shoot
                    if (inventory.HasItem(ItemType.Rupee) && inventory.HasItem(ItemType.Bow))
                    {
                        factory.CreateArrow().Create();
                        // Must use up a rupee
                        inventory.ConsumeItem(ItemType.Rupee);
                    }
                    break;
                case ItemType.BlueArrow:
                    // Bow required to shoot
                    if (inventory.HasItem(ItemType.Rupee) && inventory.HasItem(ItemType.Bow))
                    {
                        factory.CreateBlueArrow().Create();
                        // Must use up a rupee
                        inventory.ConsumeItem(ItemType.Rupee);
                    }
                    break;
                case ItemType.BlueCandle:
                    factory.CreateFireBall().Create();
                    break;
                case ItemType.RedCandle:
                    factory.CreateFireBall().Create();
                    break;
                // Healing items: consume and do negative damage to player
                case ItemType.Meat:
                    if (inventory.HasItem(ItemType.Meat))
                    {
                        player.TakeDamage(-1);
                        inventory.ConsumeItem(ItemType.Meat);
                    }
                    break;
                case ItemType.BluePotion:
                    if (inventory.HasItem(ItemType.BluePotion))
                    {
                        player.TakeDamage(-1);
                        inventory.ConsumeItem(ItemType.BluePotion);
                    }
                    break;
                case ItemType.RedPotion:
                    if (inventory.HasItem(ItemType.RedPotion))
                    {
                        player.TakeDamage(-3);
                        inventory.ConsumeItem(ItemType.RedPotion);
                    }
                    break;
                default:
                    break;
            }*/

        }
    }
}
