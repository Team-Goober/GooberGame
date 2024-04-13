using Sprint.Interfaces;

namespace Sprint.Functions.States
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
