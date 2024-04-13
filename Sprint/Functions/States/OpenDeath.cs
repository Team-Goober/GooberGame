using Sprint.Interfaces;

namespace Sprint.Functions.States
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
