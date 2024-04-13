﻿using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.SecondaryItem
{
    internal class SelectSlotCommand : ICommand
    {
        InventoryState receiver;

        public SelectSlotCommand(InventoryState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.SelectSlot();
        }
    }
}