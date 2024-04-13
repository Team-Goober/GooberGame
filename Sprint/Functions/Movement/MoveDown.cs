using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
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