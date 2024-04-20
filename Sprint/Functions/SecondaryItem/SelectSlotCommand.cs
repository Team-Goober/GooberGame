using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.SecondaryItem
{
    internal class SelectSlotCommand : ICommand
    {
        InventoryState receiver;
        private int box;

        public SelectSlotCommand(InventoryState receiver, int box)
        {
            this.receiver = receiver;
            this.box = box;
        }

        public void Execute()
        {
            // Make inventory choose the selected slot
            receiver.SelectSlot(box);
        }
    }
}
