using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Functions
{
    internal class OpenInventoryCommand : ICommand
    {

        private DungeonState receiver;

        public OpenInventoryCommand(DungeonState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.OpenInventory();
        }
    }
}
