using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Items;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Commands.SecondaryItem
{
    internal class UseBWeaponCommand : ICommand
    {

        private SimpleProjectileFactory factory;
        private Inventory inventory;

        public UseBWeaponCommand(SimpleProjectileFactory factory, Inventory inventory)
        {
            this.factory = factory;
            this.inventory = inventory;
        }

        public void Execute()
        {
            // Handle each possible item separately
            switch (inventory.GetSelection())
            {
                // Create and shoot correct projectile
                case ItemType.Boomerang:
                    factory.CreateBoomarang().Create();
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
                case ItemType.Bow:
                    if (inventory.HasItem(ItemType.Rupee))
                    {
                        factory.CreateArrow().Create();
                        // Must use up a rupee
                        inventory.ConsumeItem(ItemType.Rupee);
                    }
                    break;
                case ItemType.Candle:
                    factory.CreateFireBall().Create();
                    break;
                default:
                    break;
            }

        }
    }
}
