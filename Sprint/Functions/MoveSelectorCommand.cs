using Microsoft.Xna.Framework;
using Sprint.GameStates;
using Sprint.Interfaces;
using System.Numerics;

namespace Sprint.Functions
{
    internal class MoveSelectorCommand : ICommand
    {

        InventoryState receiver;
        Point direction;

        public MoveSelectorCommand(InventoryState inventoryState, Point dir) {
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
