using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class OpenDeath : ICommand
    {
        private DungeonState receiver;

        public OpenDeath(DungeonState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.DeathScreen();
        }
    }
}
