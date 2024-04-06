using Sprint.Interfaces;

namespace Sprint.Functions.DeathState
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
