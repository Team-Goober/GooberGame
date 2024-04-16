using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.SecondaryItem
{
    internal class DropSlotCommand : ICommand
    {
        InventoryState receiver;

        public DropSlotCommand(InventoryState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            // Tell inventory to delete current selected item
            receiver.DropSlot();
        }
    }
}
