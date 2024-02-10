using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class MoveDown : ICommand
    {
        private Player player;

        public MoveDown(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.MoveDown();
        }
    }
}