using Sprint.Characters;
using Sprint.Interfaces;


namespace Sprint.Commands
{
    internal class MoveRight : ICommand
    {
        private Player player;

        public MoveRight(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.MoveRight();
        }
    }
}