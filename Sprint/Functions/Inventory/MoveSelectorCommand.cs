using Microsoft.Xna.Framework;
using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.Inventory
{
    internal class MoveSelectorCommand : ICommand
    {

        InventoryState receiver;
        Point direction;

        public MoveSelectorCommand(InventoryState inventoryState, Point dir)
        {
            receiver = inventoryState;
            direction = dir;
        }

        public void Execute()
        {
            // Tell inventory state to move selector
            receiver.TryMoveSelector(direction);
        }
    }
}
