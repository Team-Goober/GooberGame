using Sprint.Characters;
using Sprint.Interfaces;


namespace Sprint.Functions
{
    internal class MoveLeft : ICommand
    {
        private Player player;

        public MoveLeft(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.MoveLeft();
        }
    }
}

